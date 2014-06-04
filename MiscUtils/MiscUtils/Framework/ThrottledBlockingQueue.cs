// Copyright 2014 Philip Daniels - http://www.philipdaniels.com/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

/*
Typical usage (single producer, multiple consumers):

    using (var queue = new ThrottledBlockingQueue(4))
    {
       // Use the foreground thread to add all the things to be processed.
       // This happens in serial fashion:
       foreach (var something in BigCollection())
       {
           // Create "something"...
           queue.Add(() => Process(something));
       }
    }
    // When we get to here we know that all tasks have been processed.

    The queue has a capacity which defaults to 4 times the number of workers.
    Producers will block when they call add if the queue is at capacity;
    this is a fail-safe to prevent producers from running far ahead of consumers.
  
    The above works well when you need to do work before you can commence the
    Process() method (for example, you might need to read serially from a resource).
    However, if your data structure already contains everything you need then
    it is equivalent to this loop:

        var options = new ParallelOptions() { MaxDegreeOfParallelism = 4 };
        Parallel.ForEach(BigCollection(), options, something => Process(something));

    You can capture results of each action by using a closure if you are careful.
    You should store the results on each thing that you are looping over so that they
    are all independent, then aggregate when the queue is done.

        queue.Add(() => loopVariable.Result = CalcResult(someData));
 
    This leads to a very functional style. 
 
    References
    ==========
    http://msdn.microsoft.com/en-us/library/ff963553.aspx 
      - online version of the book "Parallel Programming with Microsoft .NET" by
        Colin Campbell, Ralph Johnson, Ade Miller, Stephen Toub
    http://www.albahari.com/threading/
      - Joe Albahari's article on threading. Recommended to read it all. This
        queue uses techniques from the "Parallel Processing" section and the
        "Basic Synchronization" section.
    http://msdn.microsoft.com/en-us/library/dd460693(v=vs.110).aspx
      - "Parallel Programming in the .NET Framework"
        General MSDN portal on parallel programming.
        especially http://msdn.microsoft.com/en-us/library/hh873175(v=vs.110).aspx
        "Task-based Asynchronous Pattern (TAP)"
    http://msdn.microsoft.com/en-us/library/hh191443(v=vs.110).aspx
      - "Asynchronous Programming with Async and Await (C# and Visual Basic)"
*/

namespace MiscUtils.Framework
{
    /// <summary>
    /// A worker queue that will execute actions in the background.
    /// Use it within a using statement, the thread of execution
    /// will not proceed beyond the last } until all actions
    /// have been processedd.
    /// </summary>
    public sealed class ThrottledBlockingQueue : IDisposable
    {
        BlockingCollection<Action> queue;
        CountdownEvent countdown;
#if DEBUG
        int numItemsAdded;
        int numItemsProcessed;
#endif

        /// <summary>
        /// Creates a new queue with a worker count equal to the number of processors
        /// and a capacity 4 times that.
        /// </summary>
        public ThrottledBlockingQueue()
            : this(Environment.ProcessorCount)
        {
        }

        /// <summary>
        /// Creates a new queue with the specified worker count and a capacity
        /// 4 times that.
        /// </summary>
        /// <param name="workerCount">Number of workers to create.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "workerCount*4")]
        public ThrottledBlockingQueue(int workerCount)
            : this(workerCount, workerCount * 4)
        {
        }

        /// <summary>
        /// Creates a new queue with the specified worker count and capacity.
        /// </summary>
        /// <param name="workerCount">Number of workers to create.</param>
        /// <param name="capacity">Capacity of the queue. Producers will be blocked when they
        /// call <c>Add</c> if the queue is at capacity.</param>
        public ThrottledBlockingQueue(int workerCount, int capacity)
        {
            capacity.ThrowIfLessThan(workerCount, "capacity", "It makes no sense to have more workers than capacity.");

            queue = new BlockingCollection<Action>();

            // Create and start a separate Task for each consumer.
            for (int i = 0; i < workerCount; i++)
            {
                Task.Factory.StartNew(Consumer);
            }

            // When each consumer is done, they will signal this event.
            countdown = new CountdownEvent(workerCount);
        }

        /// <summary>
        /// Add a new action to the queue. The action should be self-contained
        /// (i.e. thread-safe). This method will block if the queue is at capacity;
        /// this ensures that producers do not run far ahead of consumers (it is
        /// easy to create out of memory errors if that happens).
        /// </summary>
        /// <param name="action">The action.</param>
        public void Add(Action action)
        {
            queue.Add(action);
#if DEBUG
            Interlocked.Increment(ref numItemsAdded);
#endif
        }

        /// <summary>
        /// Disposes of the queue and wait until all remaining actions
        /// have been processed.
        /// </summary>
        public void Dispose() 
        {
            // This stops you adding more actions to the queue
            // (which is impossible anyway because you typically call
            // this from the trailing "}" of a using statement).
            queue.CompleteAdding();

#if DEBUG
            Debug.WriteLine("ThrottledBlockingQueue is waiting...");
#endif

            // This will block until all the consumers call Signal().
            countdown.Wait();

#if DEBUG
            Debug.WriteLine("ThrottledBlockingQueue has finished waiting.");

            Debug.Assert
                (
                numItemsAdded == numItemsProcessed,
                String.Format
                    (
                    CultureInfo.InvariantCulture, 
                    "The number items processed is {0} but the number of items added is {1}",
                    numItemsProcessed, numItemsAdded
                    )
                );
#endif

            queue.Dispose();
            countdown.Dispose();
        }

        void Consumer()
        {
            // This sequence that we’re enumerating will block when no elements
            // are available and will end when CompleteAdding is called. 
            foreach (Action action in queue.GetConsumingEnumerable())
            {
                action();     // Perform task.
#if DEBUG
                Interlocked.Increment(ref numItemsProcessed);
#endif
            }

            // We're done. Decrement the countdown by 1.
            countdown.Signal();
        }
    }
}

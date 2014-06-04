using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiscUtils.Framework
{
    // Step 1: Create an EventArgs subclass. Can use readonly or properties.
    class PriceChangedEventArgs : EventArgs
    {
        public readonly decimal OldPrice;
        public decimal NewPrice { get; private set; }

        public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
        {
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }

    class CanonicalEventExample
    {
        // Step 2: Expose a public event.
        public event EventHandler<PriceChangedEventArgs> PriceChanged;

        // Step 3: Write a protected virtual method to raise the event, called OnXXX.
        // Use a local var for thread safety.
        protected virtual void OnPriceChanged(PriceChangedEventArgs args)
        {
            var e = PriceChanged;
            if (e != null)
                e(this, args);
        }

        void SomeMethod()
        {
            // if (someCondition) then
            OnPriceChanged(new PriceChangedEventArgs(100, 200));
        }
    }
}

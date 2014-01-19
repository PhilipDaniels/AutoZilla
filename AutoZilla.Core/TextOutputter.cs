using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace AutoZilla.Core
{
    /// <summary>
    /// Pastes text to the current foreground window.
    /// <remarks>
    /// Sending multiple pastes in quick succession - for example successive
    /// calls to <code>PasteLine</code> tends to fail. It is better to form
    /// a single string containing new lines and send it in a single Paste call.
    /// The class attempt to alleviate these problems by sleeping between
    /// output attempts which means the former technique is also slower.
    /// </remarks>
    /// </summary>
    public class TextOutputter
    {
        public readonly int InterOutputSleepMilliseconds;
        public readonly int ModifierWaitSleepMilliseconds;
        public readonly IInputSimulator Sim;

        public TextOutputter()
            : this(50, 50)
        {
        }

        public TextOutputter(int interOutputSleepMilliseconds, int modifierWaitSleepMilliseconds)
        {
            InterOutputSleepMilliseconds = interOutputSleepMilliseconds;
            ModifierWaitSleepMilliseconds = modifierWaitSleepMilliseconds;
            Sim = new InputSimulator();
        }

        /// <summary>
        /// Returns true if all modifier keys (Ctrl, Shift, Alt, Win) are up.
        /// This needs to be true before you can send combinations such as Ctrl-V.
        /// </summary>
        /// <returns></returns>
        public bool AllModifierKeysAreUp()
        {
            return Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.CONTROL) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.LCONTROL) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.RCONTROL) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.SHIFT) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.LSHIFT) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.RSHIFT) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.LWIN) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.RWIN) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.MENU) &&     // alt
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.LMENU) &&
                   Sim.InputDeviceState.IsKeyUp(VirtualKeyCode.RMENU);
        }

        /// <summary>
        /// Waits for all modifier keys to go up (as reported by the <code>AllModifierKeysAreUp</code>
        /// function). You cannot send Ctrl-V or various other codes unless the modifier keys are up;
        /// you usually won't get an error but it normally just won't work.
        /// </summary>
        public TextOutputter WaitForModifiersUp()
        {
            while (!AllModifierKeysAreUp())
            {
                Thread.Sleep(ModifierWaitSleepMilliseconds);
            }

            return this;
        }

        /// <summary>
        /// Sends a Ctrl-V "paste" key sequence.
        /// </summary>
        public TextOutputter ControlV()
        {
            if (!AllModifierKeysAreUp())
                throw new BadModifierStateException("At least one modifier key is down. Cannot send Ctrl-V while this is the case.");

            Sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
            return this;
        }

        /// <summary>
        /// Sends a Ctrl-C "copy" key sequence.
        /// </summary>
        public TextOutputter ControlC()
        {
            if (!AllModifierKeysAreUp())
                throw new BadModifierStateException("At least one modifier key is down. Cannot send Ctrl-C while this is the case.");

            Sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
            return this;
        }
        
        /// <summary>
        /// Pastes a line of text. The text is copied to the clipboard (overwriting
        /// any text currently there) and then a Ctrl-V is sent. This appears to be
        /// the fastest and most reliable way of sending text.
        /// </summary>
        /// <param name="text">The text to paste.</param>
        public TextOutputter PasteString(string text)
        {
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
            ControlV();
            Thread.Sleep(InterOutputSleepMilliseconds);
            return this;
        }

        /// <summary>
        /// Pastes a line of text followed by a newline. The text is copied to the
        /// clipboard (overwriting any text currently there) and then a Ctrl-V is sent.
        /// This appears to be the fastest and most reliable way of sending text.
        /// </summary>
        /// <param name="text">The text to paste. A newline will be automatically appended.</param>
        public TextOutputter PasteLine(string text)
        {
            return PasteString(text + Environment.NewLine);
        }

        /// <summary>
        /// Press the UP arrow once.
        /// </summary>
        public TextOutputter Up()
        {
            return Up(1);
        }

        /// <summary>
        /// Press the UP arrow <paramref name="numTimes"/>.
        /// </summary>
        /// <param name="numTimes">The number of times to press the key.</param>
        public TextOutputter Up(int numTimes)
        {
            for (var i = 0; i < numTimes; i++)
                Sim.Keyboard.KeyPress(VirtualKeyCode.UP);
            return this;
        }

        /// <summary>
        /// Press the DOWN arrow once.
        /// </summary>
        public TextOutputter Down()
        {
            return Down(1);
        }

        /// <summary>
        /// Press the DOWN arrow <paramref name="numTimes"/>.
        /// </summary>
        /// <param name="numTimes">The number of times to press the key.</param>
        public TextOutputter Down(int numTimes)
        {
            for (var i = 0; i < numTimes; i++)
                Sim.Keyboard.KeyPress(VirtualKeyCode.DOWN);
            return this;
        }

        /// <summary>
        /// Press the LEFT arrow once.
        /// </summary>
        public TextOutputter Left()
        {
            return Left(1);
        }

        /// <summary>
        /// Press the LEFT arrow <paramref name="numTimes"/>.
        /// </summary>
        /// <param name="numTimes">The number of times to press the key.</param>
        public TextOutputter Left(int numTimes)
        {
            for (var i = 0; i < numTimes; i++)
                Sim.Keyboard.KeyPress(VirtualKeyCode.LEFT);
            return this;
        }

        /// <summary>
        /// Press the RIGHT arrow once.
        /// </summary>
        public TextOutputter Right()
        {
            return Right(1);
        }

        /// <summary>
        /// Press the RIGHT arrow <paramref name="numTimes"/>.
        /// </summary>
        /// <param name="numTimes">The number of times to press the key.</param>
        public TextOutputter Right(int numTimes)
        {
            for (var i = 0; i < numTimes; i++)
                Sim.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
            return this;
        }

        /// <summary>
        /// Move up/down/left/right. Positive numbers mean down/right,
        /// negative numbers mean up/left.
        /// <example>Move(3, 2) moves down 3 lines and right 2 characters.
        /// Move(-2, -5) moves up 2 lines and left 5 characters.
        /// </example>
        /// </summary>
        /// <param name="down">Number of times to move up. Use a negative number to move down.</param>
        /// <param name="right">Number of times to move right. Use a negative number to move left.</param>
        public TextOutputter Move(int down, int right)
        {
            if (down >= 0)
                Down(down);
            else
                Up(-down);

            if (right >= 0)
                Right(right);
            else
                Left(-right);

            return this;
        }

        /// <summary>
        /// Sends a HOME keypress. Typically this will move you to
        /// the beginning of the line.
        /// </summary>
        public TextOutputter Home()
        {
            Sim.Keyboard.KeyPress(VirtualKeyCode.HOME);
            return this;
        }

        /// <summary>
        /// Sends an END keypress. Typically this will move you to
        /// the end of the line.
        /// </summary>
        public TextOutputter End()
        {
            Sim.Keyboard.KeyPress(VirtualKeyCode.END);
            return this;
        }

        /// <summary>
        /// Attempts to select to the end of the current line by sending
        /// a S-END combination. Whether this works depends
        /// on how the program interprets it, but it works for common
        /// Windows text editors.
        /// </summary>
        public TextOutputter SelectToEndOfLine()
        {
            Sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.END);
            return this;
        }

        /// <summary>
        /// Attempts to select the next word in the output program by
        /// sending a CS-RIGHT combination. Whether this works depends
        /// on how the program interprets it, but it works for common
        /// Windows text editors.
        /// </summary>
        public TextOutputter SelectNextWord()
        {
            Sim.Keyboard.ModifiedKeyStroke
                (
                new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT },
                VirtualKeyCode.RIGHT
                );
            return this;
        }

        /*
        /// <summary>
        /// Sends text by simulating typing the string character by character.
        /// <remarks>This seems to be somewhat unreliable, not all apps will receive
        /// this input. Recommendation is to use <code>PasteString</code> instead.</remarks>
        /// </summary>
        /// <param name="text">The text to type.</param>
        [Obsolete("Try using the Paste methods instead. They seem more reliable and faster.")]
        public static void TypeString(string text)
        {
            Sim.Keyboard.TextEntry(text);
        }

        /// <summary>
        /// Sends text by simulating typing the string character by character.
        /// After sending the text, a RETURN keypress is simulated, effectively
        /// sending a newline.
        /// <remarks>This seems to be somewhat unreliable, not all apps will receive
        /// this input. Recommendation is to use <code>PasteString</code> instead.</remarks>
        /// </summary>
        /// <param name="text">The text to type. A RETURN will be automatically appended.</param>
        [Obsolete("Try using the Paste methods instead. They seem more reliable and faster.")]
        public static void TypeLine(string text)
        {
            Sim.Keyboard.TextEntry(text);
            Sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
        */
    }
}

using AutoZilla.Core.GlobalHotKeys;
using AutoZilla.Core.Templates;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    public interface IGlobalHotKeyManager
    {
        /// <summary>
        /// Register a new global hot key. This will throw an exception if the hot
        /// key is already registered.
        /// </summary>
        /// <param name="modifiedKey">The modified key.</param>
        /// <param name="HotKeyCallback">A callback that will be invoked when the HotKey is pressed.</param>
        void Register(ModifiedKey modifiedKey, HotKeyCallback hotKeyCallback);

        /// <summary>
        /// Register a new global hot key. This will throw an exception if the hot
        /// key is already registered.
        /// </summary>
        /// <param name="modifiers">Key modifiers (Shift, Alt etc.) that you want applied.</param>
        /// <param name="key">The key.</param>
        /// <param name="HotKeyCallback">A callback that will be invoked when the HotKey is pressed.</param>
        void Register(Modifiers modifiers, Keys key, HotKeyCallback hotKeyCallback);

        /// <summary>
        /// Registers a <code>TextTemplate</code> for automatic processing. The template
        /// must define a <code>ModifiedKey</code> in order to be invoked. An exception
        /// will be thrown if the key is already registered.
        /// </summary>
        /// <param name="textTemplate">The template to register.</param>
        void Register(TextTemplate textTemplate);

        /// <summary>
        /// Unregisters a single global hot key.
        /// </summary>
        /// <param name="modifiedKey">The <code>ModifiedKey</code> to unregister.</param>
        void Unregister(ModifiedKey modifiedKey);

        /// <summary>
        /// Unregisters a single global hot key.
        /// </summary>
        /// <param name="modifiers">Key modifiers (Shift, Alt etc.) that you want applied.</param>
        /// <param name="key">The key.</param>
        void Unregister(Modifiers modifiers, Keys key);

        /// <summary>
        /// Unregisters a template. The method finds the <code>ModifiedKey</code>
        /// that the template was registered against and unregisters it.
        /// </summary>
        /// <param name="textTemplate">The template to unregister.</param>
        void Unregister(TextTemplate textTemplate);
    }
}

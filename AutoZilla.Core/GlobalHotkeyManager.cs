using AutoZilla.Core.Extensions;
using AutoZilla.Core.GlobalHotKeys;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// Allows you to register global hot keys and be notified when they are pressed.
    /// This type is not thread safe (you must do your own locking if necessary).
    /// </summary>
    public sealed class GlobalHotKeyManager : IGlobalHotKeyManager, IDisposable
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MessageLoopForm MessageLoopForm;
        List<GlobalHotKey> HotKeys;
        List<TextTemplate> AutoTemplates;
        TextOutputter TOUT;

        public GlobalHotKeyManager()
        {
            HotKeys = new List<GlobalHotKey>();
            AutoTemplates = new List<TextTemplate>();
            TOUT = new TextOutputter();

            log.Debug("About to create MessageLoopForm");
            MessageLoopForm = new MessageLoopForm();
            log.Debug("MessageLoopForm created.");

            MessageLoopForm.HotKeyPressed += TheForm_HotKeyPressed;
            MessageLoopForm.HandleDestroyed += MessageLoopForm_HandleDestroyed;
            log.Debug("MessageLoopForm event handlers attached");

            MessageLoopForm.Show();
            log.Debug("MessageLoopForm Shown");
        }

        /// <summary>
        /// Register a new global hot key. This will throw an exception if the hot
        /// key is already registered.
        /// </summary>
        /// <param name="modifiers">Key modifiers (Shift, Alt etc.) that you want applied.</param>
        /// <param name="key">The key.</param>
        /// <param name="HotKeyCallback">A callback that will be invoked when the HotKey is pressed.</param>
        public void Register(Modifiers modifiers, Keys key, HotKeyCallback hotKeyCallback)
        {
            modifiers.ThrowIfNull("modifiers");
            key.ThrowIfNull("key");
            hotKeyCallback.ThrowIfNull("hotKeyCallback");

            string keystr = ModifiedKey.ToString(modifiers, key);
            log.Debug("RegisterGlobalHotKey, Hot key = " + keystr);

            // For multi-threaded apps, we need to be careful to use the form's handle
            // on the same thread as it was initially created.
            if (MessageLoopForm.InvokeRequired)
            {
                MessageLoopForm.Invoke(new MethodInvoker(delegate { InternalRegister(modifiers, key, hotKeyCallback); }));
            }
            else
            {
                InternalRegister(modifiers, key, hotKeyCallback);
            }

            log.Debug(keystr + " successfully registered");
        }

        /// <summary>
        /// Register a new global hot key. This will throw an exception if the hot
        /// key is already registered.
        /// </summary>
        /// <param name="modifiedKey">The modified key.</param>
        /// <param name="HotKeyCallback">A callback that will be invoked when the HotKey is pressed.</param>
        public void Register(ModifiedKey modifiedKey, HotKeyCallback hotKeyCallback)
        {
            modifiedKey.ThrowIfNull("modifiedKey");

            hotKeyCallback.ThrowIfNull("hotKeyCallback");
            Register(modifiedKey.Modifiers, modifiedKey.Key, hotKeyCallback);
        }

        /// <summary>
        /// Registers a <code>TextTemplate</code> for automatic processing. The template
        /// must define a <code>ModifiedKey</code> in order to be invoked. An exception
        /// will be thrown if the key is already registered.
        /// </summary>
        /// <param name="textTemplate">The template to register.</param>
        public void Register(TextTemplate textTemplate)
        {
            textTemplate.ThrowIfNull("template");
            textTemplate.ModifiedKey.ThrowIfNull
                (
                "template.Key",
                String.Format(CultureInfo.InvariantCulture, "The template {0} must have a ModifiedKey in order to be auto-invoked.", textTemplate.Name)
                );

            Register(textTemplate.ModifiedKey, AutoTemplateCallback);
            AutoTemplates.Add(textTemplate);
        }

        void InternalRegister(Modifiers modifiers, Keys key, HotKeyCallback HotKeyCallback)
        {
            var hk = new GlobalHotKey(modifiers, key, MessageLoopForm, HotKeyCallback, true);
            HotKeys.Add(hk);
        }

        void AutoTemplateCallback(ModifiedKey key)
        {
            var template = AutoTemplates.Single(t => t.ModifiedKey == key);
            string replacedText = template.Process();
            TOUT.WaitForModifiersUp();
            TOUT.PasteString(replacedText);
        }

        void InternalUnregister(Modifiers modifiers, Keys key)
        {
            var hk = FindHotKeyAndThrowIfNotFound(modifiers, key);
            hk.Dispose();
            HotKeys.Remove(hk);
        }

        /// <summary>
        /// Unregisters a single global hot key.
        /// </summary>
        /// <param name="modifiers">Key modifiers (Shift, Alt etc.) that you want applied.</param>
        /// <param name="key">The key.</param>
        public void Unregister(Modifiers modifiers, Keys key)
        {
            modifiers.ThrowIfNull("modifiers");
            key.ThrowIfNull("key");

            log.Debug("UnregisterGlobalHotKey, Hot key = " + ModifiedKey.ToString(modifiers, key));

            // For multi-threaded apps, we need to be careful to use the form's handle
            // on the same thread as it was initially created.
            if (MessageLoopForm.InvokeRequired)
            {
                MessageLoopForm.Invoke(new MethodInvoker(delegate { InternalUnregister(modifiers, key); }));
            }
            else
            {
                InternalUnregister(modifiers, key);
            }

            log.Debug("Hot key successfully unregistered");
        }

        /// <summary>
        /// Unregisters a single global hot key.
        /// </summary>
        /// <param name="modifiedKey">The <code>ModifiedKey</code> to unregister.</param>
        public void Unregister(ModifiedKey modifiedKey)
        {
            modifiedKey.ThrowIfNull("key");

            Unregister(modifiedKey.Modifiers, modifiedKey.Key);
        }

        /// <summary>
        /// Unregisters a template. The method finds the <code>ModifiedKey</code>
        /// that the template was registered against and unregisters it.
        /// </summary>
        /// <param name="textTemplate">The template to unregister.</param>
        public void Unregister(TextTemplate textTemplate)
        {
            textTemplate.ThrowIfNull("template");

            var ourTemplate = AutoTemplates.Single(t => t.ModifiedKey == textTemplate.ModifiedKey);
            if (ourTemplate == null)
                return;
            AutoTemplates.Remove(ourTemplate);
            Unregister(ourTemplate.ModifiedKey);
        }

        /// <summary>
        /// Unregisters all HotKeys that were registered by this application. It is not necessary
        /// to call this in normal circumstances, it will be called automatically when your
        /// application terminates.
        /// </summary>
        void UnregisterAllHotKeys()
        {
            log.Debug("Enter UnregisterAllHotKeys");

            if (HotKeys != null)
            {
                foreach (var hk in HotKeys)
                {
                    hk.Dispose();
                }
            }

            log.Debug("Num Keys unregistered = " + HotKeys.Count); 
            
            HotKeys = new List<GlobalHotKey>();
        }

        /// <summary>
        /// This event happens early enough for us to dispose the hot keys successfully
        /// (before our AppDomain/Windows message loop goes to la-la-land.)
        /// </summary>
        void MessageLoopForm_HandleDestroyed(object sender, EventArgs e)
        {
            log.Debug("Got MessageLoopForm_HandleDestroyed event.");
            UnregisterAllHotKeys();
        }

        /// <summary>
        /// Disposes all managed resources.
        /// </summary>
        public void Dispose()
        {
            UnregisterAllHotKeys(); 
            if (MessageLoopForm != null)
            {
                MessageLoopForm.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        ~GlobalHotKeyManager()
        {
            log.Debug("Finalizing the GlobalHotKeyManager class.");
            UnregisterAllHotKeys();
            if (MessageLoopForm != null)
            {
                MessageLoopForm.Dispose();
            }
        }

        /// <summary>
        /// When a hot key is pressed, find the relevant entry and invoke
        /// its callback so the client can do something.
        /// </summary>
        void TheForm_HotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            log.Debug("TheForm_HotKeyPressed, Hot key = " + e.ModifiedKey.ToString());
            var hk = FindHotKeyAndThrowIfNotFound(e.ModifiedKey.Modifiers, e.ModifiedKey.Key);
            log.Debug("About to call the HotKey's callback");
            hk.Callback(e.ModifiedKey);
        }

        GlobalHotKey FindHotKey(Modifiers modifiers, Keys key)
        {
            foreach (var hk in HotKeys)
            {
                if (hk.Modifier == modifiers && hk.Key == (int)key)
                {
                    return hk;
                }
            }

            return null;
        }

        GlobalHotKey FindHotKeyAndThrowIfNotFound(Modifiers modifiers, Keys key)
        {
            var hk = FindHotKey(modifiers, key);
            if (hk == null)
            {
                var e = new UnknownHotKeyException
                    (
                    "Could not find corresponding hot key in list. Application logic error.",
                    modifiers, key
                    );

                log.Error("Could not find corresponding hot key in list. Application logic error.", e);
                throw e;
            }

            return hk;
        }
    }
}

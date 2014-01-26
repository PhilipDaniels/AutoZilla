using AutoZilla.Core.GlobalHotkeys;
using AutoZilla.Core.Templates;
using AutoZilla.Core.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// Allows you to register global hot keys and be notified when they are pressed.
    /// This type is not thread safe (you must do your own locking if necessary).
    /// </summary>
    public class GlobalHotkeyManager : IGlobalHotKeyManager
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MessageLoopForm MessageLoopForm;
        List<GlobalHotkey> HotKeys;
        List<Template> AutoTemplates;
        TextOutputter TOUT;

        public GlobalHotkeyManager()
        {
            HotKeys = new List<GlobalHotkey>();
            AutoTemplates = new List<Template>();
            TOUT = new TextOutputter();

            log.Debug("About to create MessageLoopForm");
            MessageLoopForm = new MessageLoopForm();
            log.Debug("MessageLoopForm created.");

            MessageLoopForm.HotkeyPressed += TheForm_HotkeyPressed;
            MessageLoopForm.HandleDestroyed += MessageLoopForm_HandleDestroyed;
            log.Debug("MessageLoopForm event handlers attached");

            MessageLoopForm.Show();
            log.Debug("MessageLoopForm Shown");
        }

        void InternalRegister(Modifiers modifiers, Keys key, HotkeyCallback hotkeyCallback)
        {
            var hk = new GlobalHotkey(modifiers, key, MessageLoopForm, hotkeyCallback, true);
            HotKeys.Add(hk);
        }

        /// <summary>
        /// Register a new global hot key. This will throw an exception if the hot
        /// key is already registered.
        /// </summary>
        /// <param name="modifiers">Key modifiers (Shift, Alt etc.) that you want applied.</param>
        /// <param name="key">The key.</param>
        /// <param name="hotkeyCallback">A callback that will be invoked when the hotkey is pressed.</param>
        public void Register(Modifiers modifiers, Keys key, HotkeyCallback hotkeyCallback)
        {
            string keystr = ModifiedKey.ToString(modifiers, key);
            log.Debug("RegisterGlobalHotKey, Hot key = " + keystr);

            // For multi-threaded apps, we need to be careful to use the form's handle
            // on the same thread as it was initially created.
            if (MessageLoopForm.InvokeRequired)
            {
                MessageLoopForm.Invoke(new MethodInvoker(delegate { InternalRegister(modifiers, key, hotkeyCallback); }));
            }
            else
            {
                InternalRegister(modifiers, key, hotkeyCallback);
            }

            log.Debug(keystr + " successfully registered");
        }

        public void Register(ModifiedKey key, HotkeyCallback hotkeyCallback)
        {
            Register(key.Modifiers, key.Key, hotkeyCallback);
        }

        public void Register(Template template)
        {
            template.ThrowIfNull("template");
            template.Key.ThrowIfNull("template.Key", String.Format("The template {0} must have a key in order to be auto-invoked.", template.Name));

            Register(template.Key, AutoTemplateCallback);
            AutoTemplates.Add(template);
        }

        void AutoTemplateCallback(ModifiedKey key)
        {
            var template = AutoTemplates.Single(t => t.Key == key);
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

        public void Unregister(ModifiedKey key)
        {
            Unregister(key.Modifiers, key.Key);
        }

        public void Unregister(Template template)
        {
            template.ThrowIfNull("template");
            var ourTemplate = AutoTemplates.Single(t => t.Key == template.Key);
            if (ourTemplate == null)
                return;
            AutoTemplates.Remove(ourTemplate);
            Unregister(ourTemplate.Key);
        }

        /// <summary>
        /// Unregisters all hotkeys that were registered by this application. It is not necessary
        /// to call this in normal circumstances, it will be called automatically when your
        /// application terminates.
        /// </summary>
        void UnregisterAllHotkeys()
        {
            log.Debug("Enter UnregisterAllHotkeys");

            foreach (var hk in HotKeys)
            {
                hk.Dispose();
            }

            log.Debug("Num Keys unregistered = " + HotKeys.Count); 
            
            HotKeys = new List<GlobalHotkey>();
        }

        /// <summary>
        /// This event happens early enough for us to dispose the hot keys successfully
        /// (before our AppDomain/Windows message loop goes to la-la-land.)
        /// </summary>
        void MessageLoopForm_HandleDestroyed(object sender, EventArgs e)
        {
            log.Debug("Got MessageLoopForm_HandleDestroyed event.");
            UnregisterAllHotkeys();
        }

        ~GlobalHotkeyManager()
        {
            log.Debug("Finalizing the GlobalHotkeyManager class.");
            UnregisterAllHotkeys();
        }

        /// <summary>
        /// When a hot key is pressed, find the relevant entry and invoke
        /// its callback so the client can do something.
        /// </summary>
        void TheForm_HotkeyPressed(object sender, HotkeyPressedEventArgs e)
        {
            log.Debug("TheForm_HotkeyPressed, Hot key = " + e.HotkeyInfo.ToString());
            var hk = FindHotKeyAndThrowIfNotFound(e.HotkeyInfo.Modifiers, e.HotkeyInfo.Key);
            log.Debug("About to call the hotkey's callback");
            hk.Callback(e.HotkeyInfo);
        }

        GlobalHotkey FindHotKey(Modifiers modifiers, Keys key)
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

        GlobalHotkey FindHotKeyAndThrowIfNotFound(Modifiers modifiers, Keys key)
        {
            var hk = FindHotKey(modifiers, key);
            if (hk == null)
            {
                var e = new UnknownHotkeyException
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

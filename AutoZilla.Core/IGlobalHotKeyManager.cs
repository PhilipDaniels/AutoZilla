using AutoZilla.Core.GlobalHotkeys;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    public interface IGlobalHotKeyManager
    {
        void Register(ModifiedKey key, HotkeyCallback hotkeyCallback);
        void Register(Modifiers modifiers, Keys key, HotkeyCallback hotkeyCallback);
        void Register(Template template);
        void Unregister(ModifiedKey key);
        void Unregister(Modifiers modifiers, Keys key);
        void Unregister(Template template);
    }
}

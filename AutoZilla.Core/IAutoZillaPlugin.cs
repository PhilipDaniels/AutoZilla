using AutoZilla.Core.GlobalHotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZilla.Core
{
    /// <summary>
    /// Plugins must implement this interface. It is used to find your plugin and
    /// for AutoZilla to pass you an interface which you can use to register
    /// hot keys. REALLY IGlobalHotKeyPlugin.
    /// </summary>
    public interface IAutoZillaPlugin
    {
        /// <summary>
        /// AutoZilla will cause this method as the first thing it does. You should
        /// take the reference to the <code>IGlobalHotKeyManager</code> and store
        /// it away somewhere for later use.
        /// </summary>
        /// <param name="manager"></param>
        void InitialiseHotKeyManager(IGlobalHotKeyManager manager);
    }
}

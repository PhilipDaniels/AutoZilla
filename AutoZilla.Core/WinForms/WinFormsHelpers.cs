using MiscUtils.Framework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoZilla.Core.WinForms
{
    public static class WinFormsHelpers
    {
        /// <summary>
        /// Enumerate the non-visual components of a form, such as its tooltips
        /// and timers.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns>Sequence of components.</returns>
        public static IEnumerable<Component> GetComponents(this Form form)
        {
            form.ThrowIfNull("form");

            return from field in form.GetType().GetFields
                       (
                       BindingFlags.Instance | 
                       BindingFlags.Public | 
                       BindingFlags.NonPublic
                       )
                   where typeof(Component).IsAssignableFrom(field.FieldType)
                   let component = (Component)field.GetValue(form)
                   where component != null
                   select component;
        }

        /// <summary>
        /// Finds the ToolTip control for a control. Examines the control,
        /// finds its form, then returns the ToolTip on that form, if
        /// there is one (null can be returned). Assumes that there is
        /// only one ToolTip control on the form.
        /// </summary>
        /// <param name="control">Control to get ToolTip for. This can be a form itself.</param>
        /// <returns>A reference to the ToolTip control on the form, if there is one.</returns>
        public static ToolTip GetToolTipControl(this Control control)
        {
            control.ThrowIfNull("control");

            var form = control as Form;
            if (form == null)
            {
                form = control.FindForm();
            }

            // Form may still be null if form is being constructed.
            if (form == null)
            {
                return null;
            }

            foreach (var component in form.GetComponents())
            {
                var tt = component as ToolTip;
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }
    }
}

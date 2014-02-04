using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Core.WinForms
{
    /// <summary>
    /// Create a button of this type on your form and associate it with a 
    /// <code>TextTemplate</code> to have it automatically run the template.
    /// The template does not need to have a hot key, but if it does it
    /// is displayed on the button.
    /// </summary>
    public class AutoTemplateButton : Button
    {
        public TextTemplate TextTemplate
        {
            get
            {
                return textTemplate;
            }
            set
            {
                textTemplate = value;
                if (DesignMode)
                    return;

                var tt = this.GetToolTipControl();
                if (textTemplate == null)
                {
                    Text = "";
                    if (tt != null)
                    {
                        tt.SetToolTip(this, null);
                    }
                }
                else
                {
                    string caption = textTemplate.Name;
                    if (textTemplate.ModifiedKey != null)
                    {
                        caption += " " + textTemplate.ModifiedKey.ToString();
                    }

                    Text = caption;
                    if (tt != null)
                    {
                        tt.SetToolTip(this, textTemplate.Description);
                    }
                }
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        TextTemplate textTemplate;
    }
}

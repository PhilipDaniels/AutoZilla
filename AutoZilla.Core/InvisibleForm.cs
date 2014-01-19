using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoZilla.Core
{
    /// <summary>
    /// This form is invisible. It is useful as a base class for
    /// forms in applications where you do not want a permanent GUI.
    /// </summary>
    public partial class InvisibleForm : Form
    {
        public InvisibleForm()
        {
            InitializeComponent();
            this.Load += InvisibleForm_Load;
        }

        /// <summary>
        /// Make this form invisible.
        /// </summary>
        void InvisibleForm_Load(object sender, EventArgs e)
        {
            Size = new Size(0, 0);
        }
    }
}

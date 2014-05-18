using AutoZilla.Core;
using AutoZilla.Core.Templates;
using AutoZilla.Core.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Plugins.MSSQL
{
    public partial class MainForm : FocusRestoringForm
    {
        public List<TextTemplate> AutoTemplates { get; set; }

        public MainForm()
        {
            InitializeComponent();
            AutoTemplates = new List<TextTemplate>();
            Load += MainForm_Load;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            if (AutoTemplates != null && AutoTemplates.Count > 0)
            {
                autoTemplateButton1.TextTemplate = AutoTemplates[0];
            }
        }
    }
}

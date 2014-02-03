using AutoZilla.Core.Templates;
using AutoZilla.Properties;
using System.Windows.Forms;

namespace AutoZilla
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Text = Resources.AppName;
            txtPluginPath.Text = AutoZillaVariables.PluginsFolder;
        }
    }
}

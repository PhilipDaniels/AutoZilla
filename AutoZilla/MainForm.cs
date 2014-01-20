using AutoZilla.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla
{
    public partial class MainForm : Form
    {
        PluginManager PluginManager = new PluginManager();

        public MainForm()
        {
            InitializeComponent();
            Text = Resources.AppName;
            txtPluginPath.Text = PluginManager.PluginPath;
            PluginManager.LoadAllPlugins();
        }
    }
}

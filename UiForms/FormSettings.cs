using ProfileManagerBL.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spear
{
    public partial class FormSettings : Form
    {
        private const string DEFAULT_STEAM = "";
        private const string DEFAULT_DOCS = "";
        private const string DEFAULT_APPDATA = "";
        private const string DEFAULT_NMMINFO = "";
        private const string DEFAULT_NMMMOD = "";
        private bool useDefaultSettings = false;

        public bool saveSettings { get; private set; }
        public SettingsViewData settings { get; private set; }


        public FormSettings()
        {
            this.saveSettings = false;
            InitializeComponent();
        }

        private void buttonSteam_Click(object sender, EventArgs e)
        {

        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {

        }

        private void buttonAppData_Click(object sender, EventArgs e)
        {

        }

        private void buttonNmmInfo_Click(object sender, EventArgs e)
        {

        }

        private void buttonNmmMod_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.saveSettings = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

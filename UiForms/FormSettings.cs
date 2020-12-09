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
        private readonly string DEFAULT_STEAM;
        private readonly string DEFAULT_DOCS;
        private readonly string DEFAULT_APPDATA = @"C:\Users\anderson_paschoalon\AppData\Local";
        private readonly string DEFAULT_NMMINFO = "";
        private readonly string DEFAULT_NMMMOD = "";
        private FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        private SettingsViewData settings { get; set; }
        public bool saveSettings { get; private set; }

        public FormSettings()
        {
            // steam defalt path
            this.DEFAULT_STEAM = @"C:\Program Files(x86)\Steam\steamapps\common";
            // my documents
            this.DEFAULT_DOCS = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            // %localappdata%
            this.DEFAULT_APPDATA = Environment.GetEnvironmentVariable("localappdata");
            // default: blank
            this.DEFAULT_NMMINFO = "";
            this.DEFAULT_NMMMOD = "";
            // settings object
            this.settings = new SettingsViewData();
            // tools
            this.folderDlg.ShowNewFolderButton = true;
            this.saveSettings = false;
            InitializeComponent();
        }

        public SettingsViewData getSettings()
        {
            return this.settings;
        }

        private void buttonSteam_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxSteam.Text = this.folderDlg.SelectedPath;
                this.settings.steam = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxDocs.Text = this.folderDlg.SelectedPath;
                this.settings.docs = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonAppData_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxAppData.Text = this.folderDlg.SelectedPath;
                this.settings.appData = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonNmmInfo_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxNmmInfo.Text = this.folderDlg.SelectedPath;
                this.settings.nmmInfo = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonNmmMod_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxNmmMod.Text = this.folderDlg.SelectedPath;
                this.settings.nmmMod = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.saveSettings = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.saveSettings = false;
            this.Close();
        }

        private void buttonDefSteam_Click(object sender, EventArgs e)
        {
            this.settings.steam = DEFAULT_STEAM;
            this.textBoxSteam.Text = DEFAULT_STEAM;
        }

        private void buttonDefDocs_Click(object sender, EventArgs e)
        {
            this.settings.docs = DEFAULT_DOCS;
            this.textBoxDocs.Text = DEFAULT_DOCS;
        }

        private void buttonDefApp_Click(object sender, EventArgs e)
        {
            this.settings.appData = DEFAULT_APPDATA;
            this.textBoxAppData.Text = DEFAULT_APPDATA;
        }
    }
}

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
        private FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        private SettingsViewData settings { get; set; }
        private readonly SettingsViewData defaultSettings;
        public bool saveSettings { get; private set; }

        public FormSettings(SettingsViewData oldSets, string gameName)
        {
            // default values
            this.defaultSettings = new SettingsViewData {
                appData = Environment.GetEnvironmentVariable("localappdata"),
                docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                steam = @"C:\Program Files (x86)\Steam\steamapps\common",
                nmmMod = "",
                nmmInfo = ""
            };
            // settings object
            this.settings = new SettingsViewData();
            // tools
            this.folderDlg.ShowNewFolderButton = true;
            this.saveSettings = false;
            InitializeComponent();
            // original settings
            this.textBoxSteam.Text = oldSets.steam;
            this.textBoxDocs.Text = oldSets.docs;
            this.textBoxAppData.Text = oldSets.appData;
            this.textBoxNmmInfo.Text = oldSets.nmmInfo;
            this.textBoxNmmMod.Text = oldSets.nmmMod;
            this.Text = this.Text + " [" + gameName + "]";
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
            this.settings.steam = defaultSettings.steam;
            this.textBoxSteam.Text = defaultSettings.steam;
        }

        private void buttonDefDocs_Click(object sender, EventArgs e)
        {
            this.settings.docs = defaultSettings.docs;
            this.textBoxDocs.Text = defaultSettings.docs;
        }

        private void buttonDefApp_Click(object sender, EventArgs e)
        {
            this.settings.appData = defaultSettings.appData;
            this.textBoxAppData.Text = defaultSettings.appData;
        }
    }
}

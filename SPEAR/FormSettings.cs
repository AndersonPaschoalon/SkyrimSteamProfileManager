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

        private void openDir(string path)
        {
            if (!Utils.CSharp.openDirectoryOnFileExplorer(path))
            {
                MessageBox.Show("Could not open directory \"" + path + "\" on file explorer",
                                "CANNOT OPEN DIRECTORY",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void updateSettings()
        {
            this.settings.steam = this.textBoxSteam.Text;
            this.settings.docs = this.textBoxDocs.Text;
            this.settings.appData = this.textBoxAppData.Text;
            this.settings.nmm = this.textBoxNmm.Text;
        }


        public FormSettings(SettingsViewData oldSets, string gameName)
        {
            // default values
            this.defaultSettings = new SettingsViewData {
                appData = Environment.GetEnvironmentVariable("localappdata"),
                docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\",
                steam = @"C:\Program Files (x86)\Steam\steamapps\common",
                nmm = "",
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
            this.textBoxNmm.Text = oldSets.nmm;
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
                this.textBoxSteam.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxDocs.Text = this.folderDlg.SelectedPath;
                this.textBoxDocs.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonAppData_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxAppData.Text = this.folderDlg.SelectedPath;
                this.textBoxAppData.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonNmmInfo_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxNmm.Text = this.folderDlg.SelectedPath;
                this.textBoxNmm.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        //private void buttonNmmMod_Click(object sender, EventArgs e)
        //{
        //    DialogResult result = this.folderDlg.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        this.textBoxNmmMod.Text = this.folderDlg.SelectedPath;
        //        this.textBoxNmmMod.Text = folderDlg.SelectedPath;
        //        Environment.SpecialFolder root = folderDlg.RootFolder;
        //    }
        //}

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.saveSettings = true;
            this.updateSettings();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.saveSettings = false;
            this.Close();
        }

        private void buttonDefSteam_Click(object sender, EventArgs e)
        {
            this.textBoxSteam.Text = defaultSettings.steam;
        }

        private void buttonDefDocs_Click(object sender, EventArgs e)
        {
            this.textBoxDocs.Text = defaultSettings.docs;
        }

        private void buttonDefApp_Click(object sender, EventArgs e)
        {
            this.textBoxAppData.Text = defaultSettings.appData;
        }

        private void buttonDefNmmInfo_Click(object sender, EventArgs e)
        {
            this.textBoxNmm.Text = defaultSettings.nmm;
        }

        //private void buttonDefNmmMod_Click(object sender, EventArgs e)
        //{
        //    this.textBoxNmmMod.Text = defaultSettings.nmmMod;
        //}

        private void pictureBoxSteam_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxSteam.Text);
        }

        private void pictureBoxDocs_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxDocs.Text);
        }

        private void pictureBoxAppData_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxAppData.Text);
        }

        private void pictureBoxNmmInfo_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxNmm.Text);
        }

        //private void pictureBoxNmmMod_Click(object sender, EventArgs e)
        //{
        //    this.openDir(this.textBoxNmm.Text);
        //}
    }
}

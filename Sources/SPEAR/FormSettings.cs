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
            this.settings.nmmPath = this.textBoxNmm.Text;
            this.settings.nmmGameFolder = this.textBoxNmmGameFolder.Text;

            this.settings.vortexPath = this.textBoxVortex.Text;
            this.settings.vortexGameFolder = this.textBoxVortexGameFolder.Text;

            this.settings.nmmExe = this.textBoxNmmExe.Text;
            this.settings.vortexExe = this.textBoxVortexExe.Text;
            //this.settings.tesveditExe this.textBoxTESVEdit.Text;

            //this.settings.vortexExe  = this.textBoxVortex;
            //this.settings.tesvedit = this.textBoxTESVEdit.Text;
            //this.settings.nmm = this.textBoxNmm.Text; 
        }


        public FormSettings(SettingsViewData oldSets, string gameName)
        {
            // settings object
            this.settings = new SettingsViewData();
            // tools
            this.folderDlg.ShowNewFolderButton = true;
            this.saveSettings = false;
            InitializeComponent();
            // original settings
            // paths
            this.textBoxNmm.Text = oldSets.nmmPath;
            this.textBoxVortex.Text = oldSets.vortexPath;
            this.textBoxNmmGameFolder.Text = oldSets.vortexGameFolder;
            this.textBoxVortexGameFolder.Text = oldSets.vortexGameFolder;
            // exe
            this.textBoxNmmExe.Text = oldSets.nmmExe;
            this.textBoxVortexExe.Text = oldSets.vortexExe;
            this.textBoxTESVEdit.Text = oldSets.tesveditExe;
            // title bar
            this.Text = this.Text + " [" + gameName + "]";
        }

        public SettingsViewData getSettings()
        {
            return this.settings;
        }

        #region save/cancel

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

        #endregion save/cancel

        private void buttonNmm_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxNmm.Text = this.folderDlg.SelectedPath;
                this.textBoxNmm.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonVortex_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxVortex.Text = this.folderDlg.SelectedPath;
                this.textBoxVortex.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void buttonTESVEdit_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxTESVEdit.Text = this.folderDlg.SelectedPath;
                this.textBoxTESVEdit.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void pictureBoxNmm_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxNmm.Text);
        }

        private void pictureBoxVortex_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxVortex.Text);
        }

        private void pictureBoxAppTESVEdit_Click(object sender, EventArgs e)
        {
            this.openDir(this.textBoxTESVEdit.Text);
        }

        private void textBoxVortexGameFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxTESVEdit_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

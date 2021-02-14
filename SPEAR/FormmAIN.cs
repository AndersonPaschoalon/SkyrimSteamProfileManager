using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Utils;
using Utils.Loggers;
using ProfileManagerBL;
using ProfileManagerBL.ViewModel;
using SpearSettings;

using ToolsManager;

namespace Spear
{
    public partial class FormMain : Form
    {
        // log
        private readonly ILogger log;
        private readonly string[] availableGames;
        // Profile Manager
        private ProfileManagerBusinessLayer managerBusinessLayer;
        // Data Grid View
        private List<ProfileViewData> lpa;
        private List<ProfileViewData> lpd;
        private DataGridView dataGridViewActive = new DataGridView();
        private BindingSource bindingSourceActive = new BindingSource();
        private DataGridView dataGridViewDesactivated = new DataGridView();
        private BindingSource bindingSourceDesactivated = new BindingSource();
        // Tests
        //private const bool ENABLE_TESTING = true;
        private const bool ENABLE_TESTING = false;
        private int testToRun = 0;

        public FormMain()
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_UI);
            log.Debug("###############################################################################");
            log.Debug("# INITIALIZE FormMain");
            log.Debug("###############################################################################");
            this.availableGames = ProfileManagerBusinessLayer.availableGames();
            if (availableGames.Length == 0)
            {
                this.availableGames = new string[]{ "*NO GAME DEFINED*"};
            }
            string startGame = this.availableGames[0];
            this.managerBusinessLayer = new ProfileManagerBusinessLayer(startGame);
            InitializeComponent();
            // update game list
            this.updateDropdownGameMenu();
            // enable testing panel
            this.panelTests.Visible = ENABLE_TESTING;
            // select first item as default
            log.Debug("-- select first item as default");
            this.toolStripComboBoxSelectGame.SelectedIndex = 0;
        }

        #region window_events 

        // update settings after saving data
        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            SettingsViewData oldSvd = this.managerBusinessLayer.action_getSettings();
            FormSettings settings = new FormSettings(oldSvd, this.managerBusinessLayer.gameName());
            settings.ShowDialog();
            if (settings.saveSettings)
            {
                int ret = this.managerBusinessLayer.action_updateSettings(settings.getSettings(), out errMsg);
                if(ret != Errors.SUCCESS)
                {
                    MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.fillDataGrids();
        }

        private void dataGridViewActive_CellMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            this.datagridRadioBtnColClickHandler(e, ref this.dataGridViewActive, 0);
            this.updateToolStripButtons();
        }

        private void dataGridViewDesactivated_CellMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            this.datagridRadioBtnColClickHandler(e, ref this.dataGridViewDesactivated, 0);
            this.updateToolStripButtons();
        }

        #endregion window_events

        #region events_helpers

        private static void ansMessageBox(bool ret, string successMsg, string errMsg)
        {
            if (ret)
            {
                MessageBox.Show(successMsg, "SUCCESS", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show(successMsg, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fillDataGrids()
        {
            log.Debug("-- fillDataGrids");
            if (ENABLE_TESTING)
            {
                this.lpd = this.managerBusinessLayer.test_getDesactivated(0);
                this.lpa = this.managerBusinessLayer.test_getActive(0);
            }
            else
            {
                this.managerBusinessLayer.reloadProfiles();
                this.lpd = this.managerBusinessLayer.getDesactivatedProfiles();
                this.lpa = this.managerBusinessLayer.getActiveProfiles();
                log.Debug("fillDataGrids >> lpd:" + ProfileManagerBusinessLayer.listProfileViewToString(this.lpd));
                log.Debug("fillDataGrids >> lpa:" + ProfileManagerBusinessLayer.listProfileViewToString(this.lpa));
            }
            // update allowed actions
            this.bindingSourceActive.DataSource = this.lpa;
            this.bindingSourceDesactivated.DataSource = this.lpd;
            this.loadDatagrid(ref this.dataGridViewDesactivated, ref this.bindingSourceDesactivated,
                              ref this.panelDesactivatedGrid, ref lpd, this.dataGridViewDesactivated_CellMouseClick);
            this.loadDatagrid(ref this.dataGridViewActive, ref this.bindingSourceActive,
                              ref this.panelActivatedGrid, ref lpa, this.dataGridViewActive_CellMouseClick);
            this.updateToolStripButtons();


        }

        private void updateDropdownGameMenu()
        {
            this.toolStripComboBoxSelectGame.Items.Clear();
            this.toolStripComboBoxSelectGame.Items.AddRange(this.availableGames);
        }

        /// <summary>
        /// This routine must be run after any click on the datagridview or after 
        /// any change on the application state
        /// </summary>
        private void updateToolStripButtons()
        {
            log.Debug("-- updateToolStripButtons");
            Thread.Sleep(100);
            EnabledOp enabled;
            if (ENABLE_TESTING)
            {
                enabled = managerBusinessLayer.test_getAllowed(this.testToRun);
            }
            else
            {
                enabled = managerBusinessLayer.allowedOperations();
            }
               
            // enable/disable allowed operations
            log.Debug("allowed operations >> Activate:" + enabled.activateProfile +
                      ", Desactivate:" + enabled.desactivateProfile +
                      ", Switch:" + enabled.switchProfile +
                      ", Edit:" + enabled.editProfile +
                      ", createGitignore:" + enabled.createGitignore +
                      ", deleteGitignore:" + enabled.deleteGitignore);

            // Disable buttons
            // - Profile Management
            this.toolStripButtonActivate.Enabled = enabled.activateProfile;
            this.toolStripButtonDesactivate.Enabled = enabled.desactivateProfile;
            this.toolStripButtonSwitch.Enabled = enabled.switchProfile;
            this.toolStripButtonEdit.Enabled = enabled.editProfile;
            this.toolStripButtonReload.Enabled = true;
            // - tools buttons
            this.toolStripButtonOpenGameFolder.Enabled = enabled.openGameFolder;
            this.toolStripButtonPlayGame.Enabled = enabled.launchGame;
            this.toolStripButtonGitignore.Enabled = enabled.createGitignore;
            this.toolStripButtonOpenGitignore.Enabled = enabled.openGititnore;
            this.toolStripButtonGitThrash.Enabled = enabled.deleteGitignore;

            // Disable Menu
            // - Profile management
            this.ACTIVATEProfileToolStripMenuItem.Enabled = enabled.activateProfile;
            this.DESACTIVATEProfileToolStripMenuItem.Enabled = enabled.desactivateProfile;
            this.SWITCHProfilesToolStripMenuItem.Enabled = enabled.switchProfile;
            this.EDITProfileToolStripMenuItem.Enabled = enabled.editProfile;
            this.RELOADProfilesToolStripMenuItem.Enabled = true;
            // - git tools
            this.createGitignoreFileToolStripMenuItem.Enabled = enabled.createGitignore;
            this.toolStripMenuItemOpenGitignoreFile.Enabled = enabled.openGititnore;
            this.deleteGitignoreFileToolStripMenuItem.Enabled = enabled.deleteGitignore;
            // - tools
            this.killSteamAppToolStripMenuItem.Enabled = enabled.killSteamApp;
            this.toolStripMenuItemOpenGameFolder.Enabled = enabled.openGameFolder;
            // - launch
            this.toolStripMenuItemLaunchGame.Enabled = enabled.launchGame;
            this.toolStripMenuItemVortex.Enabled = enabled.launchVortex;
            this.launchNMMToolStripMenuItem.Enabled = enabled.launchNMM;
            // - skyrim tools
            this.skyrimOpenLogsToolStripMenuItem.Enabled = enabled.skyrimOpenLogs;
            this.skyrimCleanLogsToolStripMenuItem.Enabled = enabled.skyrimCleanLogs;
            this.skyrimLaunchCreationKitToolStripMenuItem.Enabled = enabled.skyrimLaunchCreationKit;
            this.skyrimLaunchTESVEditToolStripMenuItem.Enabled = enabled.skyrimLaunchTESVEdit;

        }

        private void actionBeforeStartDisableAllActions()
        {
            // Disable buttons
            // - Profile Management
            this.toolStripButtonActivate.Enabled = false;
            this.toolStripButtonDesactivate.Enabled = false;
            this.toolStripButtonSwitch.Enabled = false;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonReload.Enabled = false;
            // - tools buttons
            this.toolStripButtonOpenGameFolder.Enabled = false;
            this.toolStripButtonPlayGame.Enabled = false;
            this.toolStripButtonGitignore.Enabled = false;
            this.toolStripButtonOpenGitignore.Enabled = false;
            this.toolStripButtonGitThrash.Enabled = false;

            // Disable Menu
            // - 
            this.ACTIVATEProfileToolStripMenuItem.Enabled = false;
            this.DESACTIVATEProfileToolStripMenuItem.Enabled = false;
            this.SWITCHProfilesToolStripMenuItem.Enabled = false;
            this.EDITProfileToolStripMenuItem.Enabled = false;
            this.RELOADProfilesToolStripMenuItem.Enabled = false;
            // - git tools
            this.createGitignoreFileToolStripMenuItem.Enabled = false;
            this.toolStripMenuItemOpenGitignoreFile.Enabled = false;
            this.deleteGitignoreFileToolStripMenuItem.Enabled = false;
            // - tools
            this.killSteamAppToolStripMenuItem.Enabled = false;
            this.toolStripMenuItemOpenGameFolder.Enabled = false;
            // - launch
            this.toolStripMenuItemLaunchGame.Enabled = false;
            this.toolStripMenuItemVortex.Enabled = false;
            this.launchNMMToolStripMenuItem.Enabled = false;
            // - skyrim tools
            this.skyrimOpenLogsToolStripMenuItem.Enabled = false;
            this.skyrimCleanLogsToolStripMenuItem.Enabled = false;
            this.skyrimLaunchCreationKitToolStripMenuItem.Enabled = false;
            this.skyrimLaunchTESVEditToolStripMenuItem.Enabled = false;
        }

        private void deleteGitignore()
        {
            string errMsg = "";
            var result = MessageBox.Show("Are you sure you want to DELETE .gitignore file?",
                                         "DELETE CONFIRMATION",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                bool retval = this.managerBusinessLayer.tool_deleteGitignore(out errMsg);
                ansMessageBox(retval, ".gitignore file was successfully deleted!", "ERROR: " + errMsg);
            }
            this.updateToolStripButtons();
        }

        private void createGitignore()
        {
            string errMsg = "";
            var result = MessageBox.Show("Do you want to create a .gitignore of all files of the game root folder?",
                                         "Creating gitignore",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                bool ret = this.managerBusinessLayer.tool_createGitignore(out errMsg);
                ansMessageBox(ret, ".gitignore file was successfully created!", "ERROR: " + errMsg);
            }
            this.updateToolStripButtons();
        }

        #endregion events_helpers

        #region form_helpers

        // create a datagridview into a panel
        private void loadDatagrid(ref DataGridView dgv, ref BindingSource bs, ref Panel panel,
                                  ref List<ProfileViewData> lp, DataGridViewCellEventHandler cellMouseClick)
        {
            bs.DataSource = lp;
            // Initialize the DataGridView.
            dgv.AutoGenerateColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.ColumnHeadersVisible = false;
            dgv.AutoSize = true;
            dgv.DataSource = bs;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.RowHeadersVisible = false;
            dgv.CellContentClick += new DataGridViewCellEventHandler(cellMouseClick);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            dgv.GridColor = SystemColors.Control;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = SystemColors.Control;

            // Initialize and add a check box column.
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "isChecked";
            column.Name = "isProfileChecked";
            column.Width = 40;
            dgv.Columns.Add(column);

            // Initialize and add a text box column.
            column = new DataGridViewTextBoxColumn();
            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "name";
            column.Name = "NAME";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.ReadOnly = true;
            dgv.Columns.Add(column);

            // Initialize the DataGridView.
            dgv.AutoGenerateColumns = false;
            dgv.ScrollBars = ScrollBars.Both;
            dgv.AutoSize = true;

            panel.Controls.Add(dgv);
            this.AutoSize = true;
            dgv.Update();
            dgv.Refresh();
        }

        private void datagridRadioBtnColClickHandler(DataGridViewCellEventArgs e, ref DataGridView dgv, int col)
        {
            bool oldVal = (bool)dgv.Rows[e.RowIndex].Cells[col].Value;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].Cells[col].Value = false;
            }
            dgv.RefreshEdit();
            if (e.RowIndex >= 0)
            {
                dgv.Rows[e.RowIndex].Cells[col].Value = !oldVal;
                dgv.RefreshEdit();
            }
        }

        private void postActionUpdate()
        {
            // reload form state
            this.managerBusinessLayer.reloadProfiles();
            this.lpd = this.managerBusinessLayer.getDesactivatedProfiles();
            this.lpa = this.managerBusinessLayer.getActiveProfiles();
            this.updateToolStripButtons();
            this.bindingSourceActive.DataSource = this.lpa;
            this.bindingSourceDesactivated.DataSource = this.lpd;
            this.dataGridViewActive.Update();
            this.dataGridViewActive.Refresh();
            this.dataGridViewDesactivated.Update();
            this.dataGridViewDesactivated.Refresh();
        }

        private static ProfileViewData getSelected(List<ProfileViewData> list)
        {
            foreach (var item in list)
            {
                if (item.isChecked == true)
                {
                    return item;
                }
            }
            return null;
        }

        private static ProfileViewData getSelected(List<ProfileViewData> list1,
            List<ProfileViewData> list2)
        {
            foreach (var item in list1)
            {
                if (item.isChecked == true)
                {
                    return item;
                }
            }
            foreach (var item in list2)
            {
                if (item.isChecked == true)
                {
                    return item;
                }
            }
            return null;
        }

        private void analyzeReturn(int retval, string errMsg)
        {
            if (retval == Errors.SUCCESS)
            {
                return;
            }
            else
            {
                string msg = Errors.errMsg(retval) + ". " + errMsg;
                log.Debug("ERROR " + retval + " => " + msg + ", errMsg:" + errMsg);
                MessageBox.Show(msg, "ERROR " + retval, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion form_helpers

        #region buttons_events 

        private void openWithNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.managerBusinessLayer.tool_openLogFiles();
        }

        private void exportAszipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            // TODO export to desktop
            // string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (!this.managerBusinessLayer.tool_exportLogs(".\\", out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonActivate_Click(object sender, EventArgs e)
        {
            this.actionBeforeStartDisableAllActions();
            // * EXECUTE ACTION
            int ret = 0;
            string errMsg = "";
            ProfileViewData pvd = getSelected(this.lpa, this.lpd);
            if (pvd != null)
            {
                if (pvd.state == ProfileType.INACTIVE)
                {
                    FormProfileEditor editor = new FormProfileEditor();
                    editor.ShowDialog();
                    if (!editor.wasCancelled())
                    {
                        ProfileViewData newProf = new ProfileViewData();
                        
                        newProf.name = editor.getName();
                        newProf.color = editor.getColor();
                        string creatinDate = "";
                        try
                        {
                            creatinDate = DateTime.Now.ToString(this.managerBusinessLayer.dateFormat());
                        }
                        catch (Exception ex)
                        {
                            log.Warn("EXCEPTION: Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                            log.Warn("Error on using dataFormat! dateFormat:<" + 
                                     this.managerBusinessLayer.dateFormat() + ">");
                            creatinDate = DateTime.Now.ToString("yyyy/MM/dd");
                        }
                        newProf.creatingDate = creatinDate;
                        ret = this.managerBusinessLayer.action_activateInactive(newProf);
                        this.analyzeReturn(ret, "");
                    }
                    else
                    {
                        log.Debug("** EDIT ACTION WAS CANCELLED");
                    }
                }
                else // ProfileType.DESACTIVATED
                {
                    ret = this.managerBusinessLayer.action_activateDesactivated(pvd, out errMsg);
                    this.analyzeReturn(ret, errMsg);
                }
            }
            // * POST ACTION FORM UPDATE
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripButtonDesactivate_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            this.actionBeforeStartDisableAllActions();
            ProfileViewData pvd = getSelected(this.lpa);
            int ret = this.managerBusinessLayer.action_desactivateProfile(pvd, out errMsg);
            this.analyzeReturn(ret, errMsg);
            // * POST ACTION FORM UPDATE
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripButtonSwitch_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            int ret = Errors.SUCCESS;
            this.actionBeforeStartDisableAllActions();
            ProfileViewData pvdAc = getSelected(this.lpa);
            ProfileViewData pvdDe = getSelected(this.lpd);
            ret = this.managerBusinessLayer.action_switchProfiles(pvdAc, pvdDe, out errMsg);
            this.analyzeReturn(ret, errMsg);
            // * POST ACTION FORM UPDATE
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            int ret = Errors.SUCCESS;
            log.Debug("-- toolStripButtonEdit_Click()");
            this.actionBeforeStartDisableAllActions();
            log.Debug("lpa: " + ProfileManagerBusinessLayer.listProfileViewToString(this.lpa));
            log.Debug("lpd: " + ProfileManagerBusinessLayer.listProfileViewToString(this.lpd));
            ProfileViewData pvd = getSelected(this.lpa, this.lpd);
            ProfileViewData newProf = new ProfileViewData();
            FormProfileEditor editor = new FormProfileEditor();
            editor.ShowDialog();
            if (!editor.wasCancelled())
            {
                newProf.name = editor.getName();
                newProf.color = editor.getColor();
                if (pvd != null)
                {
                    string errMsg = "";
                    ret = this.managerBusinessLayer.action_updateProfile(newProf, pvd, out errMsg);
                    this.analyzeReturn(ret, errMsg);
                }
                // * POST ACTION FORM UPDATE
                this.postActionUpdate();
                this.updateToolStripButtons();
            }
            else
            {
                log.Debug("** Edit process was CANCELLED!");
            }
        }

        private void openHeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.managerBusinessLayer.tool_openHelpPage();
        }

        private void toolStripComboBoxSelectGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("-- toolStripComboBoxSelectGame_SelectedIndexChanged");
            log.Info("SELECTED GAME: " + this.toolStripComboBoxSelectGame.Text);
            string selected = this.toolStripComboBoxSelectGame.Text;
            this.managerBusinessLayer = new ProfileManagerBusinessLayer(selected);
        }

        #endregion buttons_events

        #region tests

        private void buttonRunTest_Click(object sender, EventArgs e)
        {
            managerBusinessLayer.test_updateBl(this.testToRun, ref this.lpa, ref this.lpd);
            this.updateToolStripButtons();
            this.bindingSourceActive.DataSource = this.lpa;
            this.bindingSourceDesactivated.DataSource = this.lpd;
        }

        // Test button
        private void buttonSelectTest_Click(object sender, EventArgs e)
        {
            this.testToRun++;
            if (this.testToRun > 6)
            {
                this.testToRun = 0;
            }
            this.textBoxSelectedTest.Text = "Test <" + this.testToRun + "> ";
        }

        #endregion tests

        private void toolStripButtonReload_Click(object sender, EventArgs e)
        {
            this.actionBeforeStartDisableAllActions();
            Thread.Sleep(300);
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripComboBoxSelectGame_Click(object sender, EventArgs e)
        {
            MessageBox.Show("toolStripComboBoxSelectGame_Click", "TODO2", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonGitThrash_Click(object sender, EventArgs e)
        {
            log.Debug("-- toolStripButtonGitThrash_Click");
            this.deleteGitignore();
        }

        private void toolStripButtonGitignore_Click(object sender, EventArgs e)
        {
            log.Debug("-- toolStripButtonGitignore_Click");
            this.createGitignore();
        }

        private void launchNMMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("-- launchNMMToolStripMenuItem_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchNmm(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonOpenGameFolder_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonPlayGame_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchGame(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButtonOpenGitignore_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_openGititnore(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void createGitignoreFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- createGitignoreFileToolStripMenuItem_Click");
            this.createGitignore();
        }

        private void deleteGitignoreFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- deleteGitignoreFileToolStripMenuItem_Click");
            this.deleteGitignore();
        }

        private void killSteamAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- killSteamAppToolStripMenuItem_Click");
            SpearToolsManager.killAllSteam();
        }

        private void skyrimOpenLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- skyrimOpenLogsToolStripMenuItem_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_openGameLogs(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skyrimCleanLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- skyrimCleanLogsToolStripMenuItem_Click");
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void skyrimLaunchTESVEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- skyrimLaunchTESVEditToolStripMenuItem_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchTesvEdit(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemLaunchGame_Click(object sender, EventArgs e)
        {
            log.Debug(" -- toolStripMenuItemLaunchGame_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchGame(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemVortex_Click(object sender, EventArgs e)
        {
            log.Debug(" -- toolStripMenuItemVortex_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchVortex(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItemOpenGitignoreFile_Click(object sender, EventArgs e)
        {
            log.Debug(" -- toolStripMenuItemOpenGitignoreFile_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_openGititnore(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void creationKitSkyrimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug(" -- skyrimLaunchCreationKitToolStripMenuItem_Click");
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_launchCreationKit(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void creationKitSkyrimSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void openGameAppDataFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void openGameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_openGameFolder(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openGameDocumentsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string errMsg = "";
            if (!this.managerBusinessLayer.tool_openGameDocuments(out errMsg))
            {
                MessageBox.Show(errMsg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void openNMMGameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void openVortexGameFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO", "TODO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

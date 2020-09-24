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
using Utils;
using Utils.Loggers;
using ProfileManagerBL;
using ProfileManagerBL.ViewModel;

namespace Spear
{
    public partial class FormMain : Form
    {
        // log
        private readonly ILogger log;
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
            //log = ConsoleLogger.getInstance();
            log.Debug("###############################################################################");
            log.Debug("# INITIALIZE FormMain");
            log.Debug("###############################################################################");
            InitializeComponent();
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
            FormSettings settings = new FormSettings();
            settings.ShowDialog();
            if (settings.saveSettings)
            {
                this.managerBusinessLayer.action_updateSettings(settings.settings);
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
                //EnabledOp enabled = managerBusinessLayer.allowedOperations;
               
            // enable/disable allowed operations
            log.Debug("allowed operations >> Activate:" + enabled.activateProfile +
                      ", Desactivate:" + enabled.desactivateProfile +
                      ", Switch:" + enabled.switchProfile +
                      ", Edit:" + enabled.editProfile);
            this.toolStripButtonActivate.Enabled = enabled.activateProfile;
            this.toolStripButtonDesactivate.Enabled = enabled.desactivateProfile;
            this.toolStripButtonSwitch.Enabled = enabled.switchProfile;
            this.toolStripButtonEdit.Enabled = enabled.editProfile;
            this.toolStripButtonReload.Enabled = true;
        }


        private void actionBeforeStartDisableAllActions()
        {
            this.toolStripButtonActivate.Enabled = false;
            this.toolStripButtonDesactivate.Enabled = false;
            this.toolStripButtonSwitch.Enabled = false;
            this.toolStripButtonEdit.Enabled = false;
            this.toolStripButtonReload.Enabled = false;
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
            this.managerBusinessLayer.openLogFiles();
        }

        private void exportAszipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: In Development...");
        }

        private void toolStripButtonActivate_Click(object sender, EventArgs e)
        {
            this.actionBeforeStartDisableAllActions();
            // * EXECUTE ACTION
            int ret = 0;
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
                    ret = this.managerBusinessLayer.action_activateDesactivated(pvd);
                    this.analyzeReturn(ret, "");
                }
            }
            // * POST ACTION FORM UPDATE
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripButtonDesactivate_Click(object sender, EventArgs e)
        {
            this.actionBeforeStartDisableAllActions();
            ProfileViewData pvd = getSelected(this.lpa);
            int ret = this.managerBusinessLayer.action_desactivateProfile(pvd);
            this.analyzeReturn(ret, "");
            // * POST ACTION FORM UPDATE
            this.postActionUpdate();
            this.updateToolStripButtons();
        }

        private void toolStripButtonSwitch_Click(object sender, EventArgs e)
        {
            int ret = Errors.SUCCESS;
            this.actionBeforeStartDisableAllActions();
            ProfileViewData pvdAc = getSelected(this.lpa);
            ProfileViewData pvdDe = getSelected(this.lpd);
            ret = this.managerBusinessLayer.action_switchProfiles(pvdAc, pvdDe);
            this.analyzeReturn(ret, "");
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
            this.managerBusinessLayer.openHelpPage();
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
            //this.managerBusinessLayer.reloadProfiles();
            Thread.Sleep(300);
            this.postActionUpdate();
            this.updateToolStripButtons();
        }
    }
}

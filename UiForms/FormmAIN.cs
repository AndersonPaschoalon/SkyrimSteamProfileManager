using Logger;
using Logger.Loggers;
using ProfileManagerBL;
using ProfileManagerBL.ViewModel;
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

namespace UiForms
{
    public partial class FormMain : Form
    {
        // log
        private readonly ILogger log;
        private EnabledOp ops;
        // Select Game Menu
        private ViewGame game;
        // Settings Menu
        private SettingsViewData settings;
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
        private const bool enableTesting = true;
        private int testToRun = 0;

        public FormMain()
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_UI);
            //log = ConsoleLogger.getInstance();
            log.Debug("############################################################################");
            log.Debug("# INITIALIZE FormMain");
            log.Debug("############################################################################");
            InitializeComponent();
            // enable testing panel
            this.panelTests.Visible = enableTesting;
            // select first item as default
            log.Debug("-- select first item as default");
            this.toolStripComboBoxSelectGame.SelectedIndex = 0;
        }

        private void fillDataGrids()
        {
            log.Debug("-- fillDataGrids");
            this.lpd = this.managerBusinessLayer.test_getDesactivated(0);
            this.loadDatagrid(ref this.dataGridViewDesactivated, ref this.bindingSourceDesactivated,
                              ref this.panelDesactivatedGrid, ref lpd, this.dataGridViewDesactivated_CellMouseClick);
            this.lpa = this.managerBusinessLayer.test_getActive(0);
            this.loadDatagrid(ref this.dataGridViewActive, ref this.bindingSourceActive,
                              ref this.panelActivatedGrid, ref lpa, this.dataGridViewActive_CellMouseClick);
            // update allowed actions
            updateToolStripButtons();
        }

        #region events 

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

        #endregion events

        #region events_helpers

        /// <summary>
        /// This routine must be run after any click on the datagridview or after 
        /// any change on the application state
        /// </summary>
        private void updateToolStripButtons()
        {
            log.Debug("-- updateToolStripButtons");
            Thread.Sleep(100);
            //EnabledOp enabled = managerBusinessLayer.allowedOperations;
            EnabledOp enabled = managerBusinessLayer.test_getAllowed(this.testToRun);
            // enable/disable allowed operations
            log.Debug("allowed operations >> Activate:" + enabled.activateProfile +
                      ", Desactivate:" + enabled.desactivateProfile +
                      ", Switch:" + enabled.switchProfile +
                      ", Edit:" + enabled.editProfile);
            this.toolStripButtonActivate.Enabled = enabled.activateProfile;
            this.toolStripButtonDesactivate.Enabled = enabled.desactivateProfile;
            this.toolStripButtonSwitch.Enabled = enabled.switchProfile;
            this.toolStripButtonEdit.Enabled = enabled.editProfile;
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

        #endregion form_helpers

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
            MessageBox.Show("TODO: Button Activate...");
        }

        private void toolStripButtonDesactivate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Button Desactivate...");
        }

        private void toolStripButtonSwitch_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Button Switch...");
        }

        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO: Button Edit...");
        }

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
    }
}

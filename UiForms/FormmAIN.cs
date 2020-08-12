using ProfileManagerBL;
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

namespace UiForms
{
    public partial class FormMain : Form
    {
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



        // Test
        private int testToRun = 0;

        public FormMain()
        {
            InitializeComponent();
            this.managerBusinessLayer = new ProfileManagerBusinessLayer(ViewGame.SKYRIM);
        }

        private void setSkyrim()
        {
            skyrimSEToolStripMenuItem.Checked = false;
            this.game = ViewGame.SKYRIM;
            this.managerBusinessLayer = new ProfileManagerBusinessLayer(this.game);
        }

        private void setSkyrimSE()
        {
            skyrimSEToolStripMenuItem.Checked = false;
            this.game = ViewGame.SKYRIM_SE;
            this.managerBusinessLayer = new ProfileManagerBusinessLayer(this.game);
        }

        private void fillDataGrids()
        {
            // List<ProfileViewData> lpd = this.managerBusinessLayer.desactivated;
            this.lpd =  this.managerBusinessLayer.test_getDesactivated(0);
            this.loadDatagrid(ref this.dataGridViewDesactivated, ref this.bindingSourceDesactivated,
                              ref this.panelDesactivated, ref lpd, this.dataGridViewDesactivated_CellMouseClick);

            // List<ProfileViewData> lpa = this.managerBusinessLayer.active;
            this.lpa = this.managerBusinessLayer.test_getActive(0);
            this.loadDatagrid(ref this.dataGridViewActive, ref this.bindingSourceActive,
                              ref this.panelActivated, ref lpa, this.dataGridViewActive_CellMouseClick);

        }

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
            //dgv.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(findSelectedRow);

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
            dgv.AutoSize = true;

            panel.Controls.Add(dgv);
            //panel.Controls.Add(dgv);
            this.AutoSize = true;

        }

        #region events 

        // Set Skyrim as main game on the toolstrip menu
        private void skyrimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setSkyrim();
        }

        // set Skyrim SE as main game on toolstrip menu
        private void skyrimSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setSkyrimSE();
        }

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

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.managerBusinessLayer.openHelpPage();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.fillDataGrids();
        }

        private void dataGridViewActive_CellMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            var a = dataGridViewActive.Rows[0].Cells[0].Value;
            for (int i = 0; i < dataGridViewActive.Rows.Count; i++)
            {
                dataGridViewActive.Rows[i].Cells[0].Value = false;
            }
            dataGridViewActive.RefreshEdit();

            if (e.RowIndex >= 0)
            {
                dataGridViewActive.Rows[e.RowIndex].Cells[0].Value = true;
                dataGridViewActive.RefreshEdit();
            }
        }

        private void dataGridViewDesactivated_CellMouseClick(object sender, DataGridViewCellEventArgs e)
        {
            var a = dataGridViewDesactivated.Rows[0].Cells[0].Value;
            for (int i = 0; i < dataGridViewDesactivated.Rows.Count; i++)
            {
                dataGridViewDesactivated.Rows[i].Cells[0].Value = false;
            }
            dataGridViewDesactivated.RefreshEdit();

            if (e.RowIndex >= 0)
            {
                dataGridViewDesactivated.Rows[e.RowIndex].Cells[0].Value = true;
                dataGridViewDesactivated.RefreshEdit();
            }
        }

        #endregion events

        private void openWithNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In Development...");
        }

        private void exportAszipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In Development...");
        }

        private void deleteAllBackupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("In Development...");
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            managerBusinessLayer.test_updateBl(this.testToRun, ref this.lpa, ref this.lpd);

            this.bindingSourceActive.DataSource = this.lpa;
            this.bindingSourceDesactivated.DataSource = this.lpd;

            //this.bindingSourceActive.ResetBindings(false);
            //this.bindingSourceDesactivated.ResetBindings(false);
            //this.bindingSourceDesactivated.EndEdit();
            //dataGridViewActive.Update();
            //dataGridViewActive.Refresh();
            //dataGridViewDesactivated.Update();
            //dataGridViewDesactivated.Refresh();
            this.buttonTest.Text = "Test <" + this.testToRun + ">";
            this.testToRun++;
            if (this.testToRun > 6)
            {
                this.testToRun = 0;
            }
        }
    }
}

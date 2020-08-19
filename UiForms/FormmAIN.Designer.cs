namespace UiForms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWithNotepadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAszipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openHeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBoxSelectGame = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonActivate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDesactivate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSwitch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelActive = new System.Windows.Forms.Label();
            this.labelDesactivated = new System.Windows.Forms.Label();
            this.panelActivated = new System.Windows.Forms.Panel();
            this.panelActivatedGrid = new System.Windows.Forms.Panel();
            this.panelDesactivated = new System.Windows.Forms.Panel();
            this.panelDesactivatedGrid = new System.Windows.Forms.Panel();
            this.panelTests = new System.Windows.Forms.Panel();
            this.buttonRunTest = new System.Windows.Forms.Button();
            this.buttonSelectTest = new System.Windows.Forms.Button();
            this.textBoxSelectedTest = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelActivated.SuspendLayout();
            this.panelDesactivated.SuspendLayout();
            this.panelTests.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.logsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // logsToolStripMenuItem
            // 
            this.logsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openWithNotepadToolStripMenuItem,
            this.exportAszipToolStripMenuItem});
            this.logsToolStripMenuItem.Name = "logsToolStripMenuItem";
            this.logsToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.logsToolStripMenuItem.Text = "Logs";
            // 
            // openWithNotepadToolStripMenuItem
            // 
            this.openWithNotepadToolStripMenuItem.Name = "openWithNotepadToolStripMenuItem";
            this.openWithNotepadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openWithNotepadToolStripMenuItem.Text = "Open With Notepad";
            this.openWithNotepadToolStripMenuItem.Click += new System.EventHandler(this.openWithNotepadToolStripMenuItem_Click);
            // 
            // exportAszipToolStripMenuItem
            // 
            this.exportAszipToolStripMenuItem.Name = "exportAszipToolStripMenuItem";
            this.exportAszipToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportAszipToolStripMenuItem.Text = "Export as .zip";
            this.exportAszipToolStripMenuItem.Click += new System.EventHandler(this.exportAszipToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openHeToolStripMenuItem});
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // openHeToolStripMenuItem
            // 
            this.openHeToolStripMenuItem.Name = "openHeToolStripMenuItem";
            this.openHeToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.openHeToolStripMenuItem.Text = "Open Help Page";
            this.openHeToolStripMenuItem.Click += new System.EventHandler(this.openHeToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBoxSelectGame,
            this.toolStripSeparator1,
            this.toolStripButtonActivate,
            this.toolStripButtonDesactivate,
            this.toolStripButtonSwitch,
            this.toolStripButtonEdit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBoxSelectGame
            // 
            this.toolStripComboBoxSelectGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxSelectGame.Items.AddRange(new object[] {
            "Skyrim",
            "Skyrim SE",
            "Morrowind",
            "Age of Mitology"});
            this.toolStripComboBoxSelectGame.Name = "toolStripComboBoxSelectGame";
            this.toolStripComboBoxSelectGame.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBoxSelectGame.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxSelectGame_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonActivate
            // 
            this.toolStripButtonActivate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonActivate.Image = global::UiForms.Properties.Resources._15_tick;
            this.toolStripButtonActivate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonActivate.Name = "toolStripButtonActivate";
            this.toolStripButtonActivate.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonActivate.Text = "ACTIVATE Profile";
            this.toolStripButtonActivate.Click += new System.EventHandler(this.toolStripButtonActivate_Click);
            // 
            // toolStripButtonDesactivate
            // 
            this.toolStripButtonDesactivate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDesactivate.Image = global::UiForms.Properties.Resources._15_cancel;
            this.toolStripButtonDesactivate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDesactivate.Name = "toolStripButtonDesactivate";
            this.toolStripButtonDesactivate.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDesactivate.Text = "DESACTIVATE Profile";
            this.toolStripButtonDesactivate.Click += new System.EventHandler(this.toolStripButtonDesactivate_Click);
            // 
            // toolStripButtonSwitch
            // 
            this.toolStripButtonSwitch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSwitch.Image = global::UiForms.Properties.Resources._15_2arrow;
            this.toolStripButtonSwitch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSwitch.Name = "toolStripButtonSwitch";
            this.toolStripButtonSwitch.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSwitch.Text = "SWITCH Profiles";
            this.toolStripButtonSwitch.Click += new System.EventHandler(this.toolStripButtonSwitch_Click);
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEdit.Image = global::UiForms.Properties.Resources._15_edit;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEdit.Text = "EDIT Profiles";
            this.toolStripButtonEdit.Click += new System.EventHandler(this.toolStripButtonEdit_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 550F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelTests, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 401);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(115, 13);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(544, 380);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel2.Controls.Add(this.labelActive, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDesactivated, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelActivated, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panelDesactivated, 3, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(544, 380);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelActive
            // 
            this.labelActive.AutoSize = true;
            this.labelActive.Location = new System.Drawing.Point(30, 0);
            this.labelActive.Name = "labelActive";
            this.labelActive.Size = new System.Drawing.Size(37, 13);
            this.labelActive.TabIndex = 0;
            this.labelActive.Text = "Active";
            this.labelActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDesactivated
            // 
            this.labelDesactivated.AutoSize = true;
            this.labelDesactivated.Location = new System.Drawing.Point(288, 0);
            this.labelDesactivated.Name = "labelDesactivated";
            this.labelDesactivated.Size = new System.Drawing.Size(70, 13);
            this.labelDesactivated.TabIndex = 1;
            this.labelDesactivated.Text = "Desactivated";
            this.labelDesactivated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelActivated
            // 
            this.panelActivated.Controls.Add(this.panelActivatedGrid);
            this.panelActivated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActivated.Location = new System.Drawing.Point(30, 33);
            this.panelActivated.Name = "panelActivated";
            this.panelActivated.Size = new System.Drawing.Size(225, 334);
            this.panelActivated.TabIndex = 2;
            // 
            // panelActivatedGrid
            // 
            this.panelActivatedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelActivatedGrid.Location = new System.Drawing.Point(0, 0);
            this.panelActivatedGrid.Name = "panelActivatedGrid";
            this.panelActivatedGrid.Size = new System.Drawing.Size(225, 334);
            this.panelActivatedGrid.TabIndex = 0;
            // 
            // panelDesactivated
            // 
            this.panelDesactivated.Controls.Add(this.panelDesactivatedGrid);
            this.panelDesactivated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesactivated.Location = new System.Drawing.Point(288, 33);
            this.panelDesactivated.Name = "panelDesactivated";
            this.panelDesactivated.Size = new System.Drawing.Size(225, 334);
            this.panelDesactivated.TabIndex = 3;
            // 
            // panelDesactivatedGrid
            // 
            this.panelDesactivatedGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDesactivatedGrid.Location = new System.Drawing.Point(0, 0);
            this.panelDesactivatedGrid.Name = "panelDesactivatedGrid";
            this.panelDesactivatedGrid.Size = new System.Drawing.Size(225, 334);
            this.panelDesactivatedGrid.TabIndex = 0;
            // 
            // panelTests
            // 
            this.panelTests.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelTests.Controls.Add(this.buttonRunTest);
            this.panelTests.Controls.Add(this.buttonSelectTest);
            this.panelTests.Controls.Add(this.textBoxSelectedTest);
            this.panelTests.Controls.Add(this.label1);
            this.panelTests.Location = new System.Drawing.Point(3, 13);
            this.panelTests.Name = "panelTests";
            this.panelTests.Size = new System.Drawing.Size(106, 144);
            this.panelTests.TabIndex = 2;
            // 
            // buttonRunTest
            // 
            this.buttonRunTest.Location = new System.Drawing.Point(50, 80);
            this.buttonRunTest.Name = "buttonRunTest";
            this.buttonRunTest.Size = new System.Drawing.Size(53, 23);
            this.buttonRunTest.TabIndex = 3;
            this.buttonRunTest.Text = "RUN";
            this.buttonRunTest.UseVisualStyleBackColor = true;
            this.buttonRunTest.Click += new System.EventHandler(this.buttonRunTest_Click);
            // 
            // buttonSelectTest
            // 
            this.buttonSelectTest.Location = new System.Drawing.Point(3, 16);
            this.buttonSelectTest.Name = "buttonSelectTest";
            this.buttonSelectTest.Size = new System.Drawing.Size(100, 23);
            this.buttonSelectTest.TabIndex = 1;
            this.buttonSelectTest.Text = "SELECT TEST";
            this.buttonSelectTest.UseVisualStyleBackColor = true;
            this.buttonSelectTest.Click += new System.EventHandler(this.buttonSelectTest_Click);
            // 
            // textBoxSelectedTest
            // 
            this.textBoxSelectedTest.Location = new System.Drawing.Point(3, 45);
            this.textBoxSelectedTest.Name = "textBoxSelectedTest";
            this.textBoxSelectedTest.Size = new System.Drawing.Size(100, 20);
            this.textBoxSelectedTest.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "TEST PANEL";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelActivated.ResumeLayout(false);
            this.panelDesactivated.ResumeLayout(false);
            this.panelTests.ResumeLayout(false);
            this.panelTests.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn profileTypeDataGridViewCheckBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem logsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWithNotepadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAszipToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonActivate;
        private System.Windows.Forms.ToolStripButton toolStripButtonDesactivate;
        private System.Windows.Forms.ToolStripButton toolStripButtonSwitch;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelActive;
        private System.Windows.Forms.Label labelDesactivated;
        private System.Windows.Forms.Panel panelActivated;
        private System.Windows.Forms.Panel panelDesactivated;
        private System.Windows.Forms.TextBox textBoxSelectedTest;
        private System.Windows.Forms.Panel panelActivatedGrid;
        private System.Windows.Forms.Panel panelDesactivatedGrid;
        private System.Windows.Forms.Panel panelTests;
        private System.Windows.Forms.Button buttonRunTest;
        private System.Windows.Forms.Button buttonSelectTest;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openHeToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxSelectGame;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
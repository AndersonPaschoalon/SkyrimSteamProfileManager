namespace Spear
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.groupBoxSteam = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonAppData = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDocs = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSteam = new System.Windows.Forms.Button();
            this.groupBoxNMM = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonNmmMod = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonNmmInfo = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.radioButtonDefault = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.tabSettingsMain = new System.Windows.Forms.TabControl();
            this.tabConfiguration = new System.Windows.Forms.TabPage();
            this.tabLogs = new System.Windows.Forms.TabPage();
            this.buttonSaveLogSettings = new System.Windows.Forms.Button();
            this.groupLogBackup = new System.Windows.Forms.GroupBox();
            this.radioBkpNoLimit = new System.Windows.Forms.RadioButton();
            this.radioBkp10 = new System.Windows.Forms.RadioButton();
            this.radioBkp05 = new System.Windows.Forms.RadioButton();
            this.groupLoglevel = new System.Windows.Forms.GroupBox();
            this.radioError = new System.Windows.Forms.RadioButton();
            this.radioDebug = new System.Windows.Forms.RadioButton();
            this.radioWarn = new System.Windows.Forms.RadioButton();
            this.radioInfo = new System.Windows.Forms.RadioButton();
            this.groupBoxSteam.SuspendLayout();
            this.groupBoxNMM.SuspendLayout();
            this.tabSettingsMain.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.tabLogs.SuspendLayout();
            this.groupLogBackup.SuspendLayout();
            this.groupLoglevel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSteam
            // 
            this.groupBoxSteam.Controls.Add(this.textBox3);
            this.groupBoxSteam.Controls.Add(this.label3);
            this.groupBoxSteam.Controls.Add(this.buttonAppData);
            this.groupBoxSteam.Controls.Add(this.textBox2);
            this.groupBoxSteam.Controls.Add(this.label2);
            this.groupBoxSteam.Controls.Add(this.buttonDocs);
            this.groupBoxSteam.Controls.Add(this.textBox1);
            this.groupBoxSteam.Controls.Add(this.label1);
            this.groupBoxSteam.Controls.Add(this.buttonSteam);
            this.groupBoxSteam.Location = new System.Drawing.Point(6, 70);
            this.groupBoxSteam.Name = "groupBoxSteam";
            this.groupBoxSteam.Size = new System.Drawing.Size(533, 184);
            this.groupBoxSteam.TabIndex = 0;
            this.groupBoxSteam.TabStop = false;
            this.groupBoxSteam.Text = "Steam Settings";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(109, 140);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(418, 20);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(336, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "AppData\\Local folder:  (hint: type %localappdata% on the file explorer)";
            // 
            // buttonAppData
            // 
            this.buttonAppData.Location = new System.Drawing.Point(6, 138);
            this.buttonAppData.Name = "buttonAppData";
            this.buttonAppData.Size = new System.Drawing.Size(75, 23);
            this.buttonAppData.TabIndex = 6;
            this.buttonAppData.Text = "Browse...";
            this.buttonAppData.UseVisualStyleBackColor = true;
            this.buttonAppData.Click += new System.EventHandler(this.buttonAppData_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(109, 89);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(418, 20);
            this.textBox2.TabIndex = 5;
            this.textBox2.Text = " ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Select My Documents folder:";
            // 
            // buttonDocs
            // 
            this.buttonDocs.Location = new System.Drawing.Point(6, 87);
            this.buttonDocs.Name = "buttonDocs";
            this.buttonDocs.Size = new System.Drawing.Size(75, 23);
            this.buttonDocs.TabIndex = 3;
            this.buttonDocs.Text = "Browse...";
            this.buttonDocs.UseVisualStyleBackColor = true;
            this.buttonDocs.Click += new System.EventHandler(this.buttonDocs_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(109, 38);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(418, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = " ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Steam\\Commom  folder:";
            // 
            // buttonSteam
            // 
            this.buttonSteam.Location = new System.Drawing.Point(6, 36);
            this.buttonSteam.Name = "buttonSteam";
            this.buttonSteam.Size = new System.Drawing.Size(75, 23);
            this.buttonSteam.TabIndex = 0;
            this.buttonSteam.Text = "Browse...";
            this.buttonSteam.UseVisualStyleBackColor = true;
            this.buttonSteam.Click += new System.EventHandler(this.buttonSteam_Click);
            // 
            // groupBoxNMM
            // 
            this.groupBoxNMM.Controls.Add(this.textBox5);
            this.groupBoxNMM.Controls.Add(this.label5);
            this.groupBoxNMM.Controls.Add(this.buttonNmmMod);
            this.groupBoxNMM.Controls.Add(this.textBox4);
            this.groupBoxNMM.Controls.Add(this.label4);
            this.groupBoxNMM.Controls.Add(this.buttonNmmInfo);
            this.groupBoxNMM.Location = new System.Drawing.Point(6, 260);
            this.groupBoxNMM.Name = "groupBoxNMM";
            this.groupBoxNMM.Size = new System.Drawing.Size(532, 143);
            this.groupBoxNMM.TabIndex = 1;
            this.groupBoxNMM.TabStop = false;
            this.groupBoxNMM.Text = "(Optional) NMM Settings";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(108, 93);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(418, 20);
            this.textBox5.TabIndex = 8;
            this.textBox5.Text = " ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "NMM Mod Folder:";
            // 
            // buttonNmmMod
            // 
            this.buttonNmmMod.Location = new System.Drawing.Point(5, 91);
            this.buttonNmmMod.Name = "buttonNmmMod";
            this.buttonNmmMod.Size = new System.Drawing.Size(75, 23);
            this.buttonNmmMod.TabIndex = 6;
            this.buttonNmmMod.Text = "Browse...";
            this.buttonNmmMod.UseVisualStyleBackColor = true;
            this.buttonNmmMod.Click += new System.EventHandler(this.buttonNmmMod_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(108, 44);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(418, 20);
            this.textBox4.TabIndex = 5;
            this.textBox4.Text = " ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "NMM Info folder:";
            // 
            // buttonNmmInfo
            // 
            this.buttonNmmInfo.Location = new System.Drawing.Point(5, 42);
            this.buttonNmmInfo.Name = "buttonNmmInfo";
            this.buttonNmmInfo.Size = new System.Drawing.Size(75, 23);
            this.buttonNmmInfo.TabIndex = 3;
            this.buttonNmmInfo.Text = "Browse...";
            this.buttonNmmInfo.UseVisualStyleBackColor = true;
            this.buttonNmmInfo.Click += new System.EventHandler(this.buttonNmmInfo_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonSave.Location = new System.Drawing.Point(463, 421);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.Location = new System.Drawing.Point(363, 421);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // radioButtonDefault
            // 
            this.radioButtonDefault.AutoSize = true;
            this.radioButtonDefault.Location = new System.Drawing.Point(6, 7);
            this.radioButtonDefault.Name = "radioButtonDefault";
            this.radioButtonDefault.Size = new System.Drawing.Size(122, 17);
            this.radioButtonDefault.TabIndex = 11;
            this.radioButtonDefault.TabStop = true;
            this.radioButtonDefault.Text = "Use Default Settings";
            this.radioButtonDefault.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 30);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(157, 17);
            this.radioButton2.TabIndex = 12;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Edit Settings (Recomended)";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // tabSettingsMain
            // 
            this.tabSettingsMain.Controls.Add(this.tabConfiguration);
            this.tabSettingsMain.Controls.Add(this.tabLogs);
            this.tabSettingsMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSettingsMain.Location = new System.Drawing.Point(0, 0);
            this.tabSettingsMain.Name = "tabSettingsMain";
            this.tabSettingsMain.SelectedIndex = 0;
            this.tabSettingsMain.Size = new System.Drawing.Size(605, 481);
            this.tabSettingsMain.TabIndex = 13;
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.radioButton2);
            this.tabConfiguration.Controls.Add(this.groupBoxSteam);
            this.tabConfiguration.Controls.Add(this.radioButtonDefault);
            this.tabConfiguration.Controls.Add(this.groupBoxNMM);
            this.tabConfiguration.Controls.Add(this.buttonCancel);
            this.tabConfiguration.Controls.Add(this.buttonSave);
            this.tabConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfiguration.Size = new System.Drawing.Size(597, 455);
            this.tabConfiguration.TabIndex = 0;
            this.tabConfiguration.Text = "Game Configuration";
            this.tabConfiguration.UseVisualStyleBackColor = true;
            // 
            // tabLogs
            // 
            this.tabLogs.Controls.Add(this.buttonSaveLogSettings);
            this.tabLogs.Controls.Add(this.groupLogBackup);
            this.tabLogs.Controls.Add(this.groupLoglevel);
            this.tabLogs.Location = new System.Drawing.Point(4, 22);
            this.tabLogs.Name = "tabLogs";
            this.tabLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogs.Size = new System.Drawing.Size(597, 455);
            this.tabLogs.TabIndex = 1;
            this.tabLogs.Text = "Logs";
            this.tabLogs.UseVisualStyleBackColor = true;
            // 
            // buttonSaveLogSettings
            // 
            this.buttonSaveLogSettings.Location = new System.Drawing.Point(133, 237);
            this.buttonSaveLogSettings.Name = "buttonSaveLogSettings";
            this.buttonSaveLogSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveLogSettings.TabIndex = 6;
            this.buttonSaveLogSettings.Text = "SAVE";
            this.buttonSaveLogSettings.UseVisualStyleBackColor = true;
            // 
            // groupLogBackup
            // 
            this.groupLogBackup.Controls.Add(this.radioBkpNoLimit);
            this.groupLogBackup.Controls.Add(this.radioBkp10);
            this.groupLogBackup.Controls.Add(this.radioBkp05);
            this.groupLogBackup.Location = new System.Drawing.Point(8, 131);
            this.groupLogBackup.Name = "groupLogBackup";
            this.groupLogBackup.Size = new System.Drawing.Size(200, 100);
            this.groupLogBackup.TabIndex = 5;
            this.groupLogBackup.TabStop = false;
            this.groupLogBackup.Text = "Log Backup";
            // 
            // radioBkpNoLimit
            // 
            this.radioBkpNoLimit.AutoSize = true;
            this.radioBkpNoLimit.Location = new System.Drawing.Point(4, 66);
            this.radioBkpNoLimit.Name = "radioBkpNoLimit";
            this.radioBkpNoLimit.Size = new System.Drawing.Size(63, 17);
            this.radioBkpNoLimit.TabIndex = 2;
            this.radioBkpNoLimit.TabStop = true;
            this.radioBkpNoLimit.Text = "No Limit";
            this.radioBkpNoLimit.UseVisualStyleBackColor = true;
            // 
            // radioBkp10
            // 
            this.radioBkp10.AutoSize = true;
            this.radioBkp10.Location = new System.Drawing.Point(4, 43);
            this.radioBkp10.Name = "radioBkp10";
            this.radioBkp10.Size = new System.Drawing.Size(37, 17);
            this.radioBkp10.TabIndex = 1;
            this.radioBkp10.TabStop = true;
            this.radioBkp10.Text = "10";
            this.radioBkp10.UseVisualStyleBackColor = true;
            // 
            // radioBkp05
            // 
            this.radioBkp05.AutoSize = true;
            this.radioBkp05.Location = new System.Drawing.Point(4, 20);
            this.radioBkp05.Name = "radioBkp05";
            this.radioBkp05.Size = new System.Drawing.Size(37, 17);
            this.radioBkp05.TabIndex = 0;
            this.radioBkp05.TabStop = true;
            this.radioBkp05.Text = "05";
            this.radioBkp05.UseVisualStyleBackColor = true;
            // 
            // groupLoglevel
            // 
            this.groupLoglevel.Controls.Add(this.radioError);
            this.groupLoglevel.Controls.Add(this.radioDebug);
            this.groupLoglevel.Controls.Add(this.radioWarn);
            this.groupLoglevel.Controls.Add(this.radioInfo);
            this.groupLoglevel.Location = new System.Drawing.Point(6, 6);
            this.groupLoglevel.Name = "groupLoglevel";
            this.groupLoglevel.Size = new System.Drawing.Size(200, 119);
            this.groupLoglevel.TabIndex = 4;
            this.groupLoglevel.TabStop = false;
            this.groupLoglevel.Text = "Log Level";
            // 
            // radioError
            // 
            this.radioError.AutoSize = true;
            this.radioError.Location = new System.Drawing.Point(6, 19);
            this.radioError.Name = "radioError";
            this.radioError.Size = new System.Drawing.Size(47, 17);
            this.radioError.TabIndex = 0;
            this.radioError.TabStop = true;
            this.radioError.Text = "Error";
            this.radioError.UseVisualStyleBackColor = true;
            // 
            // radioDebug
            // 
            this.radioDebug.AutoSize = true;
            this.radioDebug.Location = new System.Drawing.Point(6, 91);
            this.radioDebug.Name = "radioDebug";
            this.radioDebug.Size = new System.Drawing.Size(57, 17);
            this.radioDebug.TabIndex = 3;
            this.radioDebug.TabStop = true;
            this.radioDebug.Text = "Debug";
            this.radioDebug.UseVisualStyleBackColor = true;
            // 
            // radioWarn
            // 
            this.radioWarn.AutoSize = true;
            this.radioWarn.Location = new System.Drawing.Point(6, 43);
            this.radioWarn.Name = "radioWarn";
            this.radioWarn.Size = new System.Drawing.Size(51, 17);
            this.radioWarn.TabIndex = 1;
            this.radioWarn.TabStop = true;
            this.radioWarn.Text = "Warn";
            this.radioWarn.UseVisualStyleBackColor = true;
            // 
            // radioInfo
            // 
            this.radioInfo.AutoSize = true;
            this.radioInfo.Location = new System.Drawing.Point(6, 67);
            this.radioInfo.Name = "radioInfo";
            this.radioInfo.Size = new System.Drawing.Size(43, 17);
            this.radioInfo.TabIndex = 2;
            this.radioInfo.TabStop = true;
            this.radioInfo.Text = "Info";
            this.radioInfo.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 481);
            this.Controls.Add(this.tabSettingsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.Text = "Edit Settings";
            this.groupBoxSteam.ResumeLayout(false);
            this.groupBoxSteam.PerformLayout();
            this.groupBoxNMM.ResumeLayout(false);
            this.groupBoxNMM.PerformLayout();
            this.tabSettingsMain.ResumeLayout(false);
            this.tabConfiguration.ResumeLayout(false);
            this.tabConfiguration.PerformLayout();
            this.tabLogs.ResumeLayout(false);
            this.groupLogBackup.ResumeLayout(false);
            this.groupLogBackup.PerformLayout();
            this.groupLoglevel.ResumeLayout(false);
            this.groupLoglevel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSteam;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSteam;
        private System.Windows.Forms.GroupBox groupBoxNMM;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAppData;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDocs;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonNmmMod;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonNmmInfo;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonDefault;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TabControl tabSettingsMain;
        private System.Windows.Forms.TabPage tabConfiguration;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.Button buttonSaveLogSettings;
        private System.Windows.Forms.GroupBox groupLogBackup;
        private System.Windows.Forms.RadioButton radioBkpNoLimit;
        private System.Windows.Forms.RadioButton radioBkp10;
        private System.Windows.Forms.RadioButton radioBkp05;
        private System.Windows.Forms.GroupBox groupLoglevel;
        private System.Windows.Forms.RadioButton radioError;
        private System.Windows.Forms.RadioButton radioDebug;
        private System.Windows.Forms.RadioButton radioWarn;
        private System.Windows.Forms.RadioButton radioInfo;
    }
}
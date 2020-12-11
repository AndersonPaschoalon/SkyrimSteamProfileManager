namespace Spear
{
    partial class FormProfileEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProfileEditor));
            this.label1 = new System.Windows.Forms.Label();
            this.textProfileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.buttonPickColor = new System.Windows.Forms.Button();
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profile Name";
            // 
            // textProfileName
            // 
            this.textProfileName.Location = new System.Drawing.Point(86, 10);
            this.textProfileName.Name = "textProfileName";
            this.textProfileName.Size = new System.Drawing.Size(243, 20);
            this.textProfileName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Color";
            // 
            // buttonPickColor
            // 
            this.buttonPickColor.Location = new System.Drawing.Point(254, 44);
            this.buttonPickColor.Name = "buttonPickColor";
            this.buttonPickColor.Size = new System.Drawing.Size(75, 23);
            this.buttonPickColor.TabIndex = 3;
            this.buttonPickColor.Text = "Pick Color...";
            this.buttonPickColor.UseVisualStyleBackColor = true;
            this.buttonPickColor.Click += new System.EventHandler(this.buttonPickColor_Click);
            // 
            // textBoxColor
            // 
            this.textBoxColor.Enabled = false;
            this.textBoxColor.Location = new System.Drawing.Point(86, 46);
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.Size = new System.Drawing.Size(152, 20);
            this.textBoxColor.TabIndex = 4;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(254, 90);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(163, 90);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FormProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(339, 125);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxColor);
            this.Controls.Add(this.buttonPickColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textProfileName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormProfileEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Profile Editor";
            this.Load += new System.EventHandler(this.FormProfileEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textProfileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button buttonPickColor;
        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
    }
}
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
    public partial class FormProfileEditor : Form
    {
        private bool cancel;

        public string getName()
        {
            return this.textProfileName.Text;
        }

        public Color getColor()
        {
            return this.textBoxColor.BackColor;
        }

        public bool wasCancelled()
        {
            return this.cancel;
        }

        public FormProfileEditor()
        {
            InitializeComponent();
        }

        private void buttonPickColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            textBoxColor.BackColor = colorDialog.Color;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.cancel = false; ;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cancel = true;
            this.Close();
        }

        private void FormProfileEditor_Load(object sender, EventArgs e)
        {
            this.cancel = false;
        }
    }
}

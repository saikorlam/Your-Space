using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InventorySolution
{
    public partial class WelcomeScr : Form
    {
        public WelcomeScr()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Record rcform = new Record();
            rcform.MdiParent = this;
            rcform.Show();
            rcform.StartPosition = FormStartPosition.CenterParent;
            btnNext.Enabled = false;

        }

        private void WelcomeScr_Layout(object sender, LayoutEventArgs e)
        {

        }
    }
}

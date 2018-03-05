using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAWindower
{
    public partial class Thumbnail : UserControl
    {
        public Thumbnail()
        {
            InitializeComponent();
        }

        private void windowTitle_Click(object sender, EventArgs e)
        {
            string input;

            if (InputDialog.Show(this, "Please enter a name for this window.", out input) == DialogResult.OK)
                windowTitle.Text = input;
        }
    }
}

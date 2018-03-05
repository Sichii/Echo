using System;
using System.Windows.Forms;

namespace DAWindower
{
    internal partial class InputDialog : Form
    {
        private InputDialog(string msg)
        {
            InitializeComponent();
            msgLbl.Text = msg;
        }

        internal static DialogResult Show(IWin32Window owner, string msg, out string input)
        {
            using (InputDialog dialog = new InputDialog(msg))
            {
                DialogResult result = dialog.ShowDialog(owner);
                input = dialog.saveName.Text;

                return result;
            }
        }


    }
}

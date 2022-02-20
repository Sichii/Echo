using System.Windows.Forms;

namespace Echo;

public partial class MessageDialog : Form
{
    public MessageDialog(string msg)
    {
        InitializeComponent();
        messageLbl.Text = msg;
    }

    internal static DialogResult Show(IWin32Window owner, string msg, string caption = "Message Dialog")
    {
        using var message = new MessageDialog(msg);

        message.Text = caption;

        return message.ShowDialog(owner);
    }
}
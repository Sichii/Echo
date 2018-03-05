using System;
using System.Windows.Forms;

namespace DAWindower
{
    internal partial class Thumbnail : Form
    {
        private Client client;
        private IntPtr tHandle = new IntPtr();

        internal Thumbnail()
        {
            InitializeComponent();
        }

        internal bool SetClient(Client cli)
        {
            client = cli;

            //register the thumbnail
            int x = Dwmapi.DwmRegisterThumbnail(Handle, client.Handle, out tHandle);

            //attempt to register this darkages process to a thumbnail, 0 for success
            if (x == 0)
            {
                //create a new thumbnail properties struct and set properties/location/size/etc
                ThumbnailProperties tProperties = new ThumbnailProperties();
                tProperties.Visible = true;
                tProperties.Flags = ThumbnailFlags.Visible | ThumbnailFlags.RectDestination | ThumbnailFlags.Opacity;
                tProperties.Opacity = 255;
                tProperties.OnlyClientRect = true;
                tProperties.DestinationRect = new Rect(Left, Top + 25, Width, Height);

                //update the thumbnail
                Dwmapi.DwmUpdateThumbnailProperties(tHandle, ref tProperties);
                return true;
            }
            else
                return false;
        }

        private void windowTitle_Click(object sender, EventArgs e)
        {
            string input;

            if (InputDialog.Show(this, "Please enter a name for this window.", out input) == DialogResult.OK)
                windowTitle.Text = input;
        }
    }
}

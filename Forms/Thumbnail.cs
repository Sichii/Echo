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
    internal partial class Thumbnail : UserControl
    {
        private bool Generated = false;
        private MainForm MainForm;
        private Client Client;
        private IntPtr tHandle = new IntPtr();

        internal Thumbnail(MainForm mainForm, Client client)
        {
            MainForm = mainForm;
            Client = client;
            InitializeComponent();
        }

        internal bool GenerateThumbnail()
        {
            int x = Dwmapi.DwmRegisterThumbnail(MainForm.Handle, Client.MainHandle, out tHandle);
            //attempt to register this darkages process to a thumbnail, 0 for success
            if (Dwmapi.DwmRegisterThumbnail(MainForm.Handle, Client.MainHandle, out tHandle) == 0)
            {
                //create a new thumbnail properties struct and set properties/location/size/etc
                ThumbnailProperties tProperties = new ThumbnailProperties();
                tProperties.Visible = true;
                tProperties.Flags = ThumbnailFlags.Visible | ThumbnailFlags.RectDestination | ThumbnailFlags.Opacity | ThumbnailFlags.SourceClientAreaOnly;
                tProperties.Opacity = 255;
                tProperties.OnlyClientRect = true;

                //now we determine the location of the thumbnail
                //first we convert this usercontrol's rect to a screen rect
                Rectangle screenRect = RectangleToScreen(DisplayRectangle);
                //clientrect will represent the mainform co-ordinates of this form
                Rectangle clientRect = MainForm.RectangleToClient(screenRect);

                tProperties.DestinationRect = new Rect(clientRect.Left, clientRect.Top + 24, clientRect.Left + clientRect.Width, clientRect.Top + clientRect.Height);

                //update the thumbnail
                Dwmapi.DwmUpdateThumbnailProperties(tHandle, ref tProperties);
                return true;
            }
            else
                return false;
        }

        internal void UpdateThumbnail()
        {
            Dwmapi.DwmUnregisterThumbnail(tHandle);
            GenerateThumbnail();
        }

        private void windowTitle_Click(object sender, EventArgs e)
        {
            string input;

            if (InputDialog.Show(MainForm, "Please enter a name for this window.", out input) == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(input))
                    return;

                windowTitleLbl.Text = input;
                Client.Name = input;
                User32.SetWindowText(Client.Proc.MainWindowHandle, input);
            }
        }

        private void toggleHide_Click(object sender, EventArgs e)
        {
            Client.Resize(0, 0, true);
        }

        private void small_Click(object sender, EventArgs e)
        {
            Client.Resize(640, 480);
        }

        private void large_Click(object sender, EventArgs e)
        {
            Client.Resize(1280, 960);
        }

        private void fullscreen_Click(object sender, EventArgs e)
        {
            Client.Resize(0, 0, false, true);
        }

        private void Thumbnail_Click(object sender, EventArgs e)
        {
            User32.SetForegroundWindow((int)Client.MainHandle);
            User32.ShowWindow(Client.MainHandle, ShowWindowFlags.ActiveShow);
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            DestroyThumbnail();
        }

        internal void DestroyThumbnail(bool kill = true, bool refresh = true)
        {
            if (InvokeRequired)
                Invoke((Action)(() => DestroyThumbnail(kill, refresh)));
            else
            {
                Client.IsRunning = false;

                if (kill)
                    Client.Proc.Kill();

                //set thumbnail to invisible incase its not dead yet
                ThumbnailProperties tProps = new ThumbnailProperties();
                tProps.Visible = false;
                tProps.Flags = ThumbnailFlags.Visible;

                //unregister the thumbnail
                Dwmapi.DwmUnregisterThumbnail(tHandle);
                Dwmapi.DwmUpdateThumbnailProperties(tHandle, ref tProps);

                Hide();
                MainForm.RemoveClient(Client);

                Dispose();

                if (refresh)
                    MainForm.RefreshThumbnails();
            }
        }
    }
}

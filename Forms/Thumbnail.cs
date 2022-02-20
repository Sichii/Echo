using System;
using System.Threading;
using System.Windows.Forms;
using Echo.Definitions;
using Echo.PInvoke;
using Echo.Structs;

namespace Echo;

internal partial class Thumbnail : UserControl
{
    private Client Client;
    private MainForm MainForm;
    private IntPtr ThumbnailHandle;

    internal Thumbnail(MainForm mainForm, Client client)
    {
        MainForm = mainForm;
        Client = client;
        InitializeComponent();
    }

    internal bool Create()
    {
        lock (MainForm.Sync)
        {
            //attempt to register this darkages process to a thumbnail, 0 for success
            if (NativeMethods.DwmRegisterThumbnail(MainForm.Handle, Client.MainWindowHandle, out ThumbnailHandle) == 0)
            {
                //create a new thumbnail properties struct and set properties/location/size/etc
                var tProperties = new ThumbnailProperties
                {
                    Visible = true,
                    Flags = ThumbnailFlags.Visible | ThumbnailFlags.RectDestination | ThumbnailFlags.Opacity |
                            ThumbnailFlags.SourceClientAreaOnly,
                    Opacity = 255,
                    OnlyClientRect = true
                };

                //now determine the location of the thumbnail
                //first we convert this usercontrol's rect to a screen rect
                var screenRect = RectangleToScreen(DisplayRectangle);
                //clientrect will represent the mainform co-ordinates of this form
                var clientRect = MainForm.RectangleToClient(screenRect);

                tProperties.DestinationRect = new(clientRect.Left, clientRect.Top + 24, clientRect.Left + clientRect.Width,
                    clientRect.Top + clientRect.Height);

                //update the thumbnail
                _ = NativeMethods.DwmUpdateThumbnailProperties(ThumbnailHandle, ref tProperties);

                return true;
            }

            return false;
        }
    }

    internal void Destroy()
    {
        lock (MainForm.Sync)
        {
            //set thumbnail to invisible incase its not dead yet
            var tProps = new ThumbnailProperties
            {
                Visible = false,
                Flags = ThumbnailFlags.Visible
            };

            //unregister the thumbnail
            _ = NativeMethods.DwmUnregisterThumbnail(ThumbnailHandle);
            _ = NativeMethods.DwmUpdateThumbnailProperties(ThumbnailHandle, ref tProps);

            Hide();
            MainForm = null!;
            Client = null!;
        }
    }

    internal void Renew()
    {
        _ = NativeMethods.DwmUnregisterThumbnail(ThumbnailHandle);
        Create();
    }

    #region Handlers

    private void ToggleHide_Click(object sender, EventArgs e) => Client.Resize(0, 0, true);

    private void Small_Click(object sender, EventArgs e) => Client.Resize(640, 480);

    private void Large_Click(object sender, EventArgs e) => Client.Resize(1280, 960);

    private void Large4k_Click(object sender, EventArgs e) => Client.Resize(2560, 1920);

    private void Fullscreen_Click(object sender, EventArgs e) => Client.Resize(0, 0, false, true);

    private void ExitBtn_Click(object sender, EventArgs e)
    {
        Client.Process.EnableRaisingEvents = false;
        Client.Destroy();
    }

    private void Thumbnail_Click(object sender, EventArgs e)
    {
        //if it's hidden, unhide it
        if (!NativeMethods.IsWindowVisible(Client.MainWindowHandle))
            Client.Resize(0, 0, true);
        //if it's fullscreen, restore it to it's current position and size
        else if (Client.State.HasFlag(ClientState.Fullscreen))
            NativeMethods.ShowWindowAsync(Client.MainWindowHandle, ShowWindowFlags.ActiveShow);
        //otherwise, restore it to it's last known position and size
        else
            NativeMethods.ShowWindowAsync(Client.MainWindowHandle, ShowWindowFlags.ActiveNormal);

        //wait for window to appear
        while (Client.MainWindowHandle == IntPtr.Zero)
            Thread.Sleep(10);

        //set the window as the foreground window
        _ = NativeMethods.SetForegroundWindow((int)Client.MainWindowHandle);
    }

    #endregion
}
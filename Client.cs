using System;
using System.Diagnostics;

namespace DAWindower
{
    internal class Client
    {
        internal MainForm MainForm;
        internal string Name;
        internal int ProcId;
        internal Thumbnail Thumb;
        internal Rect ClientRect;
        internal Rect WindowRect;
        internal int TitleHeight => WindowRect.Height - ClientRect.Height;
        internal int BorderWidth => WindowRect.Width - ClientRect.Width;

        internal bool IsRunning = true;
        internal IntPtr HiddenHandle;
        internal IntPtr MainHandle => Proc.MainWindowHandle;
        internal IntPtr Handle => Proc.Handle;

        internal Process Proc => Process.GetProcessById(ProcId);

        internal Client(MainForm mainForm, int processId)
        {
            Name = "Unknown";
            MainForm = mainForm;
            ProcId = processId;
            Thumb = new Thumbnail(mainForm, this);
        }

        internal void Resize(int width, int height, bool hide = false, bool fullScreen = false)
        {
            //if toggling hide
            if (hide)
            {
                if (!User32.IsWindowVisible(MainHandle))
                {
                    User32.ShowWindow(HiddenHandle, ShowWindowFlags.ActiveNormal);
                    //the mainwindowhandle changes here, so we need to update the thumbnail
                    Thumb.UpdateThumbnail();
                }
                else
                {
                    HiddenHandle = MainHandle;
                    User32.ShowWindow(MainHandle, ShowWindowFlags.Hide);
                }

                return;
            }//if not toggling hide, then unhide the client if it is hidden
            else if (!User32.IsWindowVisible(MainHandle))
                User32.ShowWindow(MainHandle, ShowWindowFlags.ActiveNormal);


            if (fullScreen)
            {   //set borderless windowed fullscreen
                User32.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                User32.ShowWindowAsync(MainHandle, ShowWindowFlags.ActiveMaximized);
            }
            else
            {
                if (!(User32.GetWindowLong(MainHandle, WindowFlags.Style).HasFlag(WindowStyleFlags.Caption)))
                    User32.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                //get da window and client dimensions
                User32.GetClientRect(MainHandle, ref ClientRect);
                User32.GetWindowRect(MainHandle, ref WindowRect);

                //set window size
                User32.MoveWindow(MainHandle, WindowRect.X, WindowRect.Y, width + BorderWidth, height + TitleHeight, true);
            }
        }
    }
}

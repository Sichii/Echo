using System;
using System.Diagnostics;
using System.Drawing;

namespace DAWindower
{
    internal class Client : IDisposable
    {
        internal ProcessMemoryStream PMS;
        internal DateTime Creation;
        internal MainForm MainForm;
        internal string Name;
        internal int ProcessID;
        internal Thumbnail Thumbnail;
        internal Rect ClientRect;
        internal Rect WindowRect;
        internal ClientState State;
        internal bool IsRunning = true;
        internal IntPtr HiddenHandle;
        internal bool Disposed = false;
        internal bool Disposing = false;

        #region Client/Window Rect
        internal Point Point => WindowRect.Location;
        internal int WinWidth => WindowRect.Width;
        internal int WinHeight => WindowRect.Height;
        internal int CliWidth => ClientRect.Width;
        internal int CliHeight => ClientRect.Height;
        internal int BorderWidth => WindowRect.Width - ClientRect.Width;
        internal int TitleHeight => WindowRect.Height - ClientRect.Height;
        #endregion

        #region Process / Handle
        internal IntPtr MainHandle => Process.MainWindowHandle;
        internal IntPtr Handle => Process.Handle;
        internal Process Process => Process.GetProcessById(ProcessID);
        #endregion

        internal Client(MainForm mainForm, int processId)
        {
            Creation = DateTime.UtcNow;
            Name = $@"Unknown {mainForm.CurrentIndex}";
            MainForm = mainForm;
            ProcessID = processId;
            Thumbnail = new Thumbnail(mainForm, this);

            PMS = new ProcessMemoryStream(ProcessID, ProcessAccessFlags.FullAccess);
        }
        ~Client()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            Disposing = disposing;

            if (disposing)
            {
                MainForm = null;
                PMS.Dispose();
                Thumbnail.Dispose();
            }
            Disposed = true;
        }
        internal void UpdateSize()
        {
            NativeMethods.GetClientRect(MainHandle, ref ClientRect);
            NativeMethods.GetWindowRect(MainHandle, ref WindowRect);
        }

        internal void Resize(int width, int height, bool hide = false, bool fullScreen = false)
        {
            //if toggling hide
            if (hide)
            {
                //if the window isnt visible
                if (!NativeMethods.IsWindowVisible(MainHandle))
                {
                    //restore window to it's current position and size
                    State &= ~ClientState.Hidden;
                    NativeMethods.ShowWindow(HiddenHandle, ShowWindowFlags.ActiveShow);

                    //the mainwindowhandle changes here, so we need to update the thumbnail
                    Thumbnail.UpdateT();
                    Thumbnail.hiddenFsLbl.Visible = false;
                }
                else
                {
                    //if the window is visible, lets hide it
                    State |= ClientState.Hidden;

                    //we need to save the mainhandle, because once it's hidden it will return IntPtr.Zero
                    HiddenHandle = MainHandle;

                    //hide the window
                    NativeMethods.ShowWindow(MainHandle, ShowWindowFlags.Hide);

                    //again, the handle changes here (to Zero) so update the thumbnail
                    if (State.HasFlag(ClientState.Fullscreen))
                    {
                        Thumbnail.UpdateT();
                        Thumbnail.hiddenFsLbl.Visible = true;
                    }
                }
            }
            else
            {
                //if not toggling hide, then unhide the client if it is hidden
                if (!NativeMethods.IsWindowVisible(MainHandle))
                    Resize(0, 0, true);

                //if fullscreen, we arent resizing, we're just removing the titlebar and maximizing
                if (fullScreen)
                {   //set borderless windowed fullscreen
                    State &= ~ClientState.Normal;
                    State |= ClientState.Fullscreen;

                    NativeMethods.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                    NativeMethods.ShowWindowAsync(MainHandle, ShowWindowFlags.ActiveMaximized);
                }
                else
                {
                    //otherwise
                    State &= ~ClientState.Fullscreen;
                    State |= ClientState.Normal;

                    //if it doesnt have a titlebar, set the window back to a normal state (overlappedwindow)
                    if (!(NativeMethods.GetWindowLong(MainHandle, WindowFlags.Style).HasFlag(WindowStyleFlags.Caption)))
                        NativeMethods.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                    //set window size
                    NativeMethods.MoveWindow(MainHandle, WindowRect.X, WindowRect.Y, width + BorderWidth, height + TitleHeight, true);

                    //update rects
                    UpdateSize();
                }
            }
        }
    }
}

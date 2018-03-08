using System;
using System.Diagnostics;
using System.Drawing;

namespace DAWindower
{
    internal class Client
    {
        internal ProcessMemoryStream pms;
        internal DateTime Creation;
        internal MainForm MainForm;
        internal string Name;
        internal int ProcId;
        internal Thumbnail Thumb;
        internal Rect ClientRect;
        internal Rect WindowRect;
        internal ClientState State;
        internal bool IsRunning = true;
        internal IntPtr HiddenHandle;

        #region Client/Window Rect
        internal Point Point => WindowRect.Location;
        internal int wWidth => WindowRect.Width;
        internal int wHeight => WindowRect.Height;
        internal int cWidth => ClientRect.Width;
        internal int cHeight => ClientRect.Height;
        internal int BorderWidth => WindowRect.Width - ClientRect.Width;
        internal int TitleHeight => WindowRect.Height - ClientRect.Height;
        #endregion

        #region Process / Handle
        internal IntPtr MainHandle => Proc.MainWindowHandle;
        internal IntPtr Handle => Proc.Handle;
        internal Process Proc => Process.GetProcessById(ProcId);
        #endregion

        internal Client(MainForm mainForm, int processId)
        {
            Creation = DateTime.UtcNow;
            Name = $@"Unknown {mainForm.CurrentIndex}";
            MainForm = mainForm;
            ProcId = processId;
            Thumb = new Thumbnail(mainForm, this);

            pms = new ProcessMemoryStream(ProcId, ProcessAccessFlags.VmOperation | ProcessAccessFlags.VmRead | ProcessAccessFlags.VmWrite);
        }
        internal void Resize(int width, int height, bool hide = false, bool fullScreen = false)
        {
            //if toggling hide
            if (hide)
            {
                //if the window isnt visible
                if (!User32.IsWindowVisible(MainHandle))
                {
                    //restore window to it's current position and size
                    State &= ~ClientState.Hidden;
                    User32.ShowWindow(HiddenHandle, ShowWindowFlags.ActiveShow);

                    //the mainwindowhandle changes here, so we need to update the thumbnail
                    Thumb.UpdateT();
                    Thumb.hiddenFsLbl.Visible = false;
                }
                else
                {
                    //if the window is visible, lets hide it
                    State |= ClientState.Hidden;

                    //we need to save the mainhandle, because once it's hidden it will return IntPtr.Zero
                    HiddenHandle = MainHandle;

                    //hide the window
                    User32.ShowWindow(MainHandle, ShowWindowFlags.Hide);

                    //again, the handle changes here (to Zero) so update the thumbnail
                    if (State.HasFlag(ClientState.Fullscreen))
                    {
                        Thumb.UpdateT();
                        Thumb.hiddenFsLbl.Visible = true;
                    }
                }
            }
            else
            {
                //if not toggling hide, then unhide the client if it is hidden
                if (!User32.IsWindowVisible(MainHandle))
                    Resize(0, 0, true);

                //if fullscreen, we arent resizing, we're just removing the titlebar and maximizing
                if (fullScreen)
                {   //set borderless windowed fullscreen
                    State &= ~ClientState.Normal;
                    State |= ClientState.Fullscreen;

                    User32.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                    User32.ShowWindowAsync(MainHandle, ShowWindowFlags.ActiveMaximized);
                }
                else
                {
                    //otherwise
                    State &= ~ClientState.Fullscreen;
                    State |= ClientState.Normal;

                    //if it doesnt have a titlebar, set the window back to a normal state (overlappedwindow)
                    if (!(User32.GetWindowLong(MainHandle, WindowFlags.Style).HasFlag(WindowStyleFlags.Caption)))
                        User32.SetWindowLong(MainHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                    //set window size
                    User32.MoveWindow(MainHandle, WindowRect.X, WindowRect.Y, width + BorderWidth, height + TitleHeight, true);

                    //update rects
                    User32.GetClientRect(MainHandle, ref ClientRect);
                    User32.GetWindowRect(MainHandle, ref WindowRect);
                }
            }
        }
    }
}

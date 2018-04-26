using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Echo
{
    internal class Client : IDisposable
    {
        internal ProcessMemoryStream PMS;
        internal DateTime Creation;
        internal MainForm MainForm;
        internal string Name;
        internal Process Process;
        internal IntPtr ThreadHandle;
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
        internal IntPtr MainWindowHandle => Process.MainWindowHandle;
        internal IntPtr Handle => Process.Handle;
        internal int ProcessID => Process.Id;
        #endregion

        internal Client(MainForm mainForm, int processId, IntPtr threadHandle = default(IntPtr))
        {
            Creation = DateTime.UtcNow;
            Name = $@"Unknown {mainForm.CurrentIndex}";
            MainForm = mainForm;
            Process = Process.GetProcessById(processId);
            Thumbnail = new Thumbnail(mainForm, this);
            PMS = new ProcessMemoryStream(ProcessID, ProcessAccessFlags.FullAccess);
            ThreadHandle = threadHandle;

            Process.EnableRaisingEvents = true;
            Process.Exited += new EventHandler(ClientClosed);
        }
        ~Client()
        {
            Dispose(false);
        }

        #region Client Actions
        internal static Client Create(MainForm mainform)
        {
            var dir = Settings.Default.DarkAgesPath;
            var dirDawn = Settings.Default.DarkAgesPath.Replace("Darkages.exe", "dawnd.dll");

            //correct path if required
            if (!File.Exists(dir))
            {
                MessageDialog.Show(mainform, "Could not locate Darkages.exe");
                return null;
            }

            //check for dawnd, if it's not there then write it
            if (!File.Exists(dirDawn))
                File.WriteAllBytes(dirDawn, Properties.Resources.dawnd);

            //create a da process in suspended mode
            StartupInfo startupInfo = new StartupInfo();
            ProcessInfo procInfo = new ProcessInfo();
            startupInfo.Size = Marshal.SizeOf(startupInfo);
            NativeMethods.CreateProcess(dir, null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.Suspended, IntPtr.Zero, null, ref startupInfo, out procInfo);

            //create client from this process
            return new Client(mainform, procInfo.ProcessId, procInfo.ThreadHandle);
        }
        internal void Destroy(bool kill = true)
        {
            MainForm mf = MainForm;

            if (kill)
                Process.Kill();

            mf.Invoke((Action)(() =>
            {
                Thumbnail.Destroy(false);
                mf.RemoveClient(this);
                mf.RefreshThumbnails();

                Dispose();
            }));
        }
        void ClientClosed(object sender, EventArgs e)
        {
            Destroy(false);
        }

        internal void UpdateSize()
        {
            NativeMethods.GetClientRect(MainWindowHandle, ref ClientRect);
            NativeMethods.GetWindowRect(MainWindowHandle, ref WindowRect);
        }
        internal void Resize(int width, int height, bool hide = false, bool fullScreen = false)
        {
            ClientState previousState = State;
            //if toggling hide
            if (hide)
            {
                //if the window isnt visible
                if (!NativeMethods.IsWindowVisible(MainWindowHandle))
                {
                    //restore window to it's current position and size
                    State &= ~ClientState.Hidden;
                    NativeMethods.ShowWindow(HiddenHandle, ShowWindowFlags.ActiveShow);

                    //the mainwindowhandle changes here, so we need to update the thumbnail
                    Thumbnail.Renew();
                    Thumbnail.hiddenFsLbl.Visible = false;
                }
                else
                {
                    //if the window is visible, lets hide it
                    State |= ClientState.Hidden;

                    //we need to save the mainhandle, because once it's hidden it will return IntPtr.Zero
                    HiddenHandle = MainWindowHandle;

                    //hide the window
                    NativeMethods.ShowWindow(MainWindowHandle, ShowWindowFlags.Hide);

                    //again, the handle changes here (to Zero) so update the thumbnail
                    if (State.HasFlag(ClientState.Fullscreen))
                    {
                        Thumbnail.Renew();
                        Thumbnail.hiddenFsLbl.Visible = true;
                    }
                }
            }
            else
            {
                //if not toggling hide, then unhide the client if it is hidden
                if (!NativeMethods.IsWindowVisible(MainWindowHandle))
                    Resize(0, 0, true);

                //if fullscreen, we arent resizing, we're just removing the titlebar and maximizing
                if (fullScreen)
                {   //set borderless windowed fullscreen
                    State &= ~ClientState.Normal;
                    State |= ClientState.Fullscreen;

                    NativeMethods.SetWindowLong(MainWindowHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                    NativeMethods.ShowWindowAsync(MainWindowHandle, ShowWindowFlags.ActiveMaximized);
                }
                else
                {
                    //otherwise
                    State &= ~ClientState.Fullscreen;
                    State |= ClientState.Normal;

                    //if it doesnt have a titlebar, set the window back to a normal state (overlappedwindow)
                    if (!(NativeMethods.GetWindowLong(MainWindowHandle, WindowFlags.Style).HasFlag(WindowStyleFlags.Caption)))
                        NativeMethods.SetWindowLong(MainWindowHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                    //set window size
                    NativeMethods.MoveWindow(MainWindowHandle, WindowRect.X, WindowRect.Y, width + BorderWidth, height + TitleHeight, true);

                    //update rects
                }
            }
            UpdateSize();

            if (previousState == ClientState.Fullscreen && State != ClientState.Fullscreen)
                Resize(width, height, hide, fullScreen);
        }
        #endregion

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
    }
}

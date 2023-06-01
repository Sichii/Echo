using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Echo.Configuration;
using Echo.Definitions;
using Echo.PInvoke;
using Echo.Properties;
using Echo.Structs;
using Settings = Echo.Definitions.Settings;

namespace Echo;

internal class Client : IDisposable
{
    private Rect ClientRect;
    private Rect WindowRect;
    internal DateTime Creation { get; set; }
    internal bool Disposed { get; set; }
    internal bool Disposing { get; set; }
    internal IntPtr HiddenHandle { get; set; }
    internal bool IsRunning { get; set; } = true;
    internal MainForm MainForm { get; set; }
    internal string Name { get; set; }
    internal ClientState State { get; set; }
    internal IntPtr ThreadHandle { get; set; }
    internal VersionElement? Version { get; set; }
    internal byte VersionAttempts { get; set; }

    ~Client() => Dispose(false);
    internal ProcessMemoryStream Pms { get; }
    internal Process Process { get; }
    internal Thumbnail Thumbnail { get; }

    internal Client(MainForm mainForm, int processId, IntPtr threadHandle = default)
    {
        Creation = DateTime.UtcNow;
        Name = $@"Unknown {mainForm.CurrentIndex}";
        MainForm = mainForm;
        Process = Process.GetProcessById(processId);
        Thumbnail = new(mainForm, this);
        Pms = new(ProcessId, ProcessAccessFlags.FullAccess);
        ThreadHandle = threadHandle;

        Process.EnableRaisingEvents = true;
        Process.Exited += ClientClosed;
        TryAutoDetectVersionEarly();
    }

    public void Dispose()
    {
        if (Process.HasExited)
            Process.Exited -= ClientClosed;

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
            MainForm = null!;
            // ReSharper disable once ConstantConditionalAccessQualifier
            Pms?.Dispose();
            // ReSharper disable once ConstantConditionalAccessQualifier
            Thumbnail?.Dispose();
        }

        Disposed = true;
    }

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
    internal int ProcessId => Process.Id;

    #endregion

    #region Client Actions

    internal static Client? Create(MainForm mainform)
    {
        lock (MainForm.Sync)
        {
            var path = Settings.Instance.DarkAgesPath;
            var dir = Path.GetDirectoryName(path)!;
            var dirDawn = Path.Combine(dir, "dawnd.dll");
            var ddrawPath = Path.Combine(dir, "ddraw.dll");

            //correct path if required
            if (!File.Exists(path))
            {
                MessageDialog.Show(mainform, "Could not locate Darkages.exe", "Darkages Path Error");

                return null;
            }

            if (Settings.Instance.UseDDrawCompat)
            {
                if (!File.Exists(ddrawPath))
                    File.WriteAllBytes(ddrawPath, Resources.ddraw);
            }
            else
            {
                if (File.Exists(ddrawPath))
                    File.Delete(ddrawPath);
            }

            //check for dawnd, if it's not there then write it
            if (!File.Exists(dirDawn))
                File.WriteAllBytes(dirDawn, Resources.dawnd);

            //create a da process in suspended mode
            var startupInfo = new StartupInfo();
            startupInfo.Size = Marshal.SizeOf(startupInfo);

            NativeMethods.CreateProcess(path, null, IntPtr.Zero, IntPtr.Zero, false, 
                ProcessCreationFlags.Suspended | ProcessCreationFlags.DetachedProcess | ProcessCreationFlags.NewProcessGroup, 
                IntPtr.Zero, dir,
                ref startupInfo, out var procInfo);

            var client = new Client(mainform, procInfo.ProcessId, procInfo.ThreadHandle);
            client.SetSkipIntro();

            return client;
        }
    }

    internal void TryAutoDetectVersionEarly()
    {
        foreach (var version in MainForm.VersionsCollection.OfType<VersionElement>())
        {
            using var clientFileStream = File.Open(Settings.Instance.DarkAgesPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var hashProvider = MD5.Create();

            hashProvider.ComputeHash(clientFileStream);
            var clientHash = BitConverter.ToString(hashProvider.Hash!)
                .Replace("-", string.Empty);

            if (clientHash.Equals(version.ClientHash, StringComparison.OrdinalIgnoreCase))
            {
                Version = version;

                break;
            }
        }
    }

    internal void TrySetVersion()
    {
        if (Version != null || VersionAttempts > 2)
            return;

        foreach (var version in MainForm.VersionsCollection.OfType<VersionElement>())
        {
            Pms.Position = version.VersionAddressValue;
            var buffer = new byte[4];
            var _ = Pms.Read(buffer, 0, 4);

            var versionValue = BitConverter.ToUInt32(buffer);

            if (versionValue == version.ParsedVersionValue)
            {
                Version = version;

                break;
            }
        }

        VersionAttempts++;
    }

    internal void SetSkipIntro()
    {
        if (Version != null && Version.SkipIntroAddressValue != 0)
        {
            //skip intro
            Pms.Position = Version.SkipIntroAddressValue;
            Pms.WriteByte(0x90);
            Pms.WriteByte(0x90);
            Pms.WriteByte(0x90);
            Pms.WriteByte(0x90);
            Pms.WriteByte(0x90);
            Pms.WriteByte(0x90);
        }
    }

    internal void Destroy(bool kill = true)
    {
        IsRunning = false;
        var mf = MainForm;

        if (kill)
            // ReSharper disable once ConstantConditionalAccessQualifier
            Process?.Kill();

        // ReSharper disable once ConstantConditionalAccessQualifier
        mf?.Invoke(() =>
        {
            lock (MainForm.Sync)
            {
                // ReSharper disable once ConstantConditionalAccessQualifier
                Thumbnail?.Destroy();
                mf.RemoveClient(this);
                mf.RefreshThumbnails();

                Dispose();
            }
        });
    }

    private void ClientClosed(object? sender, EventArgs e) => Destroy(false);

    internal void UpdateSize()
    {
        NativeMethods.GetClientRect(MainWindowHandle, ref ClientRect);
        NativeMethods.GetWindowRect(MainWindowHandle, ref WindowRect);
    }

    internal void Resize(int width, int height, bool hide = false, bool fullScreen = false)
    {
        while (true)
        {
            var previousState = State;

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
                } else
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
            } else
            {
                //if not toggling hide, then unhide the client if it is hidden
                if (!NativeMethods.IsWindowVisible(MainWindowHandle))
                    Resize(0, 0, true);

                //if fullscreen, we arent resizing, we're just removing the titlebar and maximizing
                if (fullScreen)
                { //set borderless windowed fullscreen
                    State &= ~ClientState.Normal;
                    State |= ClientState.Fullscreen;

                    _ = NativeMethods.SetWindowLong(MainWindowHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                    _ = NativeMethods.ShowWindowAsync(MainWindowHandle, ShowWindowFlags.ActiveMaximized);
                } else
                {
                    //otherwise
                    State &= ~ClientState.Fullscreen;
                    State |= ClientState.Normal;

                    //if it doesnt have a titlebar, set the window back to a normal state (overlappedwindow)
                    if (!NativeMethods.GetWindowLong(MainWindowHandle, WindowFlags.Style)
                            .HasFlag(WindowStyleFlags.Caption))
                        _ = NativeMethods.SetWindowLong(MainWindowHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                    //set window size
                    NativeMethods.MoveWindow(MainWindowHandle, WindowRect.X, WindowRect.Y, width + BorderWidth, height + TitleHeight, true);

                    //update rects
                }
            }

            UpdateSize();

            if (previousState == ClientState.Fullscreen && State != ClientState.Fullscreen)
                continue;

            break;
        }

        if(Settings.Instance.UseDDrawCompat)
        {
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }
    }

    #endregion
}
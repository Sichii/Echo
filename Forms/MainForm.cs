using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Echo.Definitions;
using Echo.PInvoke;

namespace Echo
{
    public partial class MainForm : Form
    {
        internal static readonly object Sync = new();
        private readonly CancellationTokenSource CTS = new();
        private Task ClientHandlerTask;
        private List<Client> Clients;
        private OptionsForm Options;

        internal int CurrentIndex
        {
            get
            {
                lock (Clients)
                    return Clients.Count + 1;
            }
        }

        internal List<Client> SafeIterateClients
        {
            get
            {
                lock (Clients)
                    return Clients.Where(client => !client.Disposed && !client.Disposing).ToList();
            }
        }

        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        public MainForm(string[] args)
        {
            Clients = new List<Client>();
            InitializeComponent();
            Settings.Instance.DarkAgesPath = Environment.ExpandEnvironmentVariables(Settings.Instance.DarkAgesPath);
            ClientHandlerTask = Task.Run(() => HandleClients(CTS.Token));

            //populate displays
            while (monitors.DropDownItems.Count > 0)
                monitors.DropDownItems[0].Dispose();

            var count = 1;

            foreach (var screen in Screen.AllScreens)
            {
                var item = new ToolStripMenuItem($@"{screen.DeviceName}", null, ChangePrimaryMonitor, $@"Monitor {count++}");

                if((string.IsNullOrEmpty(Settings.Instance.PrimaryDeviceName) && screen.Primary) 
                    || screen.DeviceName.Equals(Settings.Instance.PrimaryDeviceName, StringComparison.OrdinalIgnoreCase))
                    item.Checked = true;

                monitors.DropDownItems.Add(item);
            }

            foreach(var sizeOption in sizeSelector.DropDownItems.OfType<ToolStripMenuItem>())
                if(sizeOption.Text.Equals(Settings.Instance.SizeSelection, StringComparison.OrdinalIgnoreCase))
                { 
                    sizeOption.Checked = true;
                    break;
                }

            if (args.Length > 0)
            {
                Settings.Instance.DarkAgesPath = args[0];
                launchBtn.Enabled = false;
                optionsBtn.Enabled = false;
                sizeSelector.Enabled = false;

                launchBtn.Visible = false;
                optionsBtn.Visible = false;
                sizeSelector.Visible = false;
            }

            Options = new OptionsForm();
        }

        #region Thumbnail Actions
        internal void RefreshThumbnails()
        {
            try
            {
                lock (Sync)
                    //update all thumbnail locations by unregistering, then recreating
                    foreach (var thumb in SafeIterateClients.Where(client => client.IsRunning).Select(c => c.Thumbnail))
                        thumb.Renew();
            } catch
            {
                // ignored
            }
        }
        #endregion

        #region Lauunch DA
        private void LaunchDA(object sender, EventArgs e)
        {
            var client = Client.Create(this);

            if (client == null)
                return;

            //create access handle and inject dawnd
            if (!InjectDll(NativeMethods.OpenProcess(ProcessAccessFlags.FullAccess, true, client.ProcessId)))
                return;

            //skip intro 0x0042E625
            client.Pms.Position = 0x0042E61F;
            client.Pms.WriteByte(0x90);
            client.Pms.WriteByte(0x90);
            client.Pms.WriteByte(0x90);
            client.Pms.WriteByte(0x90);
            client.Pms.WriteByte(0x90);
            client.Pms.WriteByte(0x90);

            //resume process thread
            _ = NativeMethods.ResumeThread(client.ThreadHandle);

            //wait till window is shown
            while (client.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(10);

            //add client to the client list
            AddClient(client);

            //get inner and outer rect, so we can figure out the title height and border width
            //the inner window needs to have the correct aspect ratio, not the outer

            //get the size of the client and window rects
            client.UpdateSize();

            if (fullscreen.Checked)
                client.Resize(0, 0, false, true);
            else
            {
                client.State |= ClientState.Normal;

                if (large4k.Checked)
                    client.Resize(2560, 1920);
                else if (large.Checked)
                    client.Resize(1280, 960);
            }

            //add this control to the tablelayoutview with automatic placement
            thumbTbl.Controls.Add(client.Thumbnail, -1, -1);
            //create the thumbnail using this control's position
            client.Thumbnail.Create();

            //show this control
            client.Thumbnail.Visible = true;
            client.Thumbnail.Show();
        }

        private bool InjectDll(IntPtr accessHandle)
        {
            const string DLL_NAME = "dawnd.dll";

            //length of string containing the DLL file name +1 byte padding
            var nameLength = DLL_NAME.Length + 1;

            //allocate memory within the virtual address space of the target process
            var allocate = NativeMethods.VirtualAllocEx(
                accessHandle, (IntPtr)null, (IntPtr)nameLength, 0x1000, 0x40); //allocation pour WriteProcessMemory

            //write DLL file name to allocated memory in target process
            NativeMethods.WriteProcessMemory(accessHandle, allocate, DLL_NAME, (UIntPtr)nameLength, out _);
            //retreive function pointer for remote thread
            var injectionPtr = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            //if failed to retreive function pointer
            if (injectionPtr == UIntPtr.Zero)
            {
                MessageDialog.Show(this, "Injection pointer was null.", "Injection Error");

                //return failed
                return false;
            }

            //create thread in target process, and store accessHandle in hThread
            var thread = NativeMethods.CreateRemoteThread(accessHandle, (IntPtr)null, IntPtr.Zero, injectionPtr, allocate, 0, out _);

            //make sure thread accessHandle is valid
            if (thread == IntPtr.Zero)
            {
                //incorrect thread accessHandle ... return failed
                MessageDialog.Show(this, "Remote injection thread was null. Try again...", "Injection Error");

                return false;
            }

            //time-out is 10 seconds...
            var result = NativeMethods.WaitForSingleObject(thread, 10 * 1000);

            //check whether thread timed out...
            if (result != WaitEventResult.Signaled)
            {
                //thread timed out...
                MessageDialog.Show(this, "Injection thread timed out, or signaled incorrectly. Try again...", "Injection Error");

                //make sure thread accessHandle is valid before closing... prevents crashes.
                if (thread != IntPtr.Zero)
                    //close thread in target process
                    NativeMethods.CloseHandle(thread);

                return false;
            }

            //free up allocated space ( AllocMem )
            NativeMethods.VirtualFreeEx(accessHandle, allocate, (UIntPtr)0, 0x8000);

            //make sure thread accessHandle is valid before closing... prevents crashes.
            if (thread != IntPtr.Zero)
                //close thread in target process
                NativeMethods.CloseHandle(thread);

            //return succeeded
            return true;
        }
        #endregion

        #region Cascade
        private void AllVisible(bool skipPrimary = false, List<Client> skipList = default)
        {
            //list of clients and their destination points
            var cascader = new Dictionary<Client, Point>();
            //list of all displays
            var screens = Screen.AllScreens.ToList();

            skipList ??= new List<Client>();

            //represents the index of the current display
            var current = -1;

            //sets the current display to their selected primary display
            foreach (ToolStripMenuItem item in monitors.DropDownItems)
                if (item.Checked)
                {
                    current = screens.FindIndex(s => s.DeviceName == item.Text);

                    break;
                }

            //if that failed, dont do anything
            if (current == -1)
                return;

            if (skipPrimary)
            {
                current++;

                if (current >= screens.Count)
                    current = 0;
            }

            //represents the current and maximum bounds of the current display
            var x = screens[current].Bounds.Left - 10;
            var y = screens[current].Bounds.Top + 10;
            var xMax = screens[current].Bounds.Right + 50;
            var yMax = screens[current].Bounds.Bottom + 50;

            //for each client
            foreach (var client in SafeIterateClients.Except(skipList))
            {
                //resize it to small (or large if 4k)
                if (screens[current].Bounds.Width > 3000)
                    client.Resize(1280, 960);
                else
                    client.Resize(640, 480);

                //add this client, point pair to the cascade dic
                cascader.Add(client, new Point(x, y));

                //set co-ordinates for the next client
                //tiles horizontally, then vertically, then to next screen
                x += client.CliWidth;

                if (x + client.CliWidth > xMax)
                {
                    x = screens[current].Bounds.Left - 10;
                    y += client.WinHeight;

                    if (y + client.WinHeight > yMax)
                    {
                        current++;

                        if (current >= screens.Count)
                            current = 0;

                        //if we're going to a new screen, make sure to re-grab the bounds of the new screen
                        x = screens[current].Bounds.Left - 10;
                        y = screens[current].Bounds.Top + 25;
                        xMax = screens[current].Bounds.Right + 50;
                        yMax = screens[current].Bounds.Bottom + 50;
                    }
                }
            }

            foreach ((var client, var point) in cascader)
                NativeMethods.MoveWindow(client.MainWindowHandle, point.X, point.Y, client.WinWidth, client.WinHeight, true);
        }

        private void Commander(string name)
        {
            //list of clients and their destination points
            var cascader = new Dictionary<Client, Point>();
            //list of all displays
            var screens = Screen.AllScreens.ToList();

            //grab the commander as designated by the item that was clicked
            var cmdr = SafeIterateClients.FirstOrDefault(client => client.Name == name);

            //represents the index of the current display
            var current = -1;

            //sets the current display to their selected primary display
            foreach (ToolStripMenuItem dropItem in monitors.DropDownItems)
                if (dropItem.Checked)
                {
                    current = screens.FindIndex(menuItem => menuItem.DeviceName.Equals(dropItem.Text));

                    break;
                }

            //if that failed, dont do anything
            if ((current == -1) || (cmdr == null))
                return;

            //represents the current and maximum bounds of the current display
            var x = screens[current].Bounds.Left - 10;
            var y = screens[current].Bounds.Top + 30;

            //resize commander to large (or large4k if 4k)
            if (screens[current].Bounds.Width > 3000)
                cmdr.Resize(2560, 1920);
            else
                cmdr.Resize(1280, 960);

            //add commander to be placed
            cascader.Add(cmdr, new Point(x, y));

            //set next client to be to the right of the commander window
            x = screens[current].Bounds.Left + cmdr.CliWidth - 10;
            y = screens[current].Bounds.Top + 15;

            //for the first 2 clients that arent the commander
            foreach (var client in SafeIterateClients.Where(client => client != cmdr).Take(2))
            {
                //resize it to small (or large if 4k)
                if (screens[current].Bounds.Width > 3000)
                    client.Resize(1280, 960);
                else
                    client.Resize(640, 480);

                //add the first one
                cascader.Add(client, new Point(x, y));

                //set the y to be below the first one, for the position of the 2nd
                y = screens[current].Bounds.Top + client.WinHeight + 6;
            }

            //for the rest of the clients, do all visible on the next monitor
            AllVisible(true, cascader.Keys.ToList());

            foreach ((var client, var point) in cascader)
                NativeMethods.MoveWindow(client.MainWindowHandle, point.X, point.Y, client.WinWidth, client.WinHeight, true);
        }
        #endregion

        #region Client Actions
        internal bool AddClient(Client client)
        {
            lock (Clients)
                if (!Clients.Select(cli => cli.ProcessId).Contains(client.ProcessId))
                {
                    Clients.Add(client);

                    return true;
                }

            return false;
        }

        internal void RemoveClient(Client client)
        {
            lock (Clients)
                //safely remove a client from the list
                Clients.Remove(client);
        }

        private async Task HandleClients(CancellationToken cancellationToken)
        {
            //wait till the form is shown
            while (!Visible)
            {
                if(cancellationToken.IsCancellationRequested)
                    return;

                try
                { 
                    await Task.Delay(10, cancellationToken).ConfigureAwait(false);
                } catch
                {
                    //ignored
                    return;
                }
            }

            while (true)
            {
                try
                { 
                    if(cancellationToken.IsCancellationRequested)
                        return;

                    //refresh client list
                    //for each active darkages process that we dont have added
                    foreach (var proc in Process.GetProcessesByName("Darkages")
                            .Where(proc => !SafeIterateClients.Select(client => client.ProcessId).Contains(proc.Id)))
                        //for each module in that process
                        foreach (var mod in proc.Modules)
                            //if that darkages window contains dawnd.dll
                            if (((ProcessModule)mod).ModuleName!.Equals("dawnd.dll", StringComparison.CurrentCultureIgnoreCase))
                            {
                                //add it
                                //create a new client and add it to the client list
                                var newClient = new Client(this, proc.Id);
                                newClient.Creation -= new TimeSpan(0, 0, 10);
                                newClient.State |= ClientState.Normal;

                                if (!AddClient(newClient))
                                    break;

                                //make sure the window is shown
                                var now = DateTime.UtcNow;

                                while (proc.MainWindowHandle == IntPtr.Zero)
                                {
                                    if (DateTime.UtcNow.Subtract(now).TotalMilliseconds > 500)
                                        break;

                                    Thread.Sleep(10);
                                }

                                //update the stored rects to reflect their size selection
                                newClient.UpdateSize();

                                Invoke(() =>
                                {
                                    //add this control to the tablelayoutview with automatic placement
                                    thumbTbl.Controls.Add(newClient.Thumbnail, -1, -1);
                                    //create the thumbnail using this control's position
                                    newClient.Thumbnail.Create();

                                    //show this control
                                    newClient.Thumbnail.Visible = true;
                                    newClient.Thumbnail.Show();
                                });

                                break;
                            }

                    //for each client over 5 seconds old, and rendered
                    foreach (var client in SafeIterateClients.Where(c => DateTime.UtcNow.Subtract(c.Creation).TotalSeconds > 5))
                    {
                        //name max length is 13 characters
                        var buffer = new byte[13];
                        //seek to the memory position of the name
                        client.Pms.Position = 0x73D910;
                        //read it (marshal.copy into buffer)
                        client.Pms.Read(buffer, 0, 13);

                        //get the name, remove trailing null characters
                        //split incase they relogged in an already-used client (overwrites same memory space and ends with a null character)
                        var name = Encoding.UTF8.GetString(buffer).Trim('\0').Split('\0')[0];

                        //set window, thumb, and name if it's valid
                        if (!string.IsNullOrWhiteSpace(name)
                            && !client.Thumbnail.windowTitleLbl.Text.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                            Invoke(() =>
                            {
                                client.Thumbnail.windowTitleLbl.Text = name;
                                client.Name = name;
                                _ = NativeMethods.SetWindowText(client.Process.MainWindowHandle, name);
                            });
                    }

                    try
                    {
                        await Task.Delay(5000, cancellationToken).ConfigureAwait(false);
                    } catch
                    {
                        if(cancellationToken.IsCancellationRequested)
                            return;
                    }
                } catch
                {
                    //ignored
                }
            }
        }
        #endregion

        #region Handlers
        private void DropDownCheck(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;

            small.Checked = false;
            large.Checked = false;
            large4k.Checked = false;
            fullscreen.Checked = false;

            item.Checked = true;
            Settings.Instance.SizeSelection = item.Text;
        }

        private void AllToggleHide_Click(object sender, EventArgs e)
        {
            foreach (var client in SafeIterateClients)
                client.Resize(0, 0, true);
        }

        private void AllSmall_Click(object sender, EventArgs e)
        {
            foreach (var client in SafeIterateClients)
                client.Resize(640, 480);
        }

        private void AllLarge_Click(object sender, EventArgs e)
        {
            foreach (var client in SafeIterateClients)
                client.Resize(1280, 960);
        }

        private void AllLarge4k_Click(object sender, EventArgs e)
        {
            foreach (var client in SafeIterateClients)
                client.Resize(2560, 1920);
        }

        private void OptionsBtn_Click(object sender, EventArgs e) => Options.ShowDialog(this);

        private void Commander_MouseEnter(object sender, EventArgs e)
        {
            while (commander.DropDownItems.Count > 0)
                commander.DropDownItems[0].Dispose();

            foreach (var client in SafeIterateClients)
            {
                var item = new ToolStripMenuItem(client.Name, null, Commander_Click, client.Name);
                commander.DropDownItems.Add(item);
            }
        }

        private void ChangePrimaryMonitor(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem dropItem in monitors.DropDownItems)
                dropItem.Checked = false;

            item.Checked = true;

            Settings.Instance.PrimaryDeviceName = item.Text;
        }

        private void AllVisible_Click(object sender, EventArgs e) => AllVisible();

        private void Commander_Click(object sender, EventArgs e)
        {
            //item that was clicked
            var item = (ToolStripMenuItem)sender;

            Commander(item.Name);
        }

        private void DropClosed(object sender, EventArgs e) => ((ToolStripDropDownItem)sender).DropDown.Close();

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Instance.Save();

            CTS.Cancel();

            Options.Dispose();
            Options = null;

            lock (Clients)
            {
                foreach (var client in Clients.ToList())
                    client.Dispose();

                Clients = null;
            }
        }
        #endregion
    }
}
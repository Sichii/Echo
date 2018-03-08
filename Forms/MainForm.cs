using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;
using System.Drawing;
using System.Text;

namespace DAWindower
{
    internal partial class MainForm : Form
    {
        private Thread ClientHandlerThread;
        private List<Client> Clients;

        internal int CurrentIndex
        {
            get
            {
                lock (Clients)
                    return Clients.Count + 1;
            }
        }

        internal MainForm()
        {
            Clients = new List<Client>();
            InitializeComponent();
            Settings.Default.DarkAgesPath = Environment.ExpandEnvironmentVariables(Settings.Default.DarkAgesPath);
            ClientHandlerThread = new Thread(new ThreadStart(HandleClients));
            ClientHandlerThread.Start();

            //populate displays
            while (monitors.DropDownItems.Count > 0)
                monitors.DropDownItems[0].Dispose();

            int count = 1;
            foreach (Screen screen in Screen.AllScreens)
            {
                ToolStripMenuItem item = new ToolStripMenuItem($@"{screen.DeviceName}", null, ChangePrimaryMonitor, $@"Monitor {count}");

                if (screen.Primary)
                    item.Checked = true;

                monitors.DropDownItems.Add(item);
            }
        }

        #region Lauunch DA
        private void LaunchDA(object sender, EventArgs e)
        {
            var dir = Settings.Default.DarkAgesPath;
            var dirDawn = Settings.Default.DarkAgesPath.Replace("Darkages.exe", "dawnd.dll");

            //correct path if required
            if (!File.Exists(dir))
                MessageDialog.Show(this, "Could not locate Darkages.exe");

            //check for dawnd, if it's not there then write it
            if (!File.Exists(dirDawn))
                File.WriteAllBytes(dirDawn, Properties.Resources.dawnd);

            //create a da process
            StartInfo startupInfo = new StartInfo();
            ProcInfo procInfo = new ProcInfo();

            startupInfo.Size = Marshal.SizeOf(startupInfo);
            Kernel32.CreateProcess(dir, null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.Suspended, IntPtr.Zero, null, ref startupInfo, out procInfo);

            //create client from this process
            Client client = new Client(this, procInfo.ProcessId);

            //open an access handle to this process
            IntPtr hProcess = Kernel32.OpenProcess(ProcessAccessFlags.FullAccess, true, procInfo.ProcessId);
            //inject dawnd
            if (!InjectDLL(hProcess))
                return;

            //skip intro 0x0042E625
            client.pms.Position = 0x0042E61F;
            client.pms.WriteByte(0x90);
            client.pms.WriteByte(0x90);
            client.pms.WriteByte(0x90);
            client.pms.WriteByte(0x90);
            client.pms.WriteByte(0x90);
            client.pms.WriteByte(0x90);

            //resume process thread
            Kernel32.ResumeThread(procInfo.ThreadHandle);


            lock (Clients)
                Clients.Add(client);

            //wait till window is shown
            Process p = Process.GetProcessById(procInfo.ProcessId);
            while (p.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(10);

            //get inner and outer rect, so we can figure out the title height and border width
            //the inner window needs to have the correct aspect ratio, not the outer
            User32.GetClientRect(p.MainWindowHandle, ref client.ClientRect);
            User32.GetWindowRect(p.MainWindowHandle, ref client.WindowRect);

            if (fullscreen.Checked)
            {
                client.State |= ClientState.Fullscreen;
                //set window to simply visible : not title bar, resizing, border/frame, etc
                User32.SetWindowLong(p.MainWindowHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                //maximize the window and activate it
                User32.ShowWindowAsync(p.MainWindowHandle, ShowWindowFlags.ActiveMaximized);
            }
            else if (large.Checked)
            {
                client.State |= ClientState.Normal;
                User32.MoveWindow(p.MainWindowHandle, client.WindowRect.X, client.WindowRect.Y, 1280 + client.BorderWidth, 960 + client.TitleHeight, true);
            }
            else
                client.State |= ClientState.Normal;

            //update the stored rects to reflect their size selection
            User32.GetClientRect(client.MainHandle, ref client.ClientRect);
            User32.GetWindowRect(client.MainHandle, ref client.WindowRect);

            //add this control to the tablelayoutview with automatic placement
            thumbTbl.Controls.Add(client.Thumb, -1, -1);
            //create the thumbnail using this control's position
            client.Thumb.CreateT();

            //show this control
            client.Thumb.Visible = true;
            client.Thumb.Show();
        }

        private bool InjectDLL(IntPtr accessHandle)
        {
            string dllName = "dawnd.dll";
            IntPtr bytesout;

            //length of string containing the DLL file name +1 byte padding
            int nameLength = dllName.Length + 1;
            //allocate memory within the virtual address space of the target process
            IntPtr allocate = Kernel32.VirtualAllocEx(accessHandle, (IntPtr)null, (uint)nameLength, 0x1000, 0x40); //allocation pour WriteProcessMemory

            //write DLL file name to allocated memory in target process
            Kernel32.WriteProcessMemory(accessHandle, allocate, dllName, (UIntPtr)nameLength, out bytesout);
            //retreive function pointer for remote thread
            UIntPtr injectionPtr = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            //if failed to retreive function pointer
            if (injectionPtr == null)
            {
                MessageDialog.Show(this, "Error 1");
                //return failed
                return false;
            }

            //create thread in target process, and store accessHandle in hThread
            IntPtr thread = Kernel32.CreateRemoteThread(accessHandle, (IntPtr)null, 0, injectionPtr, allocate, 0, out bytesout);
            //make sure thread accessHandle is valid
            if (thread == null)
            {
                //incorrect thread accessHandle ... return failed
                MessageDialog.Show(this, "Error 2 Try again..");
                return false;
            }
            //time-out is 10 seconds...
            WaitEventResult result = Kernel32.WaitForSingleObject(thread, 10 * 1000);
            //check whether thread timed out...
            if (result != WaitEventResult.Signaled)
            {
                //thread timed out...
                MessageDialog.Show(this, " Error 3 Try again...");
                //make sure thread accessHandle is valid before closing... prevents crashes.
                if (thread != null)
                {
                    //close thread in target process
                    Kernel32.CloseHandle(thread);
                }
                return false;
            }
            //free up allocated space ( AllocMem )
            Kernel32.VirtualFreeEx(accessHandle, allocate, (UIntPtr)0, 0x8000);
            //make sure thread accessHandle is valid before closing... prevents crashes.
            if (thread != null)
            {
                //close thread in target process
                Kernel32.CloseHandle(thread);
            }
            //return succeeded
            return true;
        }
        #endregion

        #region Cascade
        private void AllVisible(bool skipPrimary = false, List<Client> skipList = default(List<Client>))
        {
            //list of clients and their destination points
            Dictionary<Client, Point> cascader = new Dictionary<Client, Point>();
            //list of all displays
            List<Screen> Screens = Screen.AllScreens.ToList();
            //represents the index of the current display
            int current = -1;

            //sets the current display to their selected primary display
            foreach (ToolStripMenuItem item in monitors.DropDownItems)
                if (item.Checked)
                {
                    current = Screens.FindIndex(s => s.DeviceName == item.Text);
                    break;
                }

            //if that failed, dont do anything
            if (current == -1)
                return;

            if (skipPrimary)
            {
                current++;

                if (current >= Screens.Count)
                    current = 0;
            }

            if (skipList == null)
                skipList = new List<Client>();

            //represents the current and maximum bounds of the current display
            int X = Screens[current].Bounds.Left - 10;
            int Y = Screens[current].Bounds.Top + 10;
            int xMax = Screens[current].Bounds.Right + 50;
            int yMax = Screens[current].Bounds.Bottom + 50;

            lock (Clients)
            {
                //for each client
                foreach (Client client in Clients.Except(skipList))
                {
                    //resize it to small (or large if 4k)
                    if (Screens[current].Bounds.Width > 3000)
                        client.Resize(1280, 960);
                    else
                        client.Resize(640, 480);
                    //add this client, point pair to the cascade dic
                    cascader.Add(client, new Point(X, Y));

                    //set co-ordinates for the next client
                    //tiles horizontally, then vertically, then to next screen
                    X += client.cWidth;

                    if (X + client.cWidth > xMax)
                    {
                        X = Screens[current].Bounds.Left - 10;
                        Y += client.wHeight;

                        if (Y + client.wHeight > yMax)
                        {
                            current++;

                            if (current >= Screens.Count)
                                current = 0;

                            //if we're going to a new screen, make sure to re-grab the bounds of the new screen
                            X = Screens[current].Bounds.Left - 10;
                            Y = Screens[current].Bounds.Top + 25;
                            xMax = Screens[current].Bounds.Right + 50;
                            yMax = Screens[current].Bounds.Bottom + 50;
                        }
                    }
                }
            }

            foreach (var kvp in cascader)
                User32.MoveWindow(kvp.Key.MainHandle, kvp.Value.X, kvp.Value.Y, kvp.Key.wWidth, kvp.Key.wHeight, true);
        }
        private void Commander(string name)
        {
            //list of clients and their destination points
            Dictionary<Client, Point> cascader = new Dictionary<Client, Point>();
            //list of all displays
            List<Screen> Screens = Screen.AllScreens.ToList();
            //the commander
            Client commander;

            //grab the commander as designated by the item that was clicked
            lock (Clients)
                commander = Clients.FirstOrDefault(c => c.Name == name);

            //represents the index of the current display
            int current = -1;

            //sets the current display to their selected primary display
            foreach (ToolStripMenuItem dropItem in monitors.DropDownItems)
                if (dropItem.Checked)
                {
                    current = Screens.FindIndex(s => s.DeviceName.Equals(dropItem.Text));
                    break;
                }

            //if that failed, dont do anything
            if (current == -1 || commander == null)
                return;

            //represents the current and maximum bounds of the current display
            int X = Screens[current].Bounds.Left - 10;
            int Y = Screens[current].Bounds.Top + 30;

            //resize commander to large (or large4k if 4k)
            if (Screens[current].Bounds.Width > 3000)
                commander.Resize(2560, 1920);
            else
                commander.Resize(1280, 960);

            //add commander to be placed
            cascader.Add(commander, new Point(X, Y));

            //set next client to be to the right of the commander window
            X = Screens[current].Bounds.Left + commander.cWidth - 10;
            Y = Screens[current].Bounds.Top + 15;

            lock (Clients)
            {
                //for the first 2 clients that arent the commander
                foreach (Client client in Clients.Where(c => c != commander).Take(2))
                {
                    //resize it to small (or large if 4k)
                    if (Screens[current].Bounds.Width > 3000)
                        client.Resize(1280, 960);
                    else
                        client.Resize(640, 480);

                    //add the first one
                    cascader.Add(client, new Point(X, Y));

                    //set the y to be below the first one, for the position of the 2nd
                    Y = Screens[current].Bounds.Top + client.wHeight;
                }

                //for the rest of the clients, do all visible on the next monitor
                AllVisible(true, cascader.Keys.ToList());
            }

            foreach (var kvp in cascader)
                User32.MoveWindow(kvp.Key.MainHandle, kvp.Value.X, kvp.Value.Y, kvp.Key.wWidth, kvp.Key.wHeight, true);
        }
        #endregion

        #region Thumbnail Actions
        internal void RefreshThumbnails()
        {
            if (InvokeRequired)
                Invoke((Action)(() => RefreshThumbnails()));
            else
            {
                lock (Clients)
                {
                    //update all thumbnail locations by unregistering, then recreating
                    foreach (Thumbnail thumb in Clients.Where(c => c.IsRunning).Select(c => c.Thumb))
                        thumb.UpdateT();
                }
            }
        }
        #endregion

        #region Client Actions
        internal void RemoveClient(Client client)
        {
            lock (Clients)
            {
                //safely remove a client from the list
                client.pms.Dispose();
                Clients.Remove(client);
            }
        }
        private void HandleClients()
        {
            DateTime lastNameCheck = DateTime.UtcNow;

            while (!Visible)
                Thread.Sleep(10);

            while (Visible)
            {
                List<Client> currentClients;
                List<int> notRunning;

                lock (Clients)
                {
                    //get current clients (that we see)
                    currentClients = Clients.ToList();

                    if (currentClients.Count == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    //any client we have but the system doesnt, was closed
                    notRunning = Clients.Select(c => c.ProcId).Except(Process.GetProcessesByName("Darkages").Select(p => p.Id)).ToList();
                }

                //if we have any clients that the system doesnt
                if (notRunning.Count > 0)
                {
                    //remove their thumbnail
                    foreach (Client client in currentClients)
                        if (notRunning.Contains(client.ProcId))
                            client.Thumb.DestroyT(false, false);

                    //refresh other thumbnails
                    RefreshThumbnails();
                }

                //only check names every 5 seconds
                if (DateTime.UtcNow.Subtract(lastNameCheck).TotalSeconds > 5)
                {
                    //refresh client list
                    lock (Clients)
                        currentClients = Clients.ToList();

                    //for each client over 5 seconds old, and rendered
                    foreach (Client client in currentClients.Where(c => DateTime.UtcNow.Subtract(c.Creation).TotalSeconds > 5 && c.MainHandle != IntPtr.Zero))
                    {
                        //name max length is 13 characters
                        byte[] buffer = new byte[13];
                        //seek to the memory position of the name
                        client.pms.Position = 0x73D910;
                        //read it (marshal.copy into buffer)
                        client.pms.Read(buffer, 0, 13);

                        //get the name, remove trailing null characters
                        //split incase they relogged in an already-used client (overwrites same memory space and ends with a null character)
                        string name = Encoding.UTF8.GetString(buffer).Trim('\0').Split('\0')[0];
                        //set window, thumb, and name if it's valid
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            Invoke((Action)(() => client.Thumb.windowTitleLbl.Text = name));
                            client.Name = name;
                            User32.SetWindowText(client.Proc.MainWindowHandle, name);
                        }
                    }

                    //only check every 5seconds
                    lastNameCheck = DateTime.UtcNow;
                }
                Thread.Sleep(500);
            }

            return;
        }
        #endregion

        #region Handlers
        private void DropDownCheck(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            small.Checked = false;
            large.Checked = false;
            fullscreen.Checked = false;

            item.Checked = true;
        }

        private void toggleHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client cli in Clients)
                cli.Resize(0, 0, true);
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client cli in Clients)
                cli.Resize(640, 480);
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Client cli in Clients)
                cli.Resize(1280, 960);
        }


        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm opt = new OptionsForm();
            opt.Show();
        }

        private void large4k_Click(object sender, EventArgs e)
        {
            foreach (Client cli in Clients)
                cli.Resize(2560, 1920);
        }

        private void commander_MouseEnter(object sender, EventArgs e)
        {
            while (commander.DropDownItems.Count > 0)
                commander.DropDownItems[0].Dispose();

            lock (Clients)
            {
                foreach (Client c in Clients)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(c.Name, null, commander_Click, c.Name);
                    commander.DropDownItems.Add(item);
                }
            }
        }

        private void ChangePrimaryMonitor(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            foreach (ToolStripMenuItem dropItem in monitors.DropDownItems)
                dropItem.Checked = false;

            item.Checked = true;
        }

        private void allVisible_Click(object sender, EventArgs e)
        {
            AllVisible();
        }

        private void commander_Click(object sender, EventArgs e)
        {
            //item that was clicked
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            Commander(item.Name);
        }

        private void dropClosed(object sender, EventArgs e)
        {
            (sender as ToolStripDropDownItem).DropDown.Close();
        }
        #endregion
    }
}

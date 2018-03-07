using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Linq;

namespace DAWindower
{
    internal partial class MainForm : Form
    {
        private Thread ClientHandlerThread;
        private List<Client> Clients;

        internal MainForm()
        {
            Clients = new List<Client>();
            InitializeComponent();
            ClientHandlerThread = new Thread(new ThreadStart(HandleClients));
            ClientHandlerThread.Start();
        }

        private void LaunchDA(object sender, EventArgs e)
        {
            string dir = @"C:\Program Files (x86)\KRU\Dark Ages\";

            //correct path if required
            if (!File.Exists($@"{dir}Darkages.exe"))
            {
                dir = dir.Replace(" (x86)", "");
                if (!File.Exists($@"{dir}Darkages.exe"))
                    MessageDialog.Show(this, "Could not locate Darkages.exe");
            }

            //check for dawnd, if it's not there then write it
            if (!File.Exists($@"{dir}dawnd.dll"))
                File.WriteAllBytes($@"{dir}dawnd.dll", Properties.Resources.dawnd);

            //create a da process
            StartInfo startupInfo = new StartInfo();
            ProcInfo procInfo = new ProcInfo();

            startupInfo.Size = Marshal.SizeOf(startupInfo);
            Kernel32.CreateProcess($@"{dir}Darkages.exe", null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.Suspended, IntPtr.Zero, null, ref startupInfo, out procInfo);

            //open an access handle to this process
            IntPtr hProcess = Kernel32.OpenProcess(ProcessAccessFlags.FullAccess, true, procInfo.ProcessId);
            //inject dawnd
            InjectDLL(hProcess);

            //resume process thread
            Kernel32.ResumeThread(procInfo.ThreadHandle);

            //create client from this process
            Client client = new Client(this, procInfo.ProcessId);
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

            //add this control to the tablelayoutview with automatic placement
            thumbTbl.Controls.Add(client.Thumb, -1, -1);
            //create the thumbnail using this control's position
            client.Thumb.CreateT();

            //show this control
            client.Thumb.Visible = true;
            client.Thumb.Show();
        }

        internal bool InjectDLL(IntPtr accessHandle)
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
                MessageDialog.Show(this, "Error 1\n");
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

        internal void HandleClients()
        {
            while (!Visible)
                Thread.Sleep(10);

            while (Visible)
            {
                List<Client> currentClients = Clients.ToList();
                List<int> notRunning = Clients.Select(c => c.ProcId).Except(Process.GetProcessesByName("Darkages").Select(p => p.Id)).ToList();

                if (notRunning.Count > 0)
                {
                    foreach (Client client in currentClients)
                        if (notRunning.Contains(client.ProcId))
                            client.Thumb.DestroyThumbnail(false, false);

                    RefreshThumbnails();
                }
                Thread.Sleep(250);
            }

            return;
        }

        internal void RefreshThumbnails()
        {
            if (InvokeRequired)
                Invoke((Action)(() => RefreshThumbnails()));
            else
            {
                lock (Clients)
                {
                    foreach (Thumbnail thumb in Clients.Where(c => c.IsRunning).Select(c => c.Thumb))
                        thumb.UpdateT();
                }
            }
        }

        internal void RemoveClient(Client client)
        {
            lock (Clients)
            {
                Clients.Remove(client);
            }
        }

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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DAWindower
{
    public partial class MainForm : Form
    {
        private List<Client> Clients;

        public MainForm()
        {
            Clients = new List<Client>();
            InitializeComponent();
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

            //create a da process, inject, add the client to our list
            StartInfo startupInfo = new StartInfo();
            ProcInfo procInfo = new ProcInfo();

            startupInfo.Size = Marshal.SizeOf(startupInfo);
            Kernel32.CreateProcess($@"{dir}Darkages.exe", null, IntPtr.Zero, IntPtr.Zero, false, ProcessCreationFlags.Suspended, IntPtr.Zero, null, ref startupInfo, out procInfo);

            IntPtr hProcess = Kernel32.OpenProcess(ProcessAccessFlags.FullAccess, true, procInfo.ProcessId);
            InjectDLL(hProcess);

            Kernel32.ResumeThread(procInfo.ThreadHandle);

            Client client = new Client(this, procInfo.ProcessId);
            Clients.Add(client);

            Process p = Process.GetProcessById(procInfo.ProcessId);
            while (p.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(10);



            //generate and show the thumbnail
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

        private void DropDownCheck(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            small.Checked = false;
            large.Checked = false;
            fullscreen.Checked = false;

            item.Checked = true;
        }
    }
}

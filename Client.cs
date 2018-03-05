using System;
using System.Diagnostics;

namespace DAWindower
{
    internal class Client
    {
        internal MainForm MainForm;
        internal string Name;
        internal int ProcId;
        internal Process Proc => Process.GetProcessById(ProcId);
        internal Thumbnail Thumb;
        internal IntPtr Handle => Proc.MainWindowHandle;

        internal Client(MainForm mainForm, int processId)
        {
            MainForm = mainForm;
            ProcId = processId;
            Thumb = new Thumbnail();
        }

        internal bool InjectDLL()
        {
            IntPtr accessHandle = Kernel32.OpenProcess(ProcessAccessFlags.FullAccess, true, Proc.Id);
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
                MessageDialog.Show(MainForm, "Error 1\n");
                //return failed
                return false;
            }

            //create thread in target process, and store accessHandle in hThread
            IntPtr thread = Kernel32.CreateRemoteThread(accessHandle, (IntPtr)null, 0, injectionPtr, allocate, 0, out bytesout);
            //make sure thread accessHandle is valid
            if (thread == null)
            {
                //incorrect thread accessHandle ... return failed
                MessageDialog.Show(MainForm, "Error 2 Try again..");
                return false;
            }
            //time-out is 10 seconds...
            WaitEventResult result = Kernel32.WaitForSingleObject(thread, 10 * 1000);
            //check whether thread timed out...
            if (result != WaitEventResult.Signaled)
            {
                //thread timed out...
                MessageDialog.Show(MainForm, " Error 3 Try again...");
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
    }
}

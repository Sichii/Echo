using System;
using System.IO;
using Echo.Definitions;
using Echo.PInvoke;
using Echo.Properties;
using Echo.Structs;

namespace Echo
{
    internal static class External
    {
        internal static void Resize(IntPtr mainWindowHandle, int width, int height, bool fullScreen)
        {
            //if fullscreen, we arent resizing, we're just removing the titlebar and maximizing
            if (fullScreen)
            { //set borderless windowed fullscreen
                NativeMethods.SetWindowLong(mainWindowHandle, WindowFlags.Style, WindowStyleFlags.Visible);
                NativeMethods.ShowWindowAsync(mainWindowHandle, ShowWindowFlags.ActiveMaximized);
            } else
            {
                //otherwise
                var clientRect = new Rect();
                var windowRect = new Rect();

                NativeMethods.GetClientRect(mainWindowHandle, ref clientRect);
                NativeMethods.GetWindowRect(mainWindowHandle, ref windowRect);

                var BorderWidth = windowRect.Width - clientRect.Width;
                var TitleHeight = windowRect.Height - clientRect.Height;

                //if it doesnt have a titlebar, set the window back to a normal state (overlappedwindow)
                if (!NativeMethods.GetWindowLong(mainWindowHandle, WindowFlags.Style).HasFlag(WindowStyleFlags.Caption))
                    NativeMethods.SetWindowLong(mainWindowHandle, WindowFlags.Style, WindowStyleFlags.OverlappedWindow);

                //set window size
                NativeMethods.MoveWindow(mainWindowHandle, windowRect.X, windowRect.Y, width + BorderWidth, height + TitleHeight, true);
            }
        }

        internal static bool InjectDll(int processId, string dir)
        {
            var dawndPath = $@"{dir}\dawnd.dll";

            if (!File.Exists(dawndPath))
                File.WriteAllBytes(dawndPath, Resources.dawnd);

            var dllName = "dawnd.dll";
            var accessHandle = NativeMethods.OpenProcess(ProcessAccessFlags.FullAccess, true, processId);

            //length of string containing the DLL file name +1 byte padding
            var nameLength = dllName.Length + 1;
            //allocate memory within the virtual address space of the target process
            var allocate = NativeMethods.VirtualAllocEx(
                accessHandle, (IntPtr) null, (IntPtr) nameLength, 0x1000, 0x40); //allocation pour WriteProcessMemory

            //write DLL file name to allocated memory in target process
            NativeMethods.WriteProcessMemory(accessHandle, allocate, dllName, (UIntPtr) nameLength, out _);
            //retreive function pointer for remote thread
            var injectionPtr = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            //if failed to retreive function pointer
            if (injectionPtr == UIntPtr.Zero)
                return false;

            //create thread in target process, and store accessHandle in hThread
            var thread = NativeMethods.CreateRemoteThread(
                accessHandle, (IntPtr) null, IntPtr.Zero, injectionPtr, allocate, 0, out _);
            //make sure thread accessHandle is valid
            if (thread == IntPtr.Zero)
                return false;
            //time-out is 10 seconds...
            var result = NativeMethods.WaitForSingleObject(thread, 10 * 1000);
            //check whether thread timed out...
            if (result != WaitEventResult.Signaled)
            {
                //thread timed out...
                //make sure thread accessHandle is valid before closing... prevents crashes.
                if (thread != IntPtr.Zero)
                    //close thread in target process
                    NativeMethods.CloseHandle(thread);
                return false;
            }

            //free up allocated space ( AllocMem )
            NativeMethods.VirtualFreeEx(accessHandle, allocate, (UIntPtr) 0, 0x8000);
            //make sure thread accessHandle is valid before closing... prevents crashes.
            if (thread != IntPtr.Zero)
                //close thread in target process
                NativeMethods.CloseHandle(thread);
            //return succeeded
            return true;
        }
    }
}
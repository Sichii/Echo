using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Echo
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern WaitEventResult WaitForSingleObject(IntPtr hObject, int timeout);

        #region Thumbnail Manipulation
        [DllImport("dwmapi.dll")]
        internal static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);
        [DllImport("dwmapi.dll")]
        internal static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref ThumbnailProperties props);
        [DllImport("dwmapi.dll")]
        internal static extern int DwmUnregisterThumbnail(IntPtr thumb);
        #endregion

        #region Thread Manipulation
        [DllImport("kernel32")]
        internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, IntPtr dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
        [DllImport("kernel32.dll")]
        internal static extern int ResumeThread(IntPtr hThread);
        #endregion

        #region Process Manipulation
        [DllImport("kernel32.dll", ExactSpelling = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool CreateProcess(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, IntPtr environment, string currentDirectory, ref StartupInfo startupInfo, out ProcessInfo processInfo);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(ProcessAccessFlags access, bool inheritHandle, int processId);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr handle);
        #endregion

        #region Memory Manipulation
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, IntPtr count, out int bytesWritten);
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, IntPtr count, out int bytesRead);
        #endregion

        #region Window Manipulation
        [DllImport("user32.dll")]
        internal static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowFlags nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowFlags nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);
        #endregion

        #region GetWindow
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        internal static extern WindowStyleFlags GetWindowLong(IntPtr hWnd, WindowFlags nIndex);
        #endregion

        #region SetWindow
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, WindowFlags nIndex, WindowStyleFlags dwNewLong);
        [DllImport("User32.dll")]
        internal static extern int SetForegroundWindow(int hWnd);
        #endregion
    }
}

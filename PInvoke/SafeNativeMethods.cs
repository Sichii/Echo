using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;


namespace DAWindower
{
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern SafeProcessHandle OpenProcess(ProcessAccessFlags dwDesiredAccess,
    [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadProcessMemory(SafeProcessHandle hProcess, IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer, UIntPtr nSize, UIntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteProcessMemory(SafeProcessHandle hProcess, IntPtr lpBaseAddress, byte[] lpBuffer,
            int nSize, UIntPtr lpNumberOfBytesRead);
    }
}

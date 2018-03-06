﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DAWindower
{
    internal static class Kernel32
    {

        [DllImport("kernel32.dll")]
        internal static extern bool CreateProcess(string applicationName, string commandLine, IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, ProcessCreationFlags creationFlags, IntPtr environment, string currentDirectory, ref StartInfo startupInfo, out ProcInfo processInfo);



        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(ProcessAccessFlags access, bool inheritHandle, int processId);
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, int count, out int bytesRead);
        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, [Out] byte[] lpBuffer, int nSize, byte lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        internal static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        internal static extern WaitEventResult WaitForSingleObject(IntPtr hObject, int timeout);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr baseAddress, IntPtr buffer, int count, out int bytesWritten);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, byte lpNumberOfBytesWritten);
        [DllImport("kernel32")]
        internal static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        internal static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);


        public static bool Peek(Process proc, int target, byte[] data)
        {
            return Kernel32.ReadProcessMemory(proc.Handle, target, data, data.Length, 0);
        }
        public static bool Poke(Process proc, int target, byte[] data)
        {
            return Kernel32.WriteProcessMemory(proc.Handle, target, data, data.Length, 0);
        }
    }
}

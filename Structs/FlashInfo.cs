using System;
using System.Runtime.InteropServices;

namespace DAWindower
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct FlashInfo
    {
        public uint Size;
        public IntPtr Handle;
        public uint Flags;
        public uint Count;
        public uint Timeout;
    }
}

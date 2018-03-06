using System;
using System.Collections.Generic;
using System.Linq;
namespace DAWindower
{
    internal struct ProcInfo
    {
        public IntPtr ProcessHandle { get; set; }
        public IntPtr ThreadHandle { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
    }
}

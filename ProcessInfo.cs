using System;
using System.Diagnostics;

namespace DAWindower
{
    internal class ProcessInfo
    {
        internal string Name { get; set; }
        internal Process Process { get; set; }
        internal IntPtr Handle { get; set; }
        internal Thumbnail Thumbnail { get; set; }
    }
}

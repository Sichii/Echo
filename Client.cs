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
            Name = "Unknown";
            MainForm = mainForm;
            ProcId = processId;
            Thumb = new Thumbnail(this);
        }
    }
}

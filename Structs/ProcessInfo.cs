﻿using System;

namespace Echo.Structs;

public struct ProcessInfo
{
    #region Do Not ReOrder

    public IntPtr ProcessHandle { get; set; }
    public IntPtr ThreadHandle { get; set; }
    public int ProcessId { get; set; }
    public int ThreadId { get; set; }

    #endregion
}
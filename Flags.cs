﻿using System;

namespace DAWindower
{
    [Flags]
    internal enum WaitEventResult : uint
    {
        Signaled = 0,
        Abandoned = 128,
        Timeout = 258,
        Failed = uint.MaxValue
    }

    [Flags]
    internal enum ProcessAccessFlags : uint
    {
        None = 0,
        Terminate = 1,
        CreateThread = 2,
        VmOperation = 8,
        VmRead = 16,
        VmWrite = 32,
        DuplicateHandle = 64,
        CreateProcess = 128,
        SetQuota = 256,
        SetInformation = 512,
        QueryInformation = 1024,
        SuspendResume = 2048,
        QueryLimitedInformation = 4096,
        FullAccess = 2035711
    }

    [Flags]
    internal enum ProcessCreationFlags
    {
        DebugProcess = 1,
        DebugOnlyThisProcess = 2,
        Suspended = 4,
        DetachedProcess = 8,
        NewConsole = 16,
        NewProcessGroup = 512,
        UnicodeEnvironment = 1024,
        SeparateWowVdm = 2048,
        SharedWowVdm = 4096,
        InheritParentAffinity = SharedWowVdm,
        ProtectedProcess = 262144,
        ExtendedStartupInfoPresent = 524288,
        BreakawayFromJob = 16777216,
        PreserveCodeAuthZLevel = 33554432,
        DefaultErrorMode = 67108864,
        NoWindow = 134217728,
    }

    [Flags]
    internal enum ThumbnailFlags : int
    {
        RectDestination = 1,
        Opacity = 4,
        Visible = 8
    }
}

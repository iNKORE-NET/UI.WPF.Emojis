//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013–2021 Sam Hocevar <sam@hocevar.net>
//
//  This library is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.Runtime.InteropServices;

namespace Stfu
{
    // Enums from winnt.h
    internal enum SID_NAME_USE : int
    {
        SidTypeUser = 1,
        SidTypeGroup,
        SidTypeDomain,
        SidTypeAlias,
        SidTypeWellKnownGroup,
        SidTypeDeletedAccount,
        SidTypeInvalid,
        SidTypeUnknown,
        SidTypeComputer,
        SidTypeLabel,
        SidTypeLogonSession,
    };

    // Structs from winuser.h
    [StructLayout(LayoutKind.Sequential)]
    struct LASTINPUTINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 cbSize;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwTime;
    }
}

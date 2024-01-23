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
    public static class User
    {
        /// <summary>
        /// Return a timespan indicating the user input idle time. Note that this
        /// value will wrap around every 25 days or so (2^31 milliseconds).
        /// </summary>
        public static TimeSpan IdleTime
        {
            get
            {
                LASTINPUTINFO lii = new LASTINPUTINFO();
                lii.cbSize = (uint)Marshal.SizeOf(lii);
                if (!NativeMethods.GetLastInputInfo(ref lii))
                    return TimeSpan.Zero;

                var ticks = Environment.TickCount - (int)lii.dwTime;
                return TimeSpan.FromMilliseconds(ticks);
            }
        }
    }
}

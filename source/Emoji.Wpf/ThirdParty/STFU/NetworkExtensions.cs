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
using System.Net;
using System.Net.Sockets;

namespace Stfu
{
    public static class NetworkExtensions
    {
        /// <summary>
        /// Return whether a given IP is an internal (RFC1918) or loopback address
        /// </summary>
        public static bool IsInternal(this IPAddress ip)
        {
            if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                throw new NotSupportedException();

            byte[] b = ip.GetAddressBytes();
            return (b[0] == 10) || (b[0] == 127)
                || (b[0] == 172 && (b[1] >= 16 && b[1] < 32))
                || (b[0] == 192 && b[1] == 168);
        }
    }
}

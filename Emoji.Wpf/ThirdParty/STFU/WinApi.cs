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

using System.Text;
using System.Runtime.InteropServices;

namespace Stfu
{

    static internal class NativeMethods
    {
        //
        // for SchTask.cs
        //

        [DllImport("advapi32", CharSet=CharSet.Auto, SetLastError = true)]
        public static extern bool LookupAccountSid(string lpSystemName,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Sid, StringBuilder lpName,
            ref uint cchName, StringBuilder ReferencedDomainName,
            ref uint cchReferencedDomainName, out SID_NAME_USE peUse);

        //
        // for UserIdle.cs
        //

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    };
}

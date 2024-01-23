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

namespace Stfu
{
    public static class Sys
    {
        /// <summary>
        /// Retrieve the name of the “Users” group. Not sure this is in the right place.
        /// </summary>
        public static string BuiltinGroupName
        {
            get
            {
                var group_name = new StringBuilder();
                var domain_name = new StringBuilder();
                group_name.EnsureCapacity(128);
                domain_name.EnsureCapacity(128);
                var group_size = (uint)group_name.Capacity;
                var domain_size = (uint)domain_name.Capacity;
                // Build SID S-1-5-32-545 (“Users” group)
                byte[] sid = new byte[]
                {
                    1, // Revision
                    2, // SubAuthorityCount
                    0, 0, 0, 0, 0, 5, // IdentifierAuthority = SECURITY_NT_AUTHORITY (5)
                    32, 0, 0, 0, // SECURITY_BUILTIN_DOMAIN_RID (32)
                    33, 2, 0, 0, // DOMAIN_ALIAS_RID_USERS (545)
                };

                if (!NativeMethods.LookupAccountSid(null, sid, group_name, ref group_size, domain_name,
                                                    ref domain_size, out SID_NAME_USE sid_use))
                    return @"BUILTIN\Users";

                return $@"{domain_name}\{group_name}";
            }
        }
    }
}

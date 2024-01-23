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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Stfu
{
    public static class TaskScheduler
    {
        /// <summary>
        /// Return whether a task with this name exists
        /// </summary>
        public static Result<bool> HasTask(string task_name)
            => RunSchTasks($"/query /tn {task_name} /xml");

        /// <summary>
        /// Install a task with the given name and command line
        /// </summary>
        public static Result<bool> InstallTask(string task_name, string command,
                                               bool elevated = false, string author = null)
        {
            var source = $@"{Environment.SystemDirectory}\Tasks\{task_name}";
            var tmp = Path.GetTempFileName();

            try
            {
                // Create a scheduled task, then edit the resulting XML with some
                // features that the command line tool does not support, and reload
                // the XML file.
                var escaped_command = command.Replace("\"", "\"\"\"");
                var ret = RunSchTasks($"/tn {task_name} /f /create /hresult /sc onlogon /tr \"{escaped_command}\"");
                if (!ret)
                    return ret;

                var doc = new FixableXmlDocument();
                doc.Load(source);
                if (elevated)
                {
                    // Make sure we use a GroupId, not a UserId; we can’t use the SYSTEM
                    // account because it is not allowed to open GUI programs. We use the
                    // built-in BUILTIN\Users group instead.
                    doc.Renames.Add("UserId", "GroupId");
                    doc.Replaces.Add("RunLevel", "HighestAvailable"); // run with higest privileges
                    doc.Replaces.Add("GroupId", Sys.BuiltinGroupName);
                    doc.Removes.Add("LogonType"); // This tag is only legal for UserId.
                }

                if (author != null)
                {
                    doc.Replaces.Add("Author", author);
                }

                doc.Fix();
                doc.Save(tmp);

                ret = RunSchTasks($"/tn {task_name} /f /create /hresult /xml \"{tmp}\"");
                if (!ret)
                    return ret;
                File.Delete(tmp);
                return true;
            }
            catch (Exception ex)
            {
                return (false, ex.ToString());
            }
        }

        private class FixableXmlDocument : XmlDocument
        {
            public void Fix()
                => FixElement(DocumentElement);

            private void FixElement(XmlElement node)
            {
                // Rename node if necessary
                if (Renames.TryGetValue(node.Name, out string new_name))
                {
                    var tmp = node.OwnerDocument.CreateElement(new_name, node.NamespaceURI);
                    foreach (XmlNode child in node.ChildNodes)
                        tmp.AppendChild(child.CloneNode(true));
                    node.ParentNode.InsertBefore(tmp, node);
                    node.ParentNode.RemoveChild(node);
                    node = tmp;
                }

                // Replace node content if necessary
                if (Replaces.TryGetValue(node.Name, out string content))
                    node.InnerText = content;

                // Recurse
                if (node.HasChildNodes && node.FirstChild is XmlElement first_child)
                    FixElement(first_child);

                // Process next sibling
                if (node.NextSibling is XmlElement sibling)
                    FixElement(sibling);

                // Remove node if necessary (make sure to do this after sibling)
                if (Removes.Contains(node.Name))
                    node.ParentNode.RemoveChild(node);
            }

            public readonly HashSet<string> Removes = new HashSet<string>();

            public readonly Dictionary<string, string> Renames = new Dictionary<string, string>();

            public readonly Dictionary<string, string> Replaces = new Dictionary<string, string>()
            {
                { "ExecutionTimeLimit", "PT0S" },          // allow to run indefinitely
                { "MultipleInstancesPolicy", "Parallel" }, // allow multiple instances
                { "DisallowStartIfOnBatteries", "false" },
                { "StopIfGoingOnBatteries", "false" },
                { "StopOnIdleEnd", "false" },
                { "StartWhenAvailable", "false" },
                { "RunOnlyIfNetworkAvailable", "false" },
            };
        }

        private static Result<bool> RunSchTasks(string args)
        {
            var pi = new ProcessStartInfo()
            {
                FileName = "schtasks.exe",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            var p = Process.Start(pi);
            p.WaitForExit();
            var exit_code = p.ExitCode;
            if (exit_code != 0)
            {
                var stdout = p.StandardOutput.ReadToEnd();
                var stderr = p.StandardOutput.ReadToEnd();
                if (string.IsNullOrEmpty(stdout) && string.IsNullOrEmpty(stderr))
                    return (false, $"Command {pi.FileName} {pi.Arguments} returned code 0x{exit_code:X8}");
                return (false, stdout + stderr);
            }
            return true;
        }
    }
}

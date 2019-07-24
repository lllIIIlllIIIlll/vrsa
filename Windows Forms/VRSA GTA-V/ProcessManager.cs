﻿using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace VRSA_GTA_V
{
    public static class ProcessManager
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        public static bool SuspendProcess(string Name)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(Name).ToList();
            if (processes.Count > 0)
            {
                var process = System.Diagnostics.Process.GetProcessById(processes[0].Id);

                if (process.ProcessName == string.Empty)
                    return false;

                foreach (ProcessThread pT in process.Threads)
                {
                    IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                    if (pOpenThread == IntPtr.Zero)
                    {
                        continue;
                    }

                    SuspendThread(pOpenThread);

                    CloseHandle(pOpenThread);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ResumeProcess(string Name)
        {
            var processes = System.Diagnostics.Process.GetProcessesByName(Name).ToList();
            if (processes.Count > 0)
            {

                var process = System.Diagnostics.Process.GetProcessById(processes[0].Id);

                if (process.ProcessName == string.Empty)
                    return false;

                foreach (ProcessThread pT in process.Threads)
                {
                    IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                    if (pOpenThread == IntPtr.Zero)
                    {
                        continue;
                    }

                    var suspendCount = 0;
                    do
                    {
                        suspendCount = ResumeThread(pOpenThread);
                    }
                    while (suspendCount > 0);

                    CloseHandle(pOpenThread);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

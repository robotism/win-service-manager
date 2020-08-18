using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ServiceManagament
{
    class Utils
    {

        public static string GetExeFullPath(string fileName)
        {
            if (File.Exists(fileName) || Directory.Exists(fileName))
            {
                return Path.GetFullPath(fileName);
            }

            string values = Environment.GetEnvironmentVariable("PATH");
            foreach (string path in values.Split(Path.PathSeparator))
            {
                string fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }

        public static IntPtr GetWindowHandleByExecPath(string exec)
        {
            Process[] processes = GetProcessByExecPath(exec);
            if (processes.Length == 0)
            {
                return IntPtr.Zero;
            }
            IntPtr hWnd = IntPtr.Zero;
            do
            {
                hWnd = FindWindowEx(IntPtr.Zero, hWnd, null, null);
                int pid;
                GetWindowThreadProcessId(hWnd, out pid);
                foreach (Process process in processes)
                {
                    if (process.Id == pid)
                    {
                        return hWnd;
                    }
                }
            } while (hWnd != IntPtr.Zero);

            return IntPtr.Zero;
        }

        public static bool SetAutoRun(bool AddOrCancel)
        {
            string exec = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string name = Path.GetFileNameWithoutExtension(exec);
            return SetAutoRun(name, exec, AddOrCancel);
        }

        public static bool SetAutoRun(string keyName, string filePath, bool AddOrCancel)
        {
            try
            {
                RegistryKey Local = Registry.LocalMachine;
                RegistryKey runKey = Local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\");
                if (AddOrCancel)
                {
                    runKey.SetValue(keyName, filePath);
                    Local.Close();
                }
                else
                {
                    if (runKey != null)
                    {
                        runKey.DeleteValue(keyName, false);
                        Local.Close();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        public static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表  
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程  
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;

                    }
                }
            }
            return null;
        }

        public static string GetProcessPath(Process process)
        {
            string filename = null;
            try
            {
                if (process != null)
                {
                    filename = process.MainModule.FileName;
                }
            }
            catch (Exception)
            {

            }
            return filename != null ? filename : "";
        }

        public static string GetProcessWorkingDir(Process process)
        {
            string path = ProcessUtilities.GetCurrentDirectory(process);
            if (path == null)
            {
                return null;
            }
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }

        public static Process[] GetProcessByExecPath(string exec)
        {
            List<Process> list = new List<Process>();
            string name = Path.GetFileNameWithoutExtension(exec);
            Process[] processes = Process.GetProcessesByName(name);
            foreach (Process process in processes)
            {
                if (exec.Equals(GetProcessPath(process)))
                {
                    list.Add(process);
                }
            }
            return list.ToArray();
        }


        public static void ShowWindow(IntPtr hWnd)
        {
            if (IntPtr.Zero != hWnd)
            {
                ShowWindowAsync(hWnd, SW_NORMAL);
                SetForegroundWindow(hWnd);
            }
        }

        public static void HideWindow(IntPtr hWnd)
        {
            if (IntPtr.Zero != hWnd)
            {
                ShowWindowAsync(hWnd, SW_HIDE);
            }
        }


        public static IntPtr FindWindow(string title)
        {
            return FindWindow(null, title);
        }

        public static IntPtr FindWindowEx(IntPtr hwndChildAfter, string title)
        {
            return FindWindowEx(IntPtr.Zero, hwndChildAfter, null, title);
        }

        #region  Win API


        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string IpClassName, string IpWindowName);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);


        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_SHOW = 5;
        private const int SW_MINIMIZE = 6;
        private const int SW_SHOWMINNOACTIVE = 7;
        private const int SW_SHOWNA = 8;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;
        private const int SW_FORCEMINIMIZE = 11;
        private const int SW_MAX = 11;

        #endregion
    }
}

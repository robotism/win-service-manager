using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ServiceManagament
{
    public class Service
    {
        public string Name { get; set; }
        public string WorkingDerictory { get; set; }
        public string Command { get; set; }
        public string Args { get; set; }
        public string Link { get; set; }

        public long AutoRestart { get; set; }

        public bool ShowWindow { get; set; }


    }
    public class ServiceManager
    {
        public static ServiceManager SINGLETON = new ServiceManager();

        private readonly IniFile iniFile = new IniFile(MainForm.TITLE + ".ini");

        private List<Service> services;

        private ServiceManager()
        {

        }
        public List<Service> GetServivces()
        {
            if (this.services != null)
            {
                return this.services;
            }
            List<Service> list = new List<Service>();


            long autoRestart = iniFile.GetLong("config", "auto-restart", 0);

            List<string> sections = iniFile.getAllSections();

            foreach (string section in sections)
            {
                if (!section.StartsWith("service."))
                {
                    continue;
                }
                string name = section.Substring("service.".Length);
                string path = iniFile.GetString(section, "path", "");
                string exec = iniFile.GetString(section, "exec", "");
                string args = iniFile.GetString(section, "args", "");
                string link = iniFile.GetString(section, "link", "");
                bool showWindow = "true".Equals(iniFile.GetString(section, "show-window", "false").ToLower());

                if (path == null || path.Length == 0)
                {
                    continue;
                }

                string workdir;

                try
                {
                    workdir = Path.GetFullPath(path);
                    if (workdir.EndsWith("" + Path.DirectorySeparatorChar))
                    {
                        workdir = workdir.Substring(0, workdir.Length - 1);
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                if (exec == null || exec.Length == 0)
                {
                    continue;
                }
                string exe1 = Path.GetFullPath(workdir + Path.DirectorySeparatorChar + exec);
                string exe2 = Utils.GetExeFullPath(exec);

                if (File.Exists(exe1))
                {
                    exec = exe1;
                }
                else if (File.Exists(exe2))
                {
                    exec = exe2;
                }
                else
                {
                    continue;
                }

                Service service = new Service();
                service.Name = name;
                service.WorkingDerictory = workdir;

                service.Command = exec;
                service.Args = args;
                service.Link = link;
                service.AutoRestart = autoRestart;
                service.ShowWindow = showWindow;

                try
                {
                    if (!Directory.Exists(service.WorkingDerictory))
                    {
                        continue;
                    }

                    if (!File.Exists(service.Command))
                    {
                        continue;
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                try
                {
                    Process process = GetProcess(service);
                    if (process != null)
                    {
                        if (service.ShowWindow)
                        {
                            Utils.ShowWindow(process.MainWindowHandle);
                        }
                        else
                        {
                            Utils.HideWindow(process.MainWindowHandle);
                        }
                    }
                }
                catch (Exception)
                {

                }


                list.Add(service);
            }
            return this.services = list;
        }


        public bool SetShowWindow(string serviceName, bool b)
        {
            return iniFile.SetString("service." + serviceName, "show-window", b ? "true" : "false");
        }

        private bool IsChildPath(string child, string parent)
        {
            if (child.Length < parent.Length)
            {
                return false;
            }
            if (child.CompareTo(parent) == 0)
            {
                return true;
            }
            return child.StartsWith(parent)
             && child.Substring(parent.Length).StartsWith("" + Path.DirectorySeparatorChar);
        }

        public Process GetProcess(Service service)
        {
            Process[] processes = Utils.GetProcessByExecPath(service.Command);
            foreach (Process process in processes)
            {
                if (IsChildPath(service.Command, service.WorkingDerictory))
                {
                    return process;
                }
                string w = Utils.GetProcessWorkingDir(process);
                if (w == null)
                {
                    continue;
                }
                if (IsChildPath(w, service.WorkingDerictory))
                {
                    return process;
                }
            }
            return null;
        }
        public bool ExistProcess(Service service)
        {
            Process process = GetProcess(service);
            return process != null && !process.HasExited;
        }

        public void StartProcess(object sender, EventArgs e)
        {
            Button button = sender as Button;
            ServiceControl serviceControl = button.Parent as ServiceControl;
            Service service = serviceControl.GetService();


            Process process = new Process();
            process.StartInfo.FileName = service.Command;
            process.StartInfo.WorkingDirectory = service.WorkingDerictory;
            process.StartInfo.Arguments = service.Args;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "runas";
            /*
            if (!service.ShowWindow)
            {
                process.StartInfo.UseShellExecute = false;  // 将此属性设置为 false 可重定向输入、输出和错误流
                process.StartInfo.CreateNoWindow = true;
            }
            */

            process.EnableRaisingEvents = true;

            try
            {
                process.Start();
            }
            catch (Exception)
            {
                serviceControl.Invoke(new ServiceControl.SetStateDelegate(serviceControl.SetStoped));
                return;
            }

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                if (process.HasExited)
                {
                    serviceControl.Invoke(new ServiceControl.SetStateDelegate(serviceControl.SetStoped));
                    return;
                }
                Thread.Sleep(10);
            }

            if (!service.ShowWindow)
            {
                serviceControl.ShowWindow(false);
            }
            serviceControl.Invoke(new ServiceControl.SetStateDelegate(serviceControl.SetStarted));

        }

        public void StopProcess(object sender, EventArgs e)
        {
            Button button = sender as Button;
            ServiceControl serviceControl = button.Parent as ServiceControl;
            Service service = serviceControl.GetService();

            Process process = GetProcess(service);
            if (process != null)
            {
                process.Kill();
            }
        }

        public void ExitAll()
        {
            foreach (Service service in this.services)
            {
                Process process = GetProcess(service);
                if (process != null)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

    }
}

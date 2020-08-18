using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ServiceManagament
{
    public partial class ServiceControl : UserControl
    {
        private readonly Service service;
        private readonly ServiceManager manager = ServiceManager.SINGLETON;

        public delegate void SetStateDelegate();

        private Mutex mutex = new Mutex();
        private double countDown = 0;

        public ServiceControl(Service service)
        {
            InitializeComponent();
            this.service = service;
            this.labelServiceName.Text = service.Name.Substring(0, 1).ToUpper() + service.Name.Substring(1);
            this.textBoxDesc.Text = service.Command + " " + service.Args;
            this.linkLabel.Text = service.Link;
            this.linkLabel.Visible = service.Link.Length > 0 ? true : false;
        }

        private void ServiceControl_Load(object sender, EventArgs e)
        {
            this.checkBoxShowWindow.Checked = service.ShowWindow;

            if (manager.ExistProcess(this.service))
            {
                SetStarted();
            }
            else
            {
                SetStoped();
            }

        }


        private void ButtonController_Click(object sender, EventArgs e)
        {
            this.Toggle(sender, e);
        }


        public void Toggle(object sender, EventArgs e)
        {
            if (sender != this.buttonController)
            {
                sender = this.buttonController;
            }
            if (mutex.WaitOne())
            {
                this.buttonController.Enabled = false;
                if (!manager.ExistProcess(this.service))
                {
                    manager.StartProcess(sender, e);
                }
                else
                {
                    manager.StopProcess(sender, e);
                }
            }
        }

        public void SetStarted()
        {
            this.labelServiceName.ForeColor = Color.Green;
            this.buttonController.ForeColor = Color.Red;
            this.buttonController.Text = "Stop";
            this.buttonController.Enabled = true;
        }

        public void SetStoped()
        {
            this.labelServiceName.ForeColor = Color.Red;
            this.buttonController.ForeColor = Color.Green;
            this.buttonController.Text = "Start";
            this.buttonController.Enabled = true;
        }


        public Service GetService()
        {
            return this.service;
        }


        private void TimerCountDown_Tick(object sender, EventArgs e)
        {
            if (manager.ExistProcess(this.service))
            {
                this.countDown = -1;
                this.labelCountDown.Text = "";
                this.linkLabel.Visible = true;
                SetStarted();
                return;
            }
            else
            {
                SetStoped();
            }

            if (service.AutoRestart <= 0)
            {
                return;
            }

            if (this.countDown < 0)
            {
                this.countDown = service.AutoRestart;
            }

            double interval = timerCountDown.Interval;
            double down = this.countDown / interval;
            this.countDown -= interval;

            if (down > 0)
            {
                this.linkLabel.Visible = false;
                this.labelCountDown.Text = "restart after " + down + "s";
            }
            else if (down <= 0)
            {
                this.Toggle(sender, e);
            }
        }

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((sender as LinkLabel).Text);
        }

        private void CheckBoxShowWindow_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (!checkBox.Capture)
            {
                return;
            }

            if (!manager.SetShowWindow(service.Name, checkBox.Checked))
            {
                checkBox.Checked = this.service.ShowWindow;
                return;
            }

            ShowWindow(this.service.ShowWindow = checkBox.Checked);
        }

        public void ShowWindow(bool b)
        {

            IntPtr hWnd = FindWindow();
            if (b)
            {
                Utils.ShowWindow(hWnd);
            }
            else
            {
                Utils.HideWindow(hWnd);
            }
        }

        private IntPtr FindWindow()
        {
            Process process = manager.GetProcess(service);
            if (process == null)
            {
                return IntPtr.Zero;
            }

            IntPtr hWnd;
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                hWnd = process.MainWindowHandle;
            }
            else
            {
                hWnd = Utils.GetWindowHandleByExecPath(service.Command);
            }

            while (IntPtr.Zero != hWnd)
            {
                int pid;
                //获取进程ID   
                Utils.GetWindowThreadProcessId(hWnd, out pid);

                if (process.Id == pid)
                {
                    return hWnd;
                }
                else
                {
                    hWnd = Utils.FindWindowEx(hWnd, service.Command);
                }
            }
            return IntPtr.Zero;
        }
    }

}

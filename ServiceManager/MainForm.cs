using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace ServiceManagament
{
    public partial class MainForm : Form
    {
        private readonly List<Service> services = ServiceManager.SINGLETON.GetServivces();

        public static string TITLE = "ServiceManager";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Utils.SetAutoRun(true);

            /*
            int x = SystemInformation.WorkingArea.Width - this.Size.Width;
            int y = SystemInformation.WorkingArea.Height - this.Size.Height;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);
            */

            this.flowLayoutPanel.SuspendLayout();//先挂起布局逻辑
            foreach (Service service in this.services)
            {
                this.flowLayoutPanel.Controls.Add(new ServiceControl(service));//添加布局
            }
            this.flowLayoutPanel.ResumeLayout(false);//恢复布局逻辑
            this.flowLayoutPanel.AutoScroll = true;//这步很重要，在子控件比较多的时候必须调用这个方法才会出现滚动条

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void NotifyIconContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            this.notifyIconContextMenuStrip.Items.Clear();

            ToolStripMenuItem item_exit = new ToolStripMenuItem();
            item_exit.Name = "Exit";
            item_exit.Text = "Exit";
            item_exit.Click += new EventHandler(this.Exit);
            this.notifyIconContextMenuStrip.Items.Add(item_exit);
        }


        private void Exit(object sender, EventArgs e)
        {
            ServiceManager.SINGLETON.ExitAll();
            this.notifyIcon.Dispose();
            this.Close();
            this.Dispose();
            Application.Exit();
        }

        private void NotifyIconContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.notifyIconContextMenuStrip.Items.Clear();
        }

    }
}

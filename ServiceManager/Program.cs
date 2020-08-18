using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ServiceManagament
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process instance = Utils.RunningInstance();
            if (instance == null)
            {
                //没有实例在运行
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                return;
            }

            Utils.ShowWindow(Utils.FindWindow(MainForm.TITLE));
        }
    }
}

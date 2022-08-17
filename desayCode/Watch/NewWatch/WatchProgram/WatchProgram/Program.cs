using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace WatchProgram
{
    static class Program
    {
        public static Mutex mutex = new Mutex(true,"WatchProgram");
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(1000))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("测试程序已经启动");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Threading;
using System.Management;

namespace PTMSShell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer _queryTimer;
        public MainWindow()
        {
            InitializeComponent();
        }
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            //获得当前工作进程
            Process proc = Process.GetCurrentProcess();
            long usedMemory = proc.PrivateMemorySize64;
            if (usedMemory > 1024 * 1024 * 1000)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                    //KillProcessAndChildren(proc.Id);
                }
            }
        }

        public static void KillProcessAndChildren(int pid)
        {

            Process pro = Process.GetProcessById(pid);
            if (null != pro)
            {
              
                foreach (ProcessThread thread in pro.Threads)
                {
                    Console.WriteLine(String.Format("ID:{0} StartTime:{1} ThreadState{2}", thread.Id, thread.StartTime, thread.ThreadState));
                }
            }


            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            if (moc.Count > 0)
            {
                foreach (ManagementObject mo in moc)
                {
                    KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
                }
                try
                {
                    Process proc = Process.GetProcessById(pid);
                    Console.WriteLine(pid);
                    proc.Kill();
                }
                catch (ArgumentException)
                {
                    /* process already exited */
                }
            }

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            this.ptms.ObjectForScripting = new Shell();
            string Weburl = ConfigurationManager.AppSettings["PTMSWEB"];
            this.ptms.Navigate(Weburl);
            _queryTimer = new DispatcherTimer();
            _queryTimer.Interval = TimeSpan.FromSeconds(1);
            _queryTimer.Tick += _queryTimer_Tick;
            _queryTimer.Start();

        }
        public void _queryTimer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
            ClearMemory();
        }

    }
    


    [System.Runtime.InteropServices.ComVisible(true)]
    public class Shell
    {
      
        public void CloseShell()
        {
            Application.Current.MainWindow.Close();

        }
    } 
}

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

namespace PTMSShell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

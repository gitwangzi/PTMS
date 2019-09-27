using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.Utilities;

namespace Gsafety.PTMS.Manager
{
    public partial class CompxControl : UserControl
    {
        private ObservableCollection<Employee> _data;

        public ObservableCollection<Employee> EmployeeData
        {
            get { return _data; }
            set { _data = value; }
        }


        public Action<string> SaveDataCallBack;

        public CompxControl()
        {       
            InitializeComponent();
            EmployeeData = new ObservableCollection<Employee>();
            this.LayoutRoot.DataContext  = EmployeeData;
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            
            if (SaveDataCallBack != null)
            {
                var data = JsonHelper.ToJsonString(EmployeeData);
                SaveDataCallBack(data);
            }
        }
    }


    public class Employee
    {
        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }
    }
}

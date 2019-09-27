using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class MonitorNoteEdit : ChildWindow
    {
        public MonitorNoteEdit()
        {
            InitializeComponent();
            this.DataContext = this;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private RunMonitorGroup _EditRunMonitorGroup;
        public RunMonitorGroup EditRunMonitorGroup
        {
            get
            {
                return _EditRunMonitorGroup;
            }
            set
            {
                _EditRunMonitorGroup = value;
            }
        }

        public void Edit(RunMonitorGroup runMonitorGroup)
        {
            EditRunMonitorGroup = runMonitorGroup;
            
            Show();
        }    
       

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditRunMonitorGroup.Note == "")
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("AlarmNoteNotBull"), MessageDialogButton.Ok);
                return;
            }
            
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,
System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}


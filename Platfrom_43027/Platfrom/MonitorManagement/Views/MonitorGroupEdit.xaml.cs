using Gsafety.Ant.Monitor.ViewModels;
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
    public partial class MonitorGroupEdit : ChildWindow
    {

        MonitorGroupEditViewModel viewModel;
        public MonitorGroupEdit()
        {
            InitializeComponent();
            this.viewModel = new MonitorGroupEditViewModel();
            this.DataContext = viewModel;
            this.viewModel.Name = string.Empty;
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
            this.viewModel.Name = runMonitorGroup.GroupName;
            Show();
        }

        private bool Duplicate(string value)
        {
            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
            {                
                if ((item.ID!=EditRunMonitorGroup.ID)&&(item.GroupName==EditRunMonitorGroup.GroupName)) return true;
            }
            return false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditRunMonitorGroup.GroupName == "")
            {
               // MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("Monitor_GroupNameCanNotEmpty"), MessageDialogButton.Ok);
                return;
            }
            if (Duplicate(EditRunMonitorGroup.GroupName))
            {
              //  MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("Monitor_GroupNameDuplication"), MessageDialogButton.Ok);
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

        private void groupName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.groupName.Text;
            EditRunMonitorGroup.GroupName = this.groupName.Text;
        }
    }
}


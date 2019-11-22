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
using Gsafety.Ant.Monitor.ViewModels;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class MonitorNoteEdit : ChildWindow
    {

        MonitorNoteEditViewModel viewModel;
        public MonitorNoteEdit()
        {
            InitializeComponent();
            this.viewModel = new MonitorNoteEditViewModel();        
            this.DataContext = this.viewModel;
            this.viewModel.Note = string.Empty;
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
            this.viewModel.Note = runMonitorGroup.Note;
            
            Show();
        }    
       

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditRunMonitorGroup.Note == "")
            {
               // MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("AlarmNoteNotBull"), MessageDialogButton.Ok);
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

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Note = this.AlarmNote.Text;
            EditRunMonitorGroup.Note = this.AlarmNote.Text;
        }
    }
}


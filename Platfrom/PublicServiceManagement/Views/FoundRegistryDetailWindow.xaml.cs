using Gsafety.Common.Controls;
using Gsafety.PTMS.PublicServiceManagement.Views.ViewModels;
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

namespace PublicServiceManagement.Views
{
    public partial class FoundRegistryDetailWindow : ChildWindow
    {
        FoundRegistryDetailViewModel viewModel;
        public FoundRegistryDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new FoundRegistryDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;

        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


        void viewModel_OnSaveResult(object sender, Gsafety.Common.CommMessage.SaveResultArgs e)
        {
            if (e.Result)
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
                //messagebox
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    var datePicker = sender as DatePicker;
            //    if (datePicker != null && e.AddedItems.Count == 1 && e.RemovedItems.Count == 1)
            //    {
            //        var oldValue = (DateTime)e.RemovedItems[0];
            //        var newValue = (DateTime)e.AddedItems[0];
            //        if (oldValue != newValue && oldValue.TimeOfDay.TotalSeconds != 0)
            //        {
            //            datePicker.SelectedDate = datePicker.SelectedDate.Value.Date.Add(oldValue.TimeOfDay);
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //}
        }

    }
}

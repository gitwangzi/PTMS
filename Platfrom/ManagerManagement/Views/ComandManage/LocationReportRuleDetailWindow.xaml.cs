using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager.ViewModels;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    public partial class LocationReportRuleDetailWindow : ChildWindow
    {
        private readonly LocationReportRuleDetailViewModel viewModel;
        public LocationReportRuleDetailWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new LocationReportRuleDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            comboStrategy.DropDownOpened += PopupHandler.OnDropDown;
            comboStrategy.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void viewModel_OnSaveResult(object sender, SaveResultArgs e)
        {
            if (e.Result)
            {
                DialogResult = true;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
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

        private void comboStrategy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object s = comboStrategy.SelectedValue;
            if (s.ToString() == "0")
            {
                Interval_Block.Visibility = Visibility.Visible;
                txtInterval.Visibility = Visibility.Visible;

                Length_Block.Visibility = Visibility.Collapsed;
                txtLength.Visibility = Visibility.Collapsed;
            }
            if (s.ToString() == "1")
            {
                Interval_Block.Visibility = Visibility.Collapsed;
                txtInterval.Visibility = Visibility.Collapsed;

                Length_Block.Visibility = Visibility.Visible;
                txtLength.Visibility = Visibility.Visible;
                Length_Block.SetValue(Grid.RowProperty, 5);
                txtLength.SetValue(Grid.RowProperty, 5);
            }
            if (s.ToString() == "2")
            {
                Interval_Block.Visibility = Visibility.Visible;
                txtInterval.Visibility = Visibility.Visible;

                Length_Block.Visibility = Visibility.Visible;
                txtLength.Visibility = Visibility.Visible;
                Length_Block.SetValue(Grid.RowProperty, 6);
                txtLength.SetValue(Grid.RowProperty, 6);
            }
           
        }
    }
}


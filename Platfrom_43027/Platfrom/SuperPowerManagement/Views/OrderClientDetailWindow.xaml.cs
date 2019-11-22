using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using SuperPowerManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SuperPowerManagement.Views
{
    public partial class OrderClientDetailWindow : ChildWindow
    {
        private OrderClientDetailViewModel _viewModel;

        public OrderClientDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                InitializeComponent();
                this._viewModel = new OrderClientDetailViewModel();
                _viewModel.OnSaveResult += _viewModel_OnSaveResult;
                this.DataContext = _viewModel;
                this._viewModel.ActivateView(viewName, viewParameters);
                this.MouseRightButtonDown += OrderClientDetailWindow_MouseRightButtonDown;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OrderClientDetailWindow()", ex);
            }

        }

        void OrderClientDetailWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void _viewModel_OnSaveResult(object sender, Gsafety.Common.CommMessage.SaveResultArgs e)
        {
            if (e.Result)
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
            }
        }

        public OrderClientDetailViewModel ViewModel { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}


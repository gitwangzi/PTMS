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
    public partial class HeartbeatRuleDetailWindow : ChildWindow
    {
        HeartbeatRuleDetailViewModel viewModel = null;
        public HeartbeatRuleDetailWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new HeartbeatRuleDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
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
    }
}


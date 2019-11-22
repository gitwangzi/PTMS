using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation.ViewModels;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Installation.Views
{
    public partial class MaintainRecordUnfinishedDetailWindow : ChildWindow
    {
        private readonly MaintainRecordUnfinishedDetailViewModel viewModel;
        public MaintainRecordUnfinishedDetailWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new MaintainRecordUnfinishedDetailViewModel();
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            string action = viewParameters["action"].ToString();
            if(action=="view")
            Back.SetValue(Grid.ColumnProperty, 2);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

    }
}


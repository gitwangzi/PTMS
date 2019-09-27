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
    public partial class MaintainRecordDetailWindow : ChildWindow
	{
		private readonly MaintainRecordDetailViewModel viewModel;
        public MaintainRecordDetailWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
		{
			InitializeComponent();
			this.viewModel = new MaintainRecordDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
         
			this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            this.viewModel.ActivateView(viewName, viewParameters);
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


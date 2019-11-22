using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ManagerManagement.ViewModels;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ConfigurationManage
{
    public partial class VehicleTypeDetailWindow : ChildWindow
    {
        private readonly VehicleTypeDetailViewModel viewModel;
        public VehicleTypeDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ColorListDataGrid);
            this.viewModel = new VehicleTypeDetailViewModel();
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

        private void UserName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.UserName.Text;
        }
    }
}


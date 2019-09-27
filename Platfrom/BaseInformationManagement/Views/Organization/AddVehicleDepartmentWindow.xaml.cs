using Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    public partial class AddVehicleDepartmentWindow : ChildWindow
    {
        private readonly AddVehicleDepartmentViewModel viewModel;
        public AddVehicleDepartmentWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new AddVehicleDepartmentViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += AddVehicleDepartmentWindow_MouseRightButtonDown;
        }

        void AddVehicleDepartmentWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void viewModel_OnSaveResult(object sender, Common.CommMessage.SaveResultArgs e)
        {
            if(e.Result)
            {
                DialogResult = true;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Message));
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.orgName.Text;
        }
    }
}


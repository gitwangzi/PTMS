using Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    public partial class AddVehicleDetailWindow : ChildWindow
    {
        private readonly AddVehicleInfoFromDepartmentViewModel viewModel;
        public AddVehicleDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                InitializeComponent();
                viewModel = new AddVehicleInfoFromDepartmentViewModel();
                viewModel.OnSaveResult += viewModel_OnSaveResult;
                this.DataContext = viewModel;
                viewModel.ActivateView(viewName, viewParameters);
                this.MouseRightButtonDown += AddVehicleDepartmentWindow_MouseRightButtonDown;
                comboStatus.DropDownOpened += PopupHandler.OnDropDown;
                comboStatus.DropDownClosed += PopupHandler.OnDropDown;

                comboStatus2.DropDownOpened += PopupHandler.OnDropDown;
                comboStatus2.DropDownClosed += PopupHandler.OnDropDown;

                comboStatus3.DropDownOpened += PopupHandler.OnDropDown;
                comboStatus3.DropDownClosed += PopupHandler.OnDropDown;
            }
            catch (System.Exception)
            {
                
            }
        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void AddVehicleDepartmentWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void viewModel_OnSaveResult(object sender, SaveResultArgs e)
        {
            if(e.Result)
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

        private void VehicleID_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.VehicleId = this.VehicleID.Text;
        }

        private void VehicleSn_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.VehicleSn = this.VehicleSn.Text;
        }

        private void Ficha_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Ficha = this.Ficha.Text;
        }

        private void EndineId_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.EngineId = this.EndineId.Text;
        }

        private void BrandModel_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.BrandModel = this.BrandModel.Text;

        }

        private void StartYear_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.StartYear = this.StartYear.Text;
        }

        private void Region_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Region = this.Region.Text;
        }

        private void OperationLicense_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.OperationLicense = this.OperationLicense.Text;
        }

        private void Owner_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Owner = this.Owner.Text;
        }

        private void Contact_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Contact = this.Contact.Text;
        }

        private void ContactPhone_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ContactPhone = this.ContactPhone.Text;
        }

        private void ContactEmail_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ContactEmail = this.ContactEmail.Text;
        }

        private void ContactAddress_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ContactAddress = this.ContactAddress.Text;
        }
    }
}


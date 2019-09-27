using Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.Installation.Views
{
    public partial class VehicleDetailWindow : ChildWindow
    {
        private readonly AddVehicleInfoFromDepartmentViewModel viewModel;
        public VehicleDetailWindow(string viewName, IDictionary<string, object> viewParameters)
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

            comboStatus4.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus4.DropDownClosed += PopupHandler.OnDropDown;

            comboStatus5.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus5.DropDownClosed += PopupHandler.OnDropDown;
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
    }
}


using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    public partial class AntProductUserDetailWindow : ChildWindow
    {
        private readonly AntProductUserDetailViewModel viewModel;
        public AntProductUserDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new AntProductUserDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += AntProductUserDetailWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;

        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void AntProductUserDetailWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
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

        private void Account_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Account = this.Account.Text;
        }

        private void UserName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.UserName = this.UserName.Text;
        }

        private void Password_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            this.viewModel.FirstPassword = this.FirstPassword.Password;
        }

        private void SecondPassword_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            this.viewModel.SecondPassword = this.SecondPassword.Password;
        }

        private void Phone_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Phone = this.Phone.Text;
        }

        private void Mobile_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Mobile = this.Mobile.Text;
        }

        private void Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Email = this.Email.Text;
        }

        private void Address_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Address = this.Address.Text;
        }

        private void Note_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Description = this.Description.Text;
        }
    }
}


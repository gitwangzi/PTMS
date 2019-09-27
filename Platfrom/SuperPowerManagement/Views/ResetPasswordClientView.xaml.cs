using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.OrderClientService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SuperPowerManagement.Views
{
    public partial class ResetPasswordClientView : ChildWindow
    {
        PasswordInfo PasswordInfo;

        private string _userID;
        OrderClientServiceClient client = null;
        private string username;

        public ResetPasswordClientView(string ID, string userName)
        {
            InitializeComponent();
            _userID = ID;
            username = userName;
            PasswordInfo = new PasswordInfo(userName);
            this.DataContext = PasswordInfo;

            client = ServiceClientFactory.Create<OrderClientServiceClient>();
            client.ResetPasswordCompleted += client_ResetPasswordCompleted;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }



        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LogManager log = new LogManager();
            log.ManagerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.Manager = ApplicationContext.Instance.AuthenticationInfo.Account;
            log.Content = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ChangePassword");
            log.ClientID = _userID;
            log.ClientName = username;
            log.ID = Guid.NewGuid().ToString();
            client.ResetPasswordAsync(_userID, MD5.GetMd5String(this.PasswordInfo.NewPassword), log);

        }

        private void client_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {
            if (e.Result.Result)
            {
                this.DialogResult = true;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError), MessageDialogButton.OkAndCancel);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (IsValidation())
            {
                OKButton.IsEnabled = true;
            }
            else
            {
                OKButton.IsEnabled = false;
            }

        }

        private bool IsValidation()
        {
            if (Validation.GetErrors(newPasswordBox).Count > 0)
            {
                return false;
            }

            if (Validation.GetErrors(confirmPasswordBox).Count > 0)
            {
                return false;
            }

            return true;
        }

        private void confirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string newPassword = newPasswordBox.Password;
            string conpwd = confirmPasswordBox.Password;
            if (conpwd.Equals(newPassword))
            {
                PasswordInfo.ClearError();
            }
        }
    }
}

using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.AccountService;
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

namespace Gsafety.PTMS.Manager.Views.Organization
{
    public partial class ResetPasswordView : ChildWindow
    {
        PasswordInfo _PasswordInfo;
        UserServiceClient _AdClient;
        private string _userID;
        private string _account;

        public ResetPasswordView(string userName, string userID, string account)
        {
            _AdClient = ServiceClientFactory.Create<UserServiceClient>();

            _AdClient.ModifyPasswordCompleted += _AdClient_ModifyPasswordCompleted;
            _PasswordInfo = new PasswordInfo(userName);
            this.DataContext = _PasswordInfo;
            _userID = userID;
            _account = account;
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        void _AdClient_ModifyPasswordCompleted(object sender, ModifyPasswordCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Result)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("ResetPwdSuccess"), MessageDialogButton.Ok);

                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail"), ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail"));

                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            LogOperate log = new LogOperate();
            log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            log.ID = Guid.NewGuid().ToString();
            log.OperateTime = DateTime.Now.ToUniversalTime();
            log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
            log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("ResertPwd") + ":" + _account;
            _AdClient.ModifyPasswordAsync(_userID, MD5.GetMd5String(this._PasswordInfo.NewPassword), log);
            this.DialogResult = true;
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

            string newpwd = newPasswordBox.Password;
            string conpwd = confirmPasswordBox.Password;
            if (conpwd.Equals(newpwd))
            {
                _PasswordInfo.ClearError();
            }
        }
    }
}


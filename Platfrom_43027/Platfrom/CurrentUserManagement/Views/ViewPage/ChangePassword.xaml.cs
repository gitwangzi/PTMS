/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fe1ff103-5ba9-40ef-99ba-07430d0eb7b5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
/////    Project Description:    
/////             Class Name: ChangePassword
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 14:10:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 14:10:34
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using System.Windows.Navigation;
using Gsafety.PTMS.CurrentUserManagement.ViewModels;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.Share;
using Gsafety.Common.Utilities;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.CurrentUserManagement.Views.ViewPage
{
    public partial class ChangePassword : Page
    {
        #region Fields

        PasswordInfo _PasswordInfo;
        ADAccountServiceClient _AdClient;

        #endregion

        #region Attributes

        #endregion
        public ChangePassword()
        {
            InitializeComponent();
            this.Loaded += ChangePassword_Loaded;
            _AdClient = ServiceClientFactory.Create<ADAccountServiceClient>();
            _AdClient.ValidateUserCompleted += _AdClient_ValidateUserCompleted;
            _AdClient.ResetPasswordCompleted += AdClient_ResetPasswordCompleted;
            _PasswordInfo = new PasswordInfo();
            this.DataContext = _PasswordInfo;
            IsValidation();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        void ChangePassword_Loaded(object sender, RoutedEventArgs e)
        {
            this.txt_userName.Text = ApplicationContext.Instance.AuthenticationInfo.UserName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }


        void _AdClient_ValidateUserCompleted(object sender, ValidateUserCompletedEventArgs e)
        {
            if (e.Error != null)
            {

            }
            if (e.Result != null)
            {
                if (e.Result.Result != null)
                {
                    _AdClient.ResetPasswordAsync(_PasswordInfo.UserName, _PasswordInfo.NewPassword);
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserPwdError"));
                }
            }
        }




        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            _AdClient.ValidateUserAsync(_PasswordInfo.UserName, MD5.GetMd5String(_PasswordInfo.CurrentPassword));

        }

        void AdClient_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBoxHelper.ShowDialog("", "", MessageDialogButton.Ok);

            }
            if (e.Result != null)
            {
                if (e.Result.Result)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("UpdateSuccess"), "", MessageDialogButton.Ok);

                }

            }

            _PasswordInfo = new PasswordInfo();
            this.DataContext = _PasswordInfo;
            IsValidation();
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
            if (Validation.GetErrors(currentPasswordBox).Count > 0)
            {
                return false;
            }

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
            if (conpwd != string.Empty && conpwd.Equals(newpwd))
            {
                _PasswordInfo.ClearError();
            }
        }

        private void confirmPasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            // _PasswordInfo.ValidataPasswordContrast();
        }

        private void confirmPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            _PasswordInfo.ValidataPasswordContrast();

        }
    }
}

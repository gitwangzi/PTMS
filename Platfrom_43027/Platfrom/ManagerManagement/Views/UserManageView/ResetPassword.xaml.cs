/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: adaf54e4-eb4a-49db-8a46-74c28ca599b6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.UserManageView
/////    Project Description:    
/////             Class Name: ResetPassword
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 11:08:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 11:08:30
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
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.Manager.Views.UserManageView
{
    public partial class ResetPassword : ChildWindow
    {
        PasswordInfo _PasswordInfo;
        ADAccountServiceClient _AdClient;
        public ResetPassword(string userName)
        {
            _AdClient = ServiceClientFactory.Create<ADAccountServiceClient>();

            _AdClient.ResetPasswordCompleted += AdClient_ResetPasswordCompleted;
            _PasswordInfo = new PasswordInfo(userName);
            this.DataContext = _PasswordInfo;
            InitializeComponent();
            IsValidation();
        }

        private void AdClient_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {
            if (e.Result != null && e.Result.Result)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ResetPwdSuccess"));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail"));
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            _AdClient.ResetPasswordAsync(this._PasswordInfo.UserName, this._PasswordInfo.NewPassword);
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


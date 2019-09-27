/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6f6549bf-66f3-49e8-8849-ef28828a0675      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: ChangePassword
/////          Class Version: v1.0.0.0
/////            Create Time: 8/28/2013 2:01:56 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/28/2013 2:01:56 PM
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
using Gsafety.Common.Controls;
using Gsafety.PTMS.MainPage.Models;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Gsafety.Common.Utilities;
using System.Reflection;

namespace Gsafety.PTMS.MainPage.Views
{
    public partial class ChangePassword : ChildWindow
    {
        #region Fields

        PasswordInfo _PasswordInfo;
        //ADAccountServiceClient _AdClient;
        #endregion

        #region Attributes

        #endregion

        public ChangePassword()
        {
            _PasswordInfo = new PasswordInfo();
            this.DataContext = _PasswordInfo;
            InitializeComponent();
            IsValidation();
        }

        private UserServiceClient InitClient()
        {
            UserServiceClient client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetAccountInfoCompleted += client_GetAccountInfoCompleted;
            client.ModifyPasswordCompleted += client_ModifyPasswordCompleted;

            return client;
        }

        private void CloseClient(UserServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void client_ModifyPasswordCompleted(object sender, ModifyPasswordCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                       ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ChangePassword") + ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail"), 
                       MessageDialogButton.Ok);
                    this.DialogResult = false;
                }

                if (e.Result != null)
                {
                    if (e.Result.Result)
                    {
                        this.DialogResult = true;
                    }
                    else
                    {
                        this.DialogResult = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                CloseClient(client);
            }
        }

        void client_GetAccountInfoCompleted(object sender, GetAccountInfoCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess == true)
                {
                    if (e.Result.Result != null)
                    {
                        LogOperate log = new LogOperate();
                        log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                        log.ID = Guid.NewGuid().ToString();
                        log.OperateTime = DateTime.Now.ToUniversalTime();
                        log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                        log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("Cum_ChangePassword") + ":" + ApplicationContext.Instance.AuthenticationInfo.Account;
                        UserServiceClient client = InitClient();
                        client.ModifyPasswordAsync(ApplicationContext.Instance.AuthenticationInfo.UserID, MD5.GetMd5String(_PasswordInfo.NewPassword), log);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserPwdError"));
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserPwdError"));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                CloseClient(client);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UserServiceClient client = InitClient();
            client.GetAccountInfoAsync(ApplicationContext.Instance.AuthenticationInfo.Account, MD5.GetMd5String(_PasswordInfo.CurrentPassword));
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
            if (conpwd.Equals(newpwd))
            {
                _PasswordInfo.ClearError();
            }
        }

        private void confirmPasswordBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void confirmPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            _PasswordInfo.ValidataPasswordContrast();
        }
    }
}


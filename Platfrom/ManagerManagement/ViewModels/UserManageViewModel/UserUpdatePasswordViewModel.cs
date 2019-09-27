using Gsafety.Common.Controls;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 95c4a264-957a-4515-969f-d112fc23ea9a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels
/////    Project Description:    
/////             Class Name: UserUpdatePasswordViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 15:09:23
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 15:09:23
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.Models;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels
{
    public class UserUpdatePasswordViewModel : BaseEntityViewModel
    {
        public ICommand ResetCommand { get; private set; }
        public ICommand ReturnCommand { get; private set; }
        public ObservableCollection<string> GroupInfo { get; set; }

        private string _CurrentGroup;
        public string CurrentGroup
        {
            get
            {
                return _CurrentGroup;
            }
            set
            {
                _CurrentGroup = value == null ? ApplicationContext.Instance.StringResourceReader.GetString("PleaseInputGroup") : value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentGroup));
            }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value == null ? null : value.Trim();
            }
        }

        private string resetPassword;
        private string userPassword;
        private string newPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                RaisePropertyChanged("UserPassword");
                _ValidateUserpassword();
            }
        }
        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                RaisePropertyChanged("NewPassword");
                _ValidateNewpassword();

            }
        }
        public string ResetPassword
        {
            get { return resetPassword; }
            set
            {
                resetPassword = value;
                RaisePropertyChanged("ResetPassword");
                _ValidateResetPassword();
            }
        }


        private bool CheckIsFinished()
        {
            return IsFinishEnabledB && IsFinishEnabledA && IsFinishEnabledC;
        }

        private bool isFinishEnabledB;
        public bool IsFinishEnabledB
        {
            get
            {
                return isFinishEnabledB;
            }
            set
            {
                isFinishEnabledB = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }

        private bool isFinishEnabledA;
        public bool IsFinishEnabledA
        {
            get
            {
                return isFinishEnabledA;
            }
            set
            {
                isFinishEnabledA = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }
        private bool isFinishEnabledC;
        public bool IsFinishEnabledC
        {
            get
            {
                return isFinishEnabledC;
            }
            set
            {
                isFinishEnabledC = value;
                IsFinishEnabled = CheckIsFinished();
            }
        }


        public UserInfo CurrentUserInfo { get; set; }
        public UserUpdatePasswordViewModel()
        {
            try
            {
                UserPassword = string.Empty;
                NewPassword = string.Empty;
                resetPassword = string.Empty;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => Reset());

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void UserInforClient_ValidateUserCompleted(object sender, ValidateUserCompletedEventArgs e)
        {
            try
            {

                if (e.Result.Result != null)
                {
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserPwdError"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageDialogButton.Ok);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);

                CurrentUserInfo = (UserInfo)viewParameters["update"];
                UserName = CurrentUserInfo.LoginName;
                CurrentGroup = CurrentUserInfo.UserGroup;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserPassword));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => NewPassword));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ResetPassword));
                Reset();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        protected override void OnCommitted()
        {
            try
            {
                IsFinishEnabled = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        private bool isFinishEnabled;
        public bool IsFinishEnabled
        {
            get
            {
                return isFinishEnabled;
            }
            set
            {
                isFinishEnabled = value;
                RaisePropertyChanged("IsFinishEnabled");
            }
        }


        private void _ValidateUserpassword()
        {
            var prop = ExtractPropertyName(() => UserPassword);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(UserPassword))
            {
                IsFinishEnabledA = false;
            }
            else
            {
                //if (UserPassword.ToUpper().Contains(UserName.ToUpper()))
                //{
                //    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordLikeUserName"));
                //    IsFinishEnabledA = false;
                //}
                //else
                //{
                    IsFinishEnabledA = true;
                //}
            }

        }
        private bool InputValidate(string inputstr)
        {
            if (string.IsNullOrEmpty(inputstr))
            {
                return false;
            }
            int[] roleFlag = new int[4];

            foreach (var x in inputstr)
            {
                if (Char.IsDigit(x))
                {
                    roleFlag[0] = 1;
                }
                else if (Char.IsUpper(x))
                {
                    roleFlag[1] = 1;
                }
                else if (Char.IsLower(x))
                {
                    roleFlag[2] = 1;
                }
                else
                {
                    roleFlag[3] = 1;
                }

            }
            if (roleFlag.Sum() > 2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private void _ValidateNewpassword()
        {
            var prop = ExtractPropertyName(() => NewPassword);
            ClearErrors(prop);
            if (string.IsNullOrEmpty(NewPassword))
            {
                IsFinishEnabledB = false;
            }
            if (!InputValidate(NewPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
                IsFinishEnabledB = false;
                return;
            }
            if (NewPassword.Length > 20 || NewPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
                IsFinishEnabledB = false;
            }
            else
            {
                IsFinishEnabledB = true;
            }

        }



        private void _ValidateResetPassword()
        {
            var prop = ExtractPropertyName(() => ResetPassword);
            ClearErrors(prop);
            Regex regex = new Regex(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$", RegexOptions.IgnoreCase);

            if (string.IsNullOrEmpty(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("RequiredField"));
                IsFinishEnabledC = false;
                return;
            }
            if (ResetPassword.Length > 20 || ResetPassword.Length < 7)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
                IsFinishEnabledC = false;
                return;
            }
            else if (ResetPassword != NewPassword)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));
                IsFinishEnabledC = false;
                return;
            }

            if (!InputValidate(ResetPassword))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PasswordIllegal"));
                IsFinishEnabledC = false;
                return;
            }
            else
            {
                IsFinishEnabledC = true;
            }
        }

        private void Reset()
        {
            try
            {
                IsFinishEnabled = false;

                UserPassword = "";
                ResetPassword = "";
                NewPassword = "";

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        private void Return()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void UserInforClient_ResetPasswordCompleted(object sender, ResetPasswordCompletedEventArgs e)
        {
            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("UpdateSuccess"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageDialogButton.Ok);
            Return();
        }
    }
}

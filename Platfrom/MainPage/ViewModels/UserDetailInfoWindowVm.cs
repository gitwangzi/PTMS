using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Manager;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;

namespace Gsafety.Ant.MainPage.ViewModels
{
    public class UserDetailInfoWindowVm : DetailViewModel<GUser>
    {
        private UserServiceClient client;
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public UserDetailInfoWindowVm()
        {
            try
            {
                client = ServiceClientFactory.Create<UserServiceClient>();
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                client.UpdateGUserCompleted += client_UpdateGUserCompleted;
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_UpdateGUserCompleted(object sender, UpdateGUserCompletedEventArgs e)
        {
            try
            {
                SaveResultArgs args = new SaveResultArgs();
                if (e.Cancelled)
                {
                    return;
                }

                if (e.Error != null)
                {
                    args.Result = false;
                    args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }


                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        args.Result = false;
                        args.Message = e.Result.ErrorMsg;
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), e.Result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        args.Result = false;
                        args.Message = ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError);
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    args.Result = true;
                    ResetApplicationContext();
                }

                if (OnSaveResult != null)
                {
                    OnSaveResult(this, args);
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void ActivateView()
        {
            try
            {
                Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                IsReadOnly = false;
                ViewVisibility = Visibility.Visible;
                PTMS.ServiceReference.AccountService.UserServiceClient client = ServiceClientFactory.Create<UserServiceClient>();
                client.GetUserByAccoutNameCompleted += client_GetUserByAccoutNameCompleted;
                client.GetUserByAccoutNameAsync(ApplicationContext.Instance.AuthenticationInfo.Account);
                
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_GetUserByAccoutNameCompleted(object sender, GetUserByAccoutNameCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    CurrentModel = e.Result.Result;
                    InitialFromInitialModel();
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        private void ResetApplicationContext()
        {
            try
            {
                ApplicationContext.Instance.AuthenticationInfo.UserName = CurrentModel.UserName;
                ApplicationContext.Instance.AuthenticationInfo.Phone = CurrentModel.Phone;
                ApplicationContext.Instance.AuthenticationInfo.Mobile = CurrentModel.Mobile;
                ApplicationContext.Instance.AuthenticationInfo.Email = CurrentModel.Email;
                ApplicationContext.Instance.AuthenticationInfo.Address = CurrentModel.Address;
                ApplicationContext.Instance.AuthenticationInfo.Description = CurrentModel.Description;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void InitialFromInitialModel()
        {
            Account = CurrentModel.Account;
            UserName = CurrentModel.UserName;
            Phone = CurrentModel.Phone;
            Mobile = CurrentModel.Mobile;
            Email = CurrentModel.Email;
            Address = CurrentModel.Address;
            Description = CurrentModel.Description;
        }

        protected override void ValidateAll()
        {
            ValidateAccount(ExtractPropertyName(() => Account), _account);
            ValidateUserName(ExtractPropertyName(() => UserName), _username);
            ValidatePhone(ExtractPropertyName(() => Phone), _phone);
            ValidateMobile(ExtractPropertyName(() => Mobile), _mobile);
            ValidateEmail(ExtractPropertyName(() => Email), _email);
            ValidateAddress(ExtractPropertyName(() => Address), _address);
            ValidateDescription(ExtractPropertyName(() => Description), _description);
        }

        protected override void OnCommitted()
        {
            CurrentModel.Account = Account;
            CurrentModel.UserName = UserName;
            CurrentModel.Phone = Phone;
            CurrentModel.Mobile = Mobile;
            CurrentModel.Email = Email;
            CurrentModel.Address = Address;
            CurrentModel.Description = Description;
            Update();
        }

        protected void Update()
        {
            try
            {
                LogOperate log = new LogOperate();
                log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                log.ID = Guid.NewGuid().ToString();
                log.OperateTime = DateTime.Now.ToUniversalTime();
                log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("UpdateUser") + ":" + CurrentModel.Account;
                client.UpdateGUserAsync(CurrentModel, log);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region Property
        private string _account;
        public string Account
        {
            get { return _account; }
            set
            {
                _account = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Account));
            }
        }
        private void ValidateAccount(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private string _username;
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value == null ? null : value.Trim();
                ValidateUserName(ExtractPropertyName(() => UserName), _username);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }
        private void ValidateUserName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value == null ? null : value.Trim();
                ValidateMobile(ExtractPropertyName(() => Phone), _phone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Phone));
            }
        }
        //private void ValidatePhone(string prop, string value)
        //{
        //    ValidatePhone(prop, value);
        //}

        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value == null ? null : value.Trim();
                ValidateMobile(ExtractPropertyName(() => Mobile), _mobile);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Mobile));
            }
        }
        private void ValidateMobile(string prop, string value)
        {
            ValidatePhone(prop, value);
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value == null ? null : value.Trim();
                ValidateEmail(ExtractPropertyName(() => Email), _email);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            }
        }
        private void ValidateEmail(string prop, string value)
        {
            ValidateEmailFormat(prop, value);
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value == null ? null : value.Trim();
                ValidateAddress(ExtractPropertyName(() => Address), _address);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
            }
        }
        private void ValidateAddress(string prop, string value)
        {

        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value == null ? null : value.Trim();
                ValidateDescription(ExtractPropertyName(() => Description), _description);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Description));
            }
        }
        private void ValidateDescription(string prop, string value)
        {
        }

        #endregion
    }
}


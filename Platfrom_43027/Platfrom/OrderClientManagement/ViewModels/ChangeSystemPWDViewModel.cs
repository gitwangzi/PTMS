using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.ComponentModel.DataAnnotations;
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

namespace OrderClientManagement.ViewModels
{
    public class ChangeSystemPWDViewModel : NotifyDataErrorInfo
    {
        public IActionCommand ChangePWDCommand { get; protected set; }
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public ChangeSystemPWDViewModel(string userId, string account)
        {
            try
            {
                UserID = userId;
                _UserName = account;
                string errorContent = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_RequiredError");
                //SetError("CurrentPassword", errorContent);
                SetError("NewPassword", errorContent);
                SetError("ConfirmPassword", errorContent);
                ChangePWDCommand = new ActionCommand<object>(obj => ChangePWDAction());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private UserServiceClient InitClient()
        {
            UserServiceClient client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            //client.GetAccountInfoCompleted += client_GetAccountInfoCompleted;
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
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                       ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_ResetPwd") + ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Setting_Fail"),
                       MessageDialogButton.Ok);
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
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                CloseClient(client);
            }
        }

        //void client_GetAccountInfoCompleted(object sender, GetAccountInfoCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Result.IsSuccess == true)
        //        {
        //            if (e.Result.Result != null)
        //            {
        //                LogOperate log = new LogOperate();
        //                log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
        //                log.ID = Guid.NewGuid().ToString();
        //                log.OperateTime = DateTime.Now.ToUniversalTime();
        //                log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
        //                log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
        //                log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("ModifyPassword") + ":" + ApplicationContext.Instance.AuthenticationInfo.Account;
        //                UserServiceClient client = InitClient();
        //                client.ModifyPasswordAsync(UserID, MD5.GetMd5String(NewPassword), log);
        //            }
        //        }
        //        else
        //        {
        //            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserPwdError"));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
        //    }
        //    finally
        //    {
        //        UserServiceClient client = sender as UserServiceClient;
        //        CloseClient(client);
        //    }
        //}

        private void ChangePWDAction()
        {
            try
            {
                UserServiceClient client = InitClient();
                LogOperate log = new LogOperate();
                log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                log.ID = Guid.NewGuid().ToString();
                log.OperateTime = DateTime.Now.ToUniversalTime();
                log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("Cum_ChangePassword") + ":" + ApplicationContext.Instance.AuthenticationInfo.Account;
                client.ModifyPasswordAsync(UserID, MD5.GetMd5String(NewPassword), log);
                //client.GetAccountInfoAsync(UserName, MD5.GetMd5String(CurrentPassword));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region Attributes
        private string UserID = string.Empty;
        private string _UserName = string.Empty;
        private string _CurrentPassword = string.Empty;
        private string _NewPassword = string.Empty;
        private string _ConfirmPassword = string.Empty;

        public string UserName
        {
            get { return _UserName; }
        }

        public string PasswordForm
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordNotNull");
            }
        }

        //[Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
        //   ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        //[RegularExpression(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$",
        //    ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
        //    ErrorMessageResourceName = "MAINPAGE_PasswordFormError")]
        //public string CurrentPassword
        //{
        //    get { return _CurrentPassword; }
        //    set
        //    {

        //        ClearErrors("CurrentPassword");
        //        try
        //        {
        //            //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "CurrentPassword" });
        //            _CurrentPassword = value;
        //        }
        //        catch (ValidationException ex)
        //        {
        //            SetError("CurrentPassword", ex.Message);
        //        }
        //    }
        //}

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        [StringLength(20, MinimumLength = 7,
             ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "PasswordAccordRule")]
        [RegularExpression(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$",
            ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_PasswordFormError")]
        public string NewPassword
        {
            get { return _NewPassword; }
            set
            {
                ClearErrors("NewPassword");
                try
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "NewPassword" });
                    _NewPassword = value;
                }
                catch (ValidationException ex)
                {
                    SetError("NewPassword", ex.Message);
                }

                // ValidataPasswordContrast();

            }
        }

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set
            {
                ClearErrors("ConfirmPassword");
                try
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ConfirmPassword" });
                    _ConfirmPassword = value;
                    ValidataPasswordContrast();
                }
                catch (ValidationException ex)
                {
                    SetError("ConfirmPassword", ex.Message);
                }
            }
        }

        public void ClearError()
        {
            ClearErrors("ConfirmPassword");
        }

        public void ValidataPasswordContrast()
        {

            if (!NewPassword.Equals(ConfirmPassword))
            {
                SetError("ConfirmPassword", ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordDifferError"));
            }
        }

        #endregion
    }
}

using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    [ExportAsViewModel(ManagerName.AntProductUserDetailVm)]
    public class AntProductUserDetailViewModel : DetailViewModel<GUser>
    {
        private string _currentUserDepartmentID;
        private UserServiceClient client;

        private Visibility _passwordVisisbility;
        public Visibility PasswordVisibility
        {
            get { return _passwordVisisbility; }
            set
            {
                _passwordVisisbility = value;
                RaisePropertyChanged(() => PasswordVisibility);
            }
        }

        private bool _accountReadonly;
        public bool AccountReadonly
        {
            get { return _accountReadonly; }
            set
            {
                _accountReadonly = value;
                RaisePropertyChanged(() => AccountReadonly);
            }
        }

        private ObservableCollection<Role> roleItems;
        public ObservableCollection<Role> RoleItems
        {
            get { return this.roleItems; }
            set
            {
                this.roleItems = value;
                RaisePropertyChanged(() => this.RoleItems);
            }
        }
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public AntProductUserDetailViewModel()
        {
            try
            {
                RoleItems = new ObservableCollection<Role>();

                client = ServiceClientFactory.Create<UserServiceClient>();
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                InitRoleItems();

                client.InsertGUserCompleted += client_InsertGUserCompleted;
                client.UpdateGUserCompleted += client_UpdateGUserCompleted;
                client.IsUserNameExistCompleted += client_IsUserNameExistCompleted;
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_IsUserNameExistCompleted(object sender, IsUserNameExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result)
                {
                    SetError(ExtractPropertyName(() => Account), ApplicationContext.Instance.StringResourceReader.GetString("UserExist"));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_UpdateGUserCompleted(object sender, UpdateGUserCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_InsertGUserCompleted(object sender, InsertGUserCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Return()
        {
            base.Return();

            EventAggregator.Publish(new ViewNavigationArgs(ManagerName.AntProductUserManageV, new Dictionary<string, object>() { { "action", "return" }, { "userDepartmentID", _currentUserDepartmentID } }));
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                _currentUserDepartmentID = viewParameters["userDepartmentID"].ToString();

                switch (action)
                {
                    case "view":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        ViewVisibility = Visibility.Collapsed;
                        PasswordVisibility = Visibility.Collapsed;
                        AccountReadonly = true;
                        CurrentModel = viewParameters["model"] as GUser;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        ClearErrors(() => FirstPassword);
                        ClearErrors(() => SecondPassword);

                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        PasswordVisibility = Visibility.Collapsed;
                        AccountReadonly = true;
                        CurrentModel = viewParameters["model"] as GUser;
                        InitialFromInitialModel();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        AccountReadonly = false;
                        PasswordVisibility = Visibility.Visible;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new GUser();

                        CurrentModel.ID = Guid.NewGuid().ToString();
                        CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                        CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        CurrentModel.IsClientCreate = false;

                        Reset();
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region InitRoleItems
        private void InitRoleItems()
        {
            var roleClient = ServiceClientFactory.Create<RoleServiceClient>();
            roleClient.GetRoleListCompleted += roleClient_GetRoleListCompleted;
            roleClient.GetRoleListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, "", 1, -1);
        }

        void roleClient_GetRoleListCompleted(object sender, GetRoleListCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    var dialog = MessageBoxHelper.ShowDialog(ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        var dialog = MessageBoxHelper.ShowDialog(result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    foreach (var item in result.Result)
                    {
                        RoleItems.Add(item);
                    }

                    if (action == "view" || action == "update")
                    {
                        RoleID = RoleID;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        #endregion


        protected override void Reset()
        {
            if (action == "add")
            {
                Account = string.Empty;
                UserName = string.Empty;
                FirstPassword = string.Empty;
                SecondPassword = string.Empty;
                Phone = string.Empty;
                Mobile = string.Empty;
                Email = string.Empty;
                Address = string.Empty;
                Description = string.Empty;
                RoleID = string.Empty;
            }
            else
            {
                if (CurrentModel != null)
                {
                    InitialFromInitialModel();
                }
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
            RoleID = CurrentModel.RoleID;
        }

        protected override void ValidateAll()
        {
            ValidateAccount(ExtractPropertyName(() => Account), _account);
            ValidateUserName(ExtractPropertyName(() => UserName), _username);
            if (action == "add")
            {
                ValidateFirstPassword(ExtractPropertyName(() => FirstPassword), _firstPassword);
                ValidateSecondPassword(ExtractPropertyName(() => SecondPassword), _secondPassword);
            }

            ValidatePhone(ExtractPropertyName(() => Phone), _phone);
            ValidateMobile(ExtractPropertyName(() => Mobile), _mobile);
            ValidateEmail(ExtractPropertyName(() => Email), _email);
            ValidateAddress(ExtractPropertyName(() => Address), _address);
            ValidateDescription(ExtractPropertyName(() => Description), _description);
        }

        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.Account = Account;
                CurrentModel.UserName = UserName;
                CurrentModel.Department = _currentUserDepartmentID;
                CurrentModel.Phone = Phone;
                CurrentModel.Mobile = Mobile;
                CurrentModel.Email = Email;
                CurrentModel.Address = Address;
                CurrentModel.Description = Description;
                CurrentModel.RoleID = RoleID;
                LogOperate log = new LogOperate();
                log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                log.ID = Guid.NewGuid().ToString();
                log.OperateTime = DateTime.Now.ToUniversalTime();
                log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                if (action.Equals("update"))
                {
                    log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("UpdateUser") + ":" + CurrentModel.Account;
                    Update(log);
                }
                else
                {
                    log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("AddUser") + ":" + CurrentModel.Account;
                    CurrentModel.Password = MD5.GetMd5String(FirstPassword);
                    Add(log);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected void Add(LogOperate log)
        {
            client.InsertGUserAsync(CurrentModel, log);
        }

        protected void Update(LogOperate log)
        {
            client.UpdateGUserAsync(CurrentModel, log);
        }

        private void ValidateAccountSingle()
        {
            client.IsUserNameExistAsync(Account, "");
        }

        #region Property
        private string _account;
        public string Account
        {
            get { return _account; }
            set
            {
                _account = value == null ? null : value.Trim();
                ValidateAccount(ExtractPropertyName(() => Account), _account);
                if (action == "add" && string.IsNullOrWhiteSpace(Account) == false)
                {
                    ValidateAccountSingle();
                }

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

        private string _firstPassword;
        public string FirstPassword
        {
            get { return _firstPassword; }
            set
            {
                _firstPassword = value == null ? null : value.Trim();
                ValidateFirstPassword(ExtractPropertyName(() => FirstPassword), _firstPassword);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstPassword));
            }
        }

        private void ValidateFirstPassword(string prop, string value)
        {
            base.ClearErrors(ExtractPropertyName(() => FirstPassword));
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(FirstPassword))
            {
                if (FirstPassword.Length > 20 || FirstPassword.Length < 7)
                {
                    base.SetError(ExtractPropertyName(() => FirstPassword), ApplicationContext.Instance.StringResourceReader.GetString("PasswordAccordRule"));
                }
                else if (FirstPassword == UserName)
                {
                    base.SetError(ExtractPropertyName(() => FirstPassword), ApplicationContext.Instance.StringResourceReader.GetString("PasswordLikeUserName"));
                }
                else
                {
                    if (!Regex.IsMatch(FirstPassword, @"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$"))
                    {
                        base.SetError(ExtractPropertyName(() => FirstPassword), ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordFormError"));
                    }
                }
            }
        }

        private string _secondPassword;
        public string SecondPassword
        {
            get { return _secondPassword; }
            set
            {
                _secondPassword = value == null ? null : value.Trim();
                ValidateSecondPassword(ExtractPropertyName(() => SecondPassword), _secondPassword);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SecondPassword));
            }
        }

        private void ValidateSecondPassword(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (string.IsNullOrWhiteSpace(FirstPassword) == false && string.IsNullOrWhiteSpace(SecondPassword) == false)
            {
                if (FirstPassword != SecondPassword)
                {
                    base.SetError(ExtractPropertyName(() => SecondPassword), ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));
                }
                else
                {
                    base.ClearErrors(ExtractPropertyName(() => SecondPassword));
                }
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => Phone), _phone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Phone));
            }
        }

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

        private string _roleid;
        public string RoleID
        {
            get { return _roleid; }
            set
            {
                _roleid = value == null ? null : value.Trim();
                ValidateRoleID(ExtractPropertyName(() => RoleID), _roleid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RoleID));
            }
        }
        private void ValidateRoleID(string prop, string value)
        {
            ValidateRequire(prop, value);
        }
        #endregion
    }
}

using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.OrderClientManagement;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.OrderClientService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using OrderClientManagement.Views;
using System;
using System.Reflection;
using System.Windows;

namespace SuperPowerManagement.ViewModels
{
    [ExportAsViewModel(OrderClientName.OrderClientInfoVm)]
    public class OrderClientInfoViewModel : DetailViewModel<GUser>
    {
        #region property....

        Visibility _detailvisibility = Visibility.Collapsed;
        public Visibility DetailVisibility
        {
            get
            {
                return _detailvisibility;
            }
            set
            {
                _detailvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DetailVisibility));
            }
        }

        Visibility _orderclientadminlabel = Visibility.Collapsed;
        public Visibility OrderClientAdminLabel
        {
            get
            {
                return _orderclientadminlabel;
            }
            set
            {
                _orderclientadminlabel = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => OrderClientAdminLabel));
            }
        }
        Visibility _addcommonadvisibility = Visibility.Collapsed;
        public Visibility AddCommonadVisibility
        {
            get
            {
                return _addcommonadvisibility;
            }
            set
            {
                _addcommonadvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AddCommonadVisibility));
            }
        }
        Visibility _updatecommonadvisibility = Visibility.Collapsed;
        public Visibility UpdateCommonadVisibility
        {
            get
            {
                return _updatecommonadvisibility;
            }
            set
            {
                _updatecommonadvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UpdateCommonadVisibility));
            }
        }
        Visibility _passwordvisibility = Visibility.Collapsed;
        public Visibility PasswordVisibility
        {
            get
            {
                return _passwordvisibility;
            }
            set
            {
                _passwordvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PasswordVisibility));
            }
        }
        Visibility _savebuttonvisibility = Visibility.Collapsed;
        public Visibility SaveButtonVisibility
        {
            get
            {
                return _savebuttonvisibility;
            }
            set
            {
                _savebuttonvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SaveButtonVisibility));
            }
        }
        Visibility _resertbuttonvisibility = Visibility.Collapsed;
        public Visibility ResertButtonVisibility
        {
            get
            {
                return _resertbuttonvisibility;
            }
            set
            {
                _resertbuttonvisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ResertButtonVisibility));
            }
        }
        Visibility _backbuttonvisibity = Visibility.Collapsed;
        public Visibility BackButtonVisibity
        {
            get
            {
                return _backbuttonvisibity;
            }
            set
            {
                _backbuttonvisibity = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BackButtonVisibity));
            }
        }

        Visibility _changePWDBtnVisibility = Visibility.Collapsed;
        public Visibility ChangePWDBtnVisibility
        {
            get
            {
                return _changePWDBtnVisibility;
            }
            set
            {
                _changePWDBtnVisibility = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ChangePWDBtnVisibility));
            }
        }
        
        #endregion

        public IActionCommand BtnAddCommand { get; protected set; }
        public IActionCommand BtnUpdateCommand { get; protected set; }
        public IActionCommand BtnChangePWDCommand { get; protected set; }

        public OrderClientInfoViewModel()
        {
            try
            {
                BtnAddCommand = new ActionCommand<object>(obj => Add());
                BtnUpdateCommand = new ActionCommand<object>(obj => Update());
                BtnChangePWDCommand = new ActionCommand<object>(obj => ChangePWD());
                this.Account = "";
                this.UserName = "";
                this.Mobile = "";
                this.Phone = "";
                this.Address = "";
                this.Email = "";
                this.Description = "";
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        private OrderClientServiceClient InitialOrderClient()
        {
            OrderClientServiceClient _client = ServiceClientFactory.Create<OrderClientServiceClient>();
            _client.GetOrderClientCompleted += _client_GetOrderClientCompleted;
            return _client;
        }

        private UserServiceClient InitialUserClient()
        {
            UserServiceClient _clientUser = ServiceClientFactory.Create<UserServiceClient>();
            _clientUser.GetOrderClientGUserByClientIDCompleted += _clientUser_GetOrderClientGUserByClientIDCompleted;
            _clientUser.IsUserNameExistCompleted += _clientUser_IsUserNameExistCompleted;
            _clientUser.InsertOrderClientSystemAdminCompleted += _clientUser_InsertOrderClientSystemAdminCompleted;
            _clientUser.UpdateGUserCompleted += _clientUser_UpdateGUserCompleted;
            return _clientUser;
        }

        void _clientUser_InsertOrderClientSystemAdminCompleted(object sender, InsertOrderClientSystemAdminCompletedEventArgs e)
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
                        ActivateView(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
               
            }
            finally
            {
                CloseUserClient(sender);               
            }

        }
        /// <summary>
        /// 关闭客户端
        /// </summary>


        private void Add()
        {
            InitialForAdd();

            action = "add";
        }

        private void Update()
        {
            InitialForUpdate();

            action = "update";
        }

        private void ChangePWD()
        {
            ChangeSystemPWD window = new ChangeSystemPWD(InitialModel.ID, InitialModel.Account);
            window.Show();
        }

        protected override void Return()
        {
            if (action == "add")
            {
                InitialForNoSystemAdmin();

            }
            else if (action == "update")
            {
                InitialForSystemAdmin();
                SetDataFromInitialModel();
            }
        }

        private void InitialForUpdate()
        {
            try
            {
                AddCommonadVisibility = Visibility.Collapsed;
                UpdateCommonadVisibility = Visibility.Collapsed;
                OrderClientAdminLabel = Visibility.Collapsed;
                DetailVisibility = Visibility.Visible;

                PasswordVisibility = Visibility.Collapsed;
                IsReadOnly = false;

                SaveButtonVisibility = Visibility.Visible;
                ResertButtonVisibility = Visibility.Visible;
                BackButtonVisibity = Visibility.Visible;
                ChangePWDBtnVisibility = Visibility.Visible;

                SetDataFromInitialModel();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void InitialForNoSystemAdmin()
        {
            try
            {
                AddCommonadVisibility = Visibility.Visible;
                UpdateCommonadVisibility = Visibility.Collapsed;
                DetailVisibility = Visibility.Collapsed;
                OrderClientAdminLabel = Visibility.Visible;
                PasswordVisibility = Visibility.Collapsed;
                IsReadOnly = true;

                SaveButtonVisibility = Visibility.Collapsed;
                ResertButtonVisibility = Visibility.Collapsed;
                BackButtonVisibity = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InitialForAdd()
        {
            try
            {
                AddCommonadVisibility = Visibility.Collapsed;
                UpdateCommonadVisibility = Visibility.Collapsed;
                DetailVisibility = Visibility.Visible;
                OrderClientAdminLabel = Visibility.Collapsed;
                IsReadOnly = false;
                SaveButtonVisibility = Visibility.Visible;
                ResertButtonVisibility = Visibility.Visible;
                BackButtonVisibity = Visibility.Visible;
                PasswordVisibility = Visibility.Visible;
                ChangePWDBtnVisibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void InitialForSystemAdmin()
        {
            try
            {
                AddCommonadVisibility = Visibility.Collapsed;
                UpdateCommonadVisibility = Visibility.Visible;
                DetailVisibility = Visibility.Visible;
                OrderClientAdminLabel = Visibility.Collapsed;

                PasswordVisibility = Visibility.Collapsed;
                IsReadOnly = true;
                SaveButtonVisibility = Visibility.Collapsed;
                ResertButtonVisibility = Visibility.Collapsed;
                BackButtonVisibity = Visibility.Collapsed;
                ChangePWDBtnVisibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void _clientUser_GetOrderClientGUserByClientIDCompleted(object sender, GetOrderClientGUserByClientIDCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Result != null)
                {
                    InitialModel = e.Result.Result;

                    InitialForSystemAdmin();

                    SetDataFromInitialModel();
                }
                else
                {
                    InitialForNoSystemAdmin();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_clientUser_GetOrderClientGUserByClientIDCompleted", ex);
            }
            finally
            {
                CloseOrderClient(sender);
            }
        }

        private void SetDataFromInitialModel()
        {
            try
            {
                Account = InitialModel.Account;
                UserName = InitialModel.UserName;
                Phone = InitialModel.Phone;
                Mobile = InitialModel.Mobile;
                Email = InitialModel.Email;
                Address = InitialModel.Address;
                Description = InitialModel.Description;
                Password = InitialModel.Password;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void _client_GetOrderClientCompleted(object sender, GetOrderClientCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Result != null)
                {

                    BeginTime = e.Result.Result.BeginTime.ToLocalTime().ToString();
                    EndTime = e.Result.Result.EndTime.ToLocalTime().ToString();
                    DeviceCount = e.Result.Result.DeviceCount.ToString();
                    UserCount = e.Result.Result.UserCount.ToString();
                    AccountName = ApplicationContext.Instance.AuthenticationInfo.Account;

                    if (e.Result.Result.Status == (short)StatusEnum.Normal)
                    {
                        Status = ApplicationContext.Instance.StringResourceReader.GetString("OrderClientStatus_Run");
                    }
                    else
                    {
                        Status = ApplicationContext.Instance.StringResourceReader.GetString("OrderClientStatus_Suspended");
                    }

                    if (e.Result.Result.TansferMode == TansferModeEnum.NoTransfer)
                    {
                        TansferMode = ApplicationContext.Instance.StringResourceReader.GetString("NoToPlice");
                    }
                    else if (e.Result.Result.TansferMode == TansferModeEnum.DirectTransfer)
                    {
                        TansferMode = ApplicationContext.Instance.StringResourceReader.GetString("GoPlice");
                    }
                    else
                    {
                        TansferMode = ApplicationContext.Instance.StringResourceReader.GetString("WaitGoPlice");
                    }

                    if (e.Result.Result._platformversion == (short)VersionEnum.Basic)
                    {
                        Version = ApplicationContext.Instance.StringResourceReader.GetString("VersionEnum_Basic");
                    }
                    else if (e.Result.Result._platformversion == (short)VersionEnum.Standard)
                    {
                        Version = ApplicationContext.Instance.StringResourceReader.GetString("VersionEnum_Standard");
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseOrderClient(sender);
            }
        }

        private static void CloseOrderClient(object sender)
        {
            OrderClientServiceClient _client = sender as OrderClientServiceClient;
            if (_client != null)
            {
                _client.CloseAsync();
                _client = null;
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                OrderClientServiceClient _client = InitialOrderClient();
                UserServiceClient _clientUser = InitialUserClient();
                _client.GetOrderClientAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
                _clientUser.GetOrderClientGUserByClientIDAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 重置页面数据
        /// </summary>
        protected override void Reset()
        {
            try
            {
                if (action == "add")
                {
                    Account = string.Empty;
                    UserName = string.Empty;
                    Password = string.Empty;
                    Phone = string.Empty;
                    Mobile = string.Empty;
                    Email = string.Empty;
                    Address = string.Empty;
                    Description = string.Empty;
                }
                else
                {
                    SetDataFromInitialModel();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region property...
        private string _begintime;
        public string BeginTime
        {
            get
            {
                return _begintime;
            }
            set
            {
                _begintime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => BeginTime));
            }
        }

        private string _endtime;
        public string EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                _endtime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
            }
        }

        private string _tansfermode;
        public string TansferMode
        {
            get
            {
                return _tansfermode;
            }
            set
            {
                _tansfermode = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => TansferMode));
            }
        }

        private string _devicecount;
        public string DeviceCount
        {
            get
            {
                return _devicecount;
            }
            set
            {
                _devicecount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceCount));
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Status));
            }
        }

        private string _usercount;
        public string UserCount
        {
            get
            {
                return _usercount;
            }
            set
            {
                _usercount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserCount));
            }
        }

        private string _accountName;
        public string AccountName
        {
            get
            {
                return _accountName;
            }
            set
            {
                _accountName = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AccountName));
            }
        }


        private string _version;
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Version));
            }
        } 
        #endregion

        protected override void OnCommitted()
        {
            try
            {
                if (action == "add")
                {
                    UserServiceClient _clientUser = InitialUserClient();
                    this.InitialOrderClient();
                    _clientUser.IsUserNameExistAsync(Account, string.Empty);
                }
                else if (action == "update")
                {
                    this.InitialOrderClient();
                    UserServiceClient _clientUser = InitialUserClient();
                    _clientUser.IsUserNameExistAsync(Account, InitialModel.ID);
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void ValidateAll()
        {
            ValidateRequire(ExtractPropertyName(() => Account), Account);
            ValidateRequire(ExtractPropertyName(() => UserName), UserName);
            ValidateRequire(ExtractPropertyName(() => Password), Password);
            ValidatePhone(ExtractPropertyName(() => Phone), Phone);
            ValidatePhone(ExtractPropertyName(() => Mobile), Mobile);
            ValidateEmailFormat(ExtractPropertyName(() => Email), Email);
            ValidateRequire(ExtractPropertyName(() => Address), Address);
        }

        /// <summary>
        /// 判断帐户名是否已经存在
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _clientUser_IsUserNameExistCompleted(object sender, IsUserNameExistCompletedEventArgs e)
        {
            try
            {
                ClearErrors(ExtractPropertyName(() => UserName));
                if (e.Result.Result)
                {
                    base.SetError(ExtractPropertyName(() => UserName),
                        ApplicationContext.Instance.StringResourceReader.GetString("AccountNameExistsPleaseUpdate"));

                }
                else
                {
                    CurrentModel = new GUser();
                    CurrentModel.Account = Account;
                    CurrentModel.UserName = UserName;

                    CurrentModel.Phone = Phone;
                    CurrentModel.Mobile = Mobile;
                    CurrentModel.Email = Email;
                    CurrentModel.Address = Address;
                    CurrentModel.Description = Description;
                    if (action == "add")
                    {
                        CurrentModel.ID = Guid.NewGuid().ToString();
                        CurrentModel.Password = MD5.GetMd5String(Password);

                        CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                        CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                        CurrentModel.IsClientCreate = true;
                        CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                        
                        UserServiceClient _clientUser = InitialUserClient();
                        _clientUser.InsertOrderClientSystemAdminAsync(CurrentModel);
                    }
                    else
                    {
                        CurrentModel.ID = InitialModel.ID;

                        CurrentModel.Creator = InitialModel.Creator;
                        CurrentModel.CreateTime = InitialModel.CreateTime;
                        CurrentModel.IsClientCreate = InitialModel.IsClientCreate;
                        CurrentModel.RoleID = InitialModel.RoleID;
                        CurrentModel.ClientID = InitialModel.ClientID;

                        UserServiceClient _clientUser = InitialUserClient();
                        _clientUser.UpdateGUserAsync(CurrentModel, null);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseUserClient(sender);
            }

        }

        void _clientUser_UpdateGUserCompleted(object sender, UpdateGUserCompletedEventArgs e)
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
                        InitialModel = CurrentModel;

                        InitialForSystemAdmin();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseUserClient(sender);
            }

        }

        private static void CloseUserClient(object sender)
        {
            UserServiceClient _clientUser = sender as UserServiceClient;
            if (_clientUser != null)
            {
                _clientUser.CloseAsync();
                _clientUser = null;
            }
        }



        private string _account;
        public string Account
        {
            get { return _account; }
            set
            {
                _account = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => Account), _account);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Account));
            }
        }

        private string _username;
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => UserName), _username);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => Password), _password);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Password));
            }
        }

        private string _password2;
        public string Password2
        {
            get { return _password2; }
            set
            {
                _password2 = value == null ? null : value.Trim();
                ValidatePassword2(ExtractPropertyName(() => Password2), _password2);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Password2));
            }
        }
        private void ValidatePassword2(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Password2) || Password != Password2)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_CarNumberCheckFailed"));
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
                ValidatePhone(ExtractPropertyName(() => Mobile), _mobile);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Mobile));
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value == null ? null : value.Trim();
                if (IsReadOnly == false)
                {
                    base.ValidateEmailFormat(ExtractPropertyName(() => Email), _email);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value == null ? null : value.Trim();
                ValidateRequire(ExtractPropertyName(() => Address), _address);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Description));
            }
        }
    }
}

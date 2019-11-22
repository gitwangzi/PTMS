using Gsafety.Common.Controls;
using Gsafety.Common.Utilities;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Framework;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
//using Gsafety.PTMS.ServiceReference.VehicleCompanyService;

namespace Gsafety.PTMS.PTMS
{
    [ExportAsView("LoginView", IsShell = true)]
    public partial class LoginPage
    {
        #region Fields

        UserServiceClient _userClient = null;
        OrganizationClient _organizationClient = null;
        InstallStationServiceClient _installStationClient = null;
        AuthenticationInfo _authenticationInfo = new AuthenticationInfo();
        private LoginNavigationContainer NavContainer;
        private AsyncOperation asyncOper;
        private Thread _DataLoader;
        private bool _hasLoginin = false;
        private object _object = new object();

        #endregion

        #region Attributes

        [Import]
        public IEventAggregator EventAggregator { get; set; }
        public AuthenticationInfo AuthenticationInfos
        {
            get { return _authenticationInfo; }
            set { _authenticationInfo = value; }
        }

        #endregion

        public LoginPage()
        {
            InitializeComponent();
            this.DataContext = AuthenticationInfos;
            LayoutRoot.MouseRightButtonDown += LayoutRoot_MouseRightButtonDown;
            LayoutRoot.KeyDown += LayoutRoot_KeyDown;
            LayoutRoot.KeyDown += OnLogin;
            Loaded += LoginPage_Loaded;
            asyncOper = AsyncOperationManager.CreateOperation(null);
            loginImg.Visibility = System.Windows.Visibility.Collapsed;
        }

        void LayoutRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                e.Handled = true;
            }
        }

        void OnLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(null, null);
            }
        }

        void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();
            this.tbUserName.Focus();
//#if DEBUG
//            AuthenticationInfos.UserName = "cljk";
//            AuthenticationInfos.Password = "123456";
//#endif
            LayoutRoot.KeyDown += LayoutRoot_KeyDown;
        }

        void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationInfos.UserName = string.Empty;
            AuthenticationInfos.Password = string.Empty;
            HtmlPage.Window.Invoke("CloseShell");
        }

        private void tbUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            AuthenticationInfos.Message = string.Empty;
        }

        private void tbPassWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            AuthenticationInfos.Message = string.Empty;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationInfos.Message = string.Empty;

            if (string.IsNullOrEmpty(tbUserName.Text.Trim()) || string.IsNullOrEmpty(tbPassWord.Password.Trim()))
            {
                loginImg.Visibility = System.Windows.Visibility.Visible;
                AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_LoginError");
            }
            else
            {
                loginImg.Visibility = System.Windows.Visibility.Collapsed;
                AuthenticationInfos.UserName = this.tbUserName.Text.Trim();
                string password = MD5.GetMd5String(this.tbPassWord.Password.Trim());
                _userClient = ServiceClientFactory.Create<UserServiceClient>();
                _userClient.GetAccountInfoCompleted += _userClient_GetAccountInfoCompleted;
                _userClient.GetAccountInfoAsync(AuthenticationInfos.UserName, password);
                AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_Logging");
            }
        }

        void _userClient_GetAccountInfoCompleted(object sender, GetAccountInfoCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    if (e.Error.InnerException is TimeoutException)
                    {
                        AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_LoginTimeoutError");
                    }
                    else if (e.Error.InnerException is CommunicationException)
                    {
                        AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_LoginServerError");
                    }
                    else
                    {
                        AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_LoginServerError");
                    }
                    //aaaaa.Text = authenticationInfo.Message;
                    loginImg.Visibility = System.Windows.Visibility.Visible;
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.Error.InnerException);
                    return;
                }

                if (e.Result.Result != null && e.Result.Result.Allowed)
                {
                    lock (_object)
                    {
                        if (_hasLoginin == false)
                        {
                            _hasLoginin = true;
                        }
                        else
                        {
                            return;
                        }
                    }


                    AccountInfo account = e.Result.Result;
                    AuthenticationInfos.ClientID = account.User.ClientID;
                    AuthenticationInfos.Account = account.User.Account;
                    AuthenticationInfos.Department = account.User.Department;
                    AuthenticationInfos.UserName = account.User.UserName;
                    AuthenticationInfos.UserID = account.User.ID;
                    AuthenticationInfos.IsClientCreate = account.User.IsClientCreate;

                    AuthenticationInfos.Phone = account.User.Phone;
                    AuthenticationInfos.Mobile = account.User.Mobile;
                    AuthenticationInfos.Address = account.User.Address;
                    AuthenticationInfos.Email = account.User.Email;
                    AuthenticationInfos.Description = account.User.Description;
                    //get role and functionitems
                    AuthenticationInfos.Role = account.Role;
                    AuthenticationInfos.TransferMode = e.Result.Result.TransferMode;

                    ApplicationContext.Instance.AuthenticationInfo = AuthenticationInfos;

                    if (AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.SuperPower)
                    {
                        Navigate(MainPage.MainPageName.SuperPlatformV);
                    }
                    else if (AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.ClientAdmin)
                    {
                        Navigate(MainPage.MainPageName.OrderClientPlatformV);
                    }
                    else if (AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.SecurityAdmin || AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.SecurityMonitor)
                    {
                        _organizationClient = ServiceClientFactory.Create<OrganizationClient>();
                        _organizationClient.GetOrganizationByUserCompleted += _organizationClient_GetOrganizationByUserCompleted;
                        _organizationClient.GetOrganizationByUserAsync(AuthenticationInfos.UserID);
                    }
                    else if (AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.MaintainAdmin || AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.MaintainMonitor)
                    {
                        _installStationClient = ServiceClientFactory.Create<InstallStationServiceClient>();
                        _installStationClient.GetInstallStationsByUserCompleted += _installStationClient_GetInstallStationsByUserCompleted;
                        _installStationClient.GetInstallStationsByUserAsync(AuthenticationInfos.UserID);
                    }
                }
                else
                {
                    loginImg.Visibility = System.Windows.Visibility.Visible;
                    if (e.Result.ErrorMsg != null)
                        AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg);
                }
            }
            catch (Exception ex)
            {
                loginImg.Visibility = System.Windows.Visibility.Visible;
                AuthenticationInfos.Message = ApplicationContext.Instance.StringResourceReader.GetString("LOGIN_LoginServerError");
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
            finally
            {
                if (_userClient != null)
                {
                    _userClient.ChannelFactory.Close();
                    _userClient = null;
                }
            }
        }

        void _installStationClient_GetInstallStationsByUserCompleted(object sender, GetInstallStationsByUserCompletedEventArgs e)
        {
            if (e.Result.Result != null)
            {
                AuthenticationInfos.Stations = new System.Collections.Generic.List<InstallStation>();
                foreach (var item in e.Result.Result)
                {
                    AuthenticationInfos.Stations.Add(item);
                }
            }

            if (e.Result.Result != null)
            {
                loginControl.Visibility = System.Windows.Visibility.Collapsed;
                AuthenticationInfos.IsBusy = true;
                ApplicationContext.Instance.BufferManager.DataLoading();
            }

            _DataLoader = new System.Threading.Thread(BufferData_Loading);
            _DataLoader.Start();
        }


        void _organizationClient_GetOrganizationByUserCompleted(object sender, GetOrganizationByUserCompletedEventArgs e)
        {
            if (e.Result.Result != null)
            {
                loginControl.Visibility = System.Windows.Visibility.Collapsed;
                ApplicationContext.Instance.AuthenticationInfo.Organizations = e.Result.Result;

                AuthenticationInfos.IsBusy = true;
                ApplicationContext.Instance.BufferManager.DataLoading();
            }

            _DataLoader = new System.Threading.Thread(BufferData_Loading);
            _DataLoader.Start();
        }

        void Navigate(string frame)
        {
            if (NavContainer == null)
            {
                NavContainer = new LoginNavigationContainer();
            }

            dataLoading.IsBusy = false;
            LayoutRoot.Children.Remove(NavContainer);
            Grid.SetColumnSpan(NavContainer, 3);
            Grid.SetRowSpan(NavContainer, 3);
            Grid.SetRow(NavContainer, 0);
            Grid.SetColumn(NavContainer, 0);
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(NavContainer);

            EventAggregator.Publish(frame.AsViewNavigationArgs());
            EventAggregator.Publish(new ViewNavigationArgs("LoginView") { Deactivate = true });

            tbPassWord.Password = string.Empty;
            //tbUserName.Text = string.Empty;
            LayoutRoot.KeyDown -= OnLogin;

            ApplicationContext.Instance.MessageClient.Init();
        }

        private void BufferData_Loading()
        {
            while (!ApplicationContext.Instance.BusyInfo.IsInitComplete)
            {
                System.Threading.Thread.Sleep(500);
            }
            asyncOper.Post(result =>
            {
                if ((AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.SecurityAdmin || AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.SecurityMonitor))
                {
                    Navigate(MainPage.MainPageName.CentralPlatformV2);
                }
                else if (AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.MaintainAdmin || AuthenticationInfos.Role.RoleCategory == (short)RoleCategory.MaintainMonitor)
                {
                    Navigate(MainPage.MainPageName.InstallPlatformV);
                }

                LoginLogServiceClient loginLogServiceClient = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.PTMSLogManageService.LoginLogServiceClient>();
                LogAccess access = new LogAccess();
                access.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                access.ID = Guid.NewGuid().ToString();
                access.LoginTime = DateTime.Now.ToUniversalTime();
                access.LoginUser = ApplicationContext.Instance.AuthenticationInfo.Account;
                access.SessionID = ApplicationContext.Instance.MessageClient.SessionID;
                access.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                access.UserType = ApplicationContext.Instance.AuthenticationInfo.Role.RoleCategory;
                loginLogServiceClient.AddLoginLogAsync(access);
            }, null);
        }
    }
}

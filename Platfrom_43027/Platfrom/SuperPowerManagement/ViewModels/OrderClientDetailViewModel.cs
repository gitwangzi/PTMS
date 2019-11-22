using BaseLib.ViewModels;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Common.Enum;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.OrderClientService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SuperPowerManagement;
using Jounce.Core.ViewModel;
using Jounce.Framework;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace SuperPowerManagement.ViewModels
{
    [ExportAsViewModel(SuperPowerName.AddCloudAccountViewModel)]
    public class OrderClientDetailViewModel : DetailViewModel<OrderClientEx>
    {

        string notNull = ApplicationContext.Instance.StringResourceReader.GetString("NotNull");

        public event EventHandler<SaveResultArgs> OnSaveResult;

        bool _usernamereadonly = false;

        public bool UserNameReadOnly
        {
            get { return _usernamereadonly; }
            set
            {
                _usernamereadonly = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserNameReadOnly));
            }
        }

        private UserServiceClient InitialUserClient()
        {
            UserServiceClient clientUser = ServiceClientFactory.Create<UserServiceClient>();
            clientUser.IsUserNameExistCompleted += _clientUser_IsUserNameExistCompleted;

            return clientUser;
        }

        private OrderClientServiceClient InitialOrderClient()
        {
            OrderClientServiceClient client = ServiceClientFactory.Create<OrderClientServiceClient>();
            client.UpdateOrderClientCompleted += _client_UpdateOrderClientCompleted;
            client.InsertOrderClientCompleted += _client_InsertOrderClientCompleted;
            return client;
        }
        /// <summary>
        ///激活界面
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="viewParameters"></param>
        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();

                switch (action)
                {
                    case "view":
                        //modify the title
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        IsEnable = false;
                        ViewVisibility = Visibility.Collapsed;
                        UserNameReadOnly = true;
                        InitialModel = viewParameters["view"] as OrderClientEx;
                        InitialFromInitialModel();

                        break;

                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        UserNameReadOnly = true;
                        JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserNameReadOnly));
                        InitialModel = viewParameters["view"] as OrderClientEx;
                        InitialFromInitialModel();
                        IsEnable = true;

                        CurrentModel = new OrderClientEx();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        UserNameReadOnly = false;
                        CurrentModel = new OrderClientEx();
                        IsEnable = true;
                        Reset();
                        break;
                    default:
                        break;
                }
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
                    FirstCheck = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));
                    FirstStatus = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstStatus));
                    FirstVersion = true;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstVersion));

                    UserName = string.Empty;
                    Name = string.Empty;
                    Address = string.Empty;
                    Phone = string.Empty;
                    Mobile = string.Empty;
                    Email = string.Empty;
                    Contact = string.Empty;
                    UserCount = string.Empty;
                    DeviceCount = string.Empty;
                    BeginTime = DateTime.Now.ToShortDateString();
                    EndTime = DateTime.Now.AddYears(1).ToShortDateString();
                }
                else if (action == "update")
                {
                    InitialFromInitialModel();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 编辑 详细 数据加载
        /// </summary>
        public void InitialFromInitialModel()
        {
            this.IsOpened = true;
            BeginTime = InitialModel.BeginTime.ToLocalTime().ToString();
            EndTime = InitialModel.EndTime.ToLocalTime().ToString();
            UserName = InitialModel.UserName;
            Name = InitialModel.Name;
            Address = InitialModel.Address;
            Phone = InitialModel.Phone;
            Mobile = InitialModel.Mobile;
            Email = InitialModel.Email;
            Contact = InitialModel.Contact;
            UserCount = InitialModel.UserCount.ToString();
            DeviceCount = InitialModel.DeviceCount.ToString();
            if (InitialModel.Status == StatusEnum.Normal)
            {
                FirstStatus = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstStatus));
            }
            else
            {
                SecondStatus = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SecondStatus));
            }

            if (InitialModel.TansferMode == TansferModeEnum.NoTransfer)
            {
                FirstCheck = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstCheck));
            }
            else if (InitialModel.TansferMode == TansferModeEnum.DirectTransfer)
            {
                SecondCheck = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SecondCheck));
            }
            else
            {
                ThirdCheck = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ThirdCheck));
            }

            if (InitialModel._platformversion == (short)VersionEnum.Basic)
            {
                FirstVersion = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstVersion));
            }
            else if (InitialModel._platformversion == (short)VersionEnum.Standard)
            {
                SecondVersion = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SecondVersion));
            }
        }

        #region RadioButton
        public bool FirstCheck
        {
            get;
            set;
        }

        public bool SecondCheck
        {
            get;
            set;
        }

        public bool ThirdCheck
        {
            get;
            set;
        }

        public bool FirstStatus
        {
            get;
            set;
        }

        public bool SecondStatus
        {
            get;
            set;
        }
        public bool FirstVersion
        {
            get;
            set;
        }

        public bool SecondVersion
        {
            get;
            set;
        }
        #endregion

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), name);
            ValidateBeginTime(ExtractPropertyName(() => BeginTime), begintime);
            ValidateEndTime(ExtractPropertyName(() => EndTime), endtime);
            ValidatePhone(ExtractPropertyName(() => Phone), phone);
            ValidateMobile(ExtractPropertyName(() => Mobile), mobile);
            ValidateEmail(ExtractPropertyName(() => Email), email);
            ValidateContact(ExtractPropertyName(() => Contact), contact);
            ValidateAddress(ExtractPropertyName(() => Address), address);
            ValidateUserCount(ExtractPropertyName(() => UserCount), userCount);
            ValidateDeviceCount(ExtractPropertyName(() => DeviceCount), deviceCount);
        }

        protected override void OnCommitted()
        {
            if (action.Equals("update"))
            {
                GetValueForCurrentModel();
                CurrentModel.ID = InitialModel.ID;
                Update();
            }
            else
            {
                UserServiceClient clientUser = InitialUserClient();
                clientUser.IsUserNameExistAsync(UserName, string.Empty);
            }
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
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.CurrentAccountNameExists));
                    base.SetError(ExtractPropertyName(() => UserName), LProxy.CurrentAccountNameExists);

                }
                else
                {
                    GetValueForCurrentModel();

                    CurrentModel.ID = Guid.NewGuid().ToString();
                    Add();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("_clientUser_IsUserNameExistCompleted", ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                CloseUserClient(client);
            }
        }

        private void CloseUserClient(UserServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void GetValueForCurrentModel()
        {
            CurrentModel.UserName = UserName;
            CurrentModel.Name = Name;
            try
            {
                CurrentModel.BeginTime = Convert.ToDateTime(DateTime.Parse(BeginTime).ToShortDateString() + " " + "00:00:00").ToUniversalTime();
                CurrentModel.EndTime = Convert.ToDateTime(DateTime.Parse(EndTime).ToShortDateString() + " " + "23:59:59").ToUniversalTime();
            }
            catch (Exception)
            { }
            CurrentModel.Address = Address;
            CurrentModel.Phone = Phone;
            CurrentModel.Mobile = Mobile;
            CurrentModel.Email = Email;
            CurrentModel.Contact = Contact;
            CurrentModel.DeviceCount = Convert.ToInt32(DeviceCount);
            CurrentModel.UserCount = Convert.ToInt32(UserCount);

            if (FirstCheck)
            {
                CurrentModel.TansferMode = Gsafety.PTMS.ServiceReference.OrderClientService.TansferModeEnum.NoTransfer;
            }
            else if (SecondCheck)
            {
                CurrentModel.TansferMode = Gsafety.PTMS.ServiceReference.OrderClientService.TansferModeEnum.DirectTransfer;
            }
            else
            {
                CurrentModel.TansferMode = Gsafety.PTMS.ServiceReference.OrderClientService.TansferModeEnum.JudgeTransfer;
            }

            if (FirstStatus)
            {
                CurrentModel.Status = Gsafety.PTMS.ServiceReference.OrderClientService.StatusEnum.Normal;
            }
            else if (SecondStatus)
            {
                CurrentModel.Status = Gsafety.PTMS.ServiceReference.OrderClientService.StatusEnum.Stop;
            }

            if (FirstVersion)
            {
                CurrentModel._platformversion = (short)VersionEnum.Basic;
            }
            else if (SecondVersion)
            {
                CurrentModel._platformversion = (short)VersionEnum.Standard;
            }
        }

        protected override void Return()
        {
            this.IsOpened = false;
        }

        protected void Add()
        {
            OrderClientServiceClient orderclient = InitialOrderClient();
            LogManager log = new LogManager();
            log.ManagerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.Manager = ApplicationContext.Instance.AuthenticationInfo.Account;
            log.Content = ApplicationContext.Instance.StringResourceReader.GetString("Add");
            log.ClientID = CurrentModel.ID;
            log.ClientName = CurrentModel.Name;
            log.ID = Guid.NewGuid().ToString();
            orderclient.InsertOrderClientAsync(CurrentModel, log);
        }

        void _client_InsertOrderClientCompleted(object sender, InsertOrderClientCompletedEventArgs e)
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
                        ///通知业务服务更新客户转警方式
                        Gsafety.PTMS.ServiceReference.MessageServiceExt.OrderClient client = new Gsafety.PTMS.ServiceReference.MessageServiceExt.OrderClient();
                        client.ID = CurrentModel.ID;
                        switch (CurrentModel.TansferMode)
                        {
                            case TansferModeEnum.NoTransfer:
                                client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.NoTransfer;
                                break;
                            case TansferModeEnum.DirectTransfer:
                                client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.DirectTransfer;
                                break;
                            case TansferModeEnum.JudgeTransfer:
                                client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.JudgeTransfer;
                                break;
                            default:
                                break;
                        }

                        ApplicationContext.Instance.MessageClient.ClientChange(client);

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
                ApplicationContext.Instance.Logger.LogException("_client_InsertOrderClientCompleted", ex);
            }
            finally
            {
                OrderClientServiceClient client = sender as OrderClientServiceClient;
                CloseOrderClient(client);
            }

        }

        private void CloseOrderClient(OrderClientServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Update()
        {
            OrderClientServiceClient orderclient = InitialOrderClient();
            LogManager log = new LogManager();
            log.ManagerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.Manager = ApplicationContext.Instance.AuthenticationInfo.Account;
            log.Content = ApplicationContext.Instance.StringResourceReader.GetString("Modify");
            log.ClientID = CurrentModel.ID;
            log.ClientName = CurrentModel.Name;
            log.ID = Guid.NewGuid().ToString();
            orderclient.UpdateOrderClientAsync(CurrentModel, log);
        }

        void _client_UpdateOrderClientCompleted(object sender, UpdateOrderClientCompletedEventArgs e)
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
                        if (InitialModel.TansferMode != CurrentModel.TansferMode)
                        {
                            Gsafety.PTMS.ServiceReference.MessageServiceExt.OrderClient client = new Gsafety.PTMS.ServiceReference.MessageServiceExt.OrderClient();
                            client.ID = InitialModel.ID;
                            switch (CurrentModel.TansferMode)
                            {
                                case TansferModeEnum.NoTransfer:
                                    client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.NoTransfer;
                                    break;
                                case TansferModeEnum.DirectTransfer:
                                    client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.DirectTransfer;
                                    break;
                                case TansferModeEnum.JudgeTransfer:
                                    client.TansferMode = Gsafety.PTMS.ServiceReference.MessageServiceExt.TansferModeEnum.JudgeTransfer;
                                    break;
                                default:
                                    break;
                            }

                            ApplicationContext.Instance.MessageClient.ClientChange(client);
                        }

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
                ApplicationContext.Instance.Logger.LogException("_client_UpdateOrderClientCompleted", ex);
            }
            finally
            {
                OrderClientServiceClient orderclient = sender as OrderClientServiceClient;
                CloseOrderClient(orderclient);
            }
        }


        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value == null ? null : value.Trim();
                ValidateUserName(ExtractPropertyName(() => UserName), userName);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value == null ? null : value.Trim();
                ValidateName(ExtractPropertyName(() => Name), name);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            }
        }
        private string begintime = DateTime.Now.ToShortDateString() + " 00:00:00";
        public string BeginTime
        {
            get { return begintime; }
            set
            {
                begintime = value;
                ValidateBeginTime(ExtractPropertyName(() => BeginTime), begintime);
            }
        }
        private string endtime = DateTime.Now.AddYears(1).ToShortDateString() + " 23:59:59";
        public string EndTime
        {
            get { return endtime; }
            set
            {
                endtime = value;
                ValidateEndTime(ExtractPropertyName(() => EndTime), endtime);
            }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                address = value == null ? null : value.Trim();
                ValidateAddress(ExtractPropertyName(() => Address), address);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
            }
        }
        private string phone;
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value == null ? null : value.Trim();
                ValidatePhone(ExtractPropertyName(() => Phone), phone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Phone));
            }
        }
        private string mobile;
        public string Mobile
        {
            get { return mobile; }
            set
            {
                mobile = value == null ? null : value.Trim();
                ValidateMobile(ExtractPropertyName(() => Mobile), mobile);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Mobile));
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value == null ? null : value.Trim();
                ValidateEmail(ExtractPropertyName(() => Email), email);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Email));
            }
        }
        private string contact;
        public string Contact
        {
            get { return contact; }
            set
            {
                contact = value == null ? null : value.Trim();
                ValidateContact(ExtractPropertyName(() => Contact), contact);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            }
        }
        private string userCount;
        public string UserCount
        {
            get { return userCount; }
            set
            {
                userCount = value == null ? null : value.Trim();
                ValidateUserCount(ExtractPropertyName(() => UserCount), userCount);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserCount));
            }
        }
        private string deviceCount;
        public string DeviceCount
        {
            get { return deviceCount; }
            set
            {
                deviceCount = value == null ? null : value.Trim();
                ValidateDeviceCount(ExtractPropertyName(() => DeviceCount), deviceCount);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceCount));
            }
        }

        private void ValidateUserName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }
        private void ValidateName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private void ValidateBeginTime(string prop, string value)
        {
            ClearErrors(prop);
            if (!ValidateRequire(prop, value))
            {
                return;
            }

            DateTime bt = DateTime.Parse(BeginTime);
            if (!string.IsNullOrEmpty(EndTime))
            {
                DateTime et = DateTime.Parse(EndTime);
                if (bt > et)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                }
            }
        }

        private void ValidateEmail(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(value))
            {
                ValidateEmailFormat(prop, value);
            }
        }

        private void ValidateEndTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(EndTime.ToString()))
            {
                base.SetError(prop, notNull);
            }
            else
            {
                if (!string.IsNullOrEmpty(BeginTime.ToString()))
                {
                    DateTime begin = Convert.ToDateTime(BeginTime);
                    DateTime end = Convert.ToDateTime(EndTime);
                    int i = DateTime.Compare(end, begin);
                    if (i < 0)
                    {
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("Endtimeislessthanthestarttime"));
                    }
                }
            }
        }
        private void ValidateAddress(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private void ValidatePhone(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(EndTime.ToString()))
            {
                base.SetError(prop, notNull);
            }
            else
            {
                if (!Regex.IsMatch(value, @"^[0-9]\d*$", RegexOptions.IgnoreCase))
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("Mustbeanumber"));
                }
            }
        }

        private void ValidateMobile(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(EndTime.ToString()))
            {
                base.SetError(prop, notNull);
            }
            else
            {
                if (!Regex.IsMatch(value, @"^[0-9]\d*$", RegexOptions.IgnoreCase))
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("Mustbeanumber"));
                }
            }
        }

        private void ValidateContact(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private void ValidateUserCount(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(value))
            {
                ValidatePosIntFormat(prop, value);
            }
        }

        private void ValidateDeviceCount(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(value))
            {
                ValidatePosIntFormat(prop, value);
            }
        }

        private bool? _isOpened;

        public bool? IsOpened
        {
            get { return this._isOpened; }
            set
            {
                this._isOpened = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsOpened));
            }
        }

    }
}

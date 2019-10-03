using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.ServiceReference.DevGpsService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Reflection;
using System.Windows;


namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.DevGpsDetailViewVm)]
    public class DevGpsDetailViewModel : DetailViewModel<DevGps>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;

        private string _gpssn;
        public string GpsSn
        {
            get { return _gpssn; }
            set
            {
                _gpssn = value == null ? null : value.Trim();
                ValidateGpsSn(ExtractPropertyName(() => GpsSn), _gpssn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GpsSn));
            }
        }
        private void ValidateGpsSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(GpsSn))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _gpssim;
        public string GpsSim
        {
            get { return _gpssim; }
            set
            {
                _gpssim = value == null ? null : value.Trim();
                ValidateGpsSim(ExtractPropertyName(() => GpsSim), _gpssim);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GpsSim));
            }
        }
        private void ValidateGpsSim(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(GpsSim))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _gpsuid;
        public string GpsUid
        {
            get { return _gpsuid; }
            set
            {
                _gpsuid = value == null ? null : value.Trim();
                ValidateGpsUid(ExtractPropertyName(() => GpsUid), _gpsuid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => GpsUid));
            }
        }

        private void ValidateGpsUid(string p, string value)
        {
            ValidateRequire(p, value);
        }

        public Visibility saveButtonVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility SaveButtonVisibility
        {
            get
            {
                return this.saveButtonVisibility;
            }
            set
            {
                this.saveButtonVisibility = value;
                RaisePropertyChanged(() => this.SaveButtonVisibility);
            }
        }

        public Visibility resertButtonVisibility;
        /// <summary>
        /// 
        /// </summary>
        public Visibility ResertButtonVisibility
        {
            get
            {
                return this.saveButtonVisibility;
            }
            set
            {
                this.resertButtonVisibility = value;
                RaisePropertyChanged(() => this.ResertButtonVisibility);
            }
        }

        /// <summary>
        /// 初始化定位服务客户端
        /// </summary>
        /// <returns></returns>
        private DevGpsServiceClient InitializeDevGpsServiceClient()
        {
            DevGpsServiceClient client = null;
            client = ServiceClientFactory.Create<DevGpsServiceClient>();
            client.InsertDevGpsCompleted += client_InsertDevGpsCompleted;
            client.UpdateDevGpsCompleted += client_UpdateDevGpsCompleted;
            return client;
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                switch (action)
                {
                    case "view":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                        IsReadOnly = true;
                        ViewVisibility = Visibility.Collapsed;
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;

                        InitialModel = viewParameters["model"] as DevGps;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        InitialModel = viewParameters["model"] as DevGps;
                        InitialFromInitialModel();
                        CurrentModel = new DevGps();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        CurrentModel = new DevGps();
                        CurrentModel.Status = (short)DeviceSuiteStatus.Initial;

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
        protected override void Reset()
        {
            if (InitialModel == null)
            {
                GpsSn = string.Empty;
                GpsSim = string.Empty;
                GpsUid = string.Empty;
            }
            else
            {
                InitialFromInitialModel();
            }
        }

        /// <summary>
        /// 初始化表单数据
        /// </summary>
        public void InitialFromInitialModel()
        {
            GpsSn = InitialModel.GpsSn;
            GpsSim = InitialModel.GpsSim;
            GpsUid = InitialModel.GpsUid;

        }
        protected override void ValidateAll()
        {
            ValidateGpsSn(ExtractPropertyName(() => GpsSn), _gpssn);

            ValidateGpsSim(ExtractPropertyName(() => GpsSim), _gpssim);

            ValidateGpsUid(ExtractPropertyName(() => GpsUid), _gpsuid);
        }
        protected override void OnCommitted()
        {
            CurrentModel.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            CurrentModel.GpsSn = GpsSn;
            CurrentModel.GpsSim = GpsSim;
            CurrentModel.GpsUid = GpsUid;
            CurrentModel.InstallStatus = PTMS.ServiceReference.DevGpsService.InstallStatusType.UnInstall;

            CurrentModel.Valid = 1;
            if (action.Equals("update"))
            {
                CurrentModel.ID = InitialModel.ID;
                CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                CurrentModel.Creator = InitialModel.Creator;
                CurrentModel.Status = InitialModel.Status;
                Update();
            }
            else
            {
                CurrentModel.ID = Guid.NewGuid().ToString();
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                Add();
            }
        }
        protected override void Return()
        {
            base.Return();
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.DevGpsManageViewV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", CurrentModel } }));
        }
        protected void Add()
        {
            DevGpsServiceClient client = this.InitializeDevGpsServiceClient();
            client.InsertDevGpsAsync(CurrentModel);
        }
        protected void Update()
        {
            DevGpsServiceClient client = this.InitializeDevGpsServiceClient();
            client.UpdateDevGpsAsync(CurrentModel);
        }

        /// <summary>
        /// 关闭与服务端的连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseDevGpsServiceClient(DevGpsServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void client_InsertDevGpsCompleted(object sender, InsertDevGpsCompletedEventArgs e)
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
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                   e.Result.ExceptionMessage);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }

                        return;
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                DevGpsServiceClient client = sender as DevGpsServiceClient;
                this.CloseDevGpsServiceClient(client);
            }

        }

        private void client_UpdateDevGpsCompleted(object sender, UpdateDevGpsCompletedEventArgs e)
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
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                   e.Result.ExceptionMessage);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }

                        return;
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                DevGpsServiceClient client = sender as DevGpsServiceClient;
                this.CloseDevGpsServiceClient(client);
            }
        }
    }
}


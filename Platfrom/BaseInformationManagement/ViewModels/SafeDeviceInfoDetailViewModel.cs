using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SafeDeviceInfoDetailVm)]
    public class SafeDeviceInfoDetailViewModel : DetailViewModel<DevSuite>
    {

        #region 属性

        public List<EnumModel> ProtocolTypes { get; set; }
        public event EventHandler<SaveResultArgs> OnSaveResult;

        #region 数据属性
        private string _suiteid;
        public string SuiteID
        {
            get { return _suiteid; }
            set
            {
                _suiteid = value == null ? null : value.Trim();
                ValidateSuiteID(ExtractPropertyName(() => SuiteID), _suiteid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteID));
            }
        }
        private void ValidateSuiteID(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(SuiteID))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _mdvrcoresn;
        public string MdvrCoreSn
        {
            get { return _mdvrcoresn; }
            set
            {
                _mdvrcoresn = value == null ? null : value.Trim();
                ValidateMdvrCoreSn(ExtractPropertyName(() => MdvrCoreSn), _mdvrcoresn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrCoreSn));
            }
        }
        private void ValidateMdvrCoreSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(MdvrCoreSn))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _mdvrsn;
        public string MdvrSn
        {
            get { return _mdvrsn; }
            set
            {
                _mdvrsn = value == null ? null : value.Trim();
                ValidateMdvrSn(ExtractPropertyName(() => MdvrSn), _mdvrsn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrSn));
            }
        }
        private void ValidateMdvrSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(MdvrSn))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _mdvrsim;
        public string MdvrSim
        {
            get { return _mdvrsim; }
            set
            {
                _mdvrsim = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrSim));
            }
        }

        private string _mdvrsimmobile;
        public string MdvrSimMobile
        {
            get { return _mdvrsimmobile; }
            set
            {
                _mdvrsimmobile = value == null ? null : value.Trim();
                ValidateMdvrSimMobile(ExtractPropertyName(() => MdvrSimMobile), _mdvrsimmobile);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MdvrSimMobile));
            }
        }
        private void ValidateMdvrSimMobile(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(MdvrSimMobile))
            {
                foreach (var item in MdvrSimMobile)
                {
                    if (!char.IsDigit(item))
                    {
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                        break;
                    }
                }

            }
        }
        private string _upssn;
        public string UpsSn
        {
            get { return _upssn; }
            set
            {
                _upssn = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UpsSn));
            }
        }

        private string _sdsn;
        public string SdSn
        {
            get { return _sdsn; }
            set
            {
                _sdsn = value == null ? null : value.Trim();
                ValidateSdSn(ExtractPropertyName(() => SdSn), _sdsn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SdSn));
            }
        }
        private void ValidateSdSn(string prop, string value)
        {

        }
        private string _softwareversion;
        public string SoftwareVersion
        {
            get { return _softwareversion; }
            set
            {
                _softwareversion = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SoftwareVersion));
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Model));
            }
        }

        private int _protocol;
        public int Protocol
        {
            get { return _protocol; }
            set
            {
                _protocol = value;
                //ValidateProtocol(ExtractPropertyName(() => Protocol), _protocol);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Protocol));
            }
        }
        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
            }
        }
        #endregion

        #endregion
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                RaisePropertyChanged(() => IsEnable);
            }
        }

        #region 构造函数

        public SafeDeviceInfoDetailViewModel()
        {
            try
            {
                InitProrocolTypeItems();
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #endregion


        #region 方法

        /// <summary>
        /// 初始化安全套件服务客户端
        /// </summary>
        /// <returns></returns>
        private BscDevSuiteServiceClient InitialClient()
        {
            BscDevSuiteServiceClient client = ServiceClientFactory.Create<BscDevSuiteServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.InsertBscDevSuiteCompleted += client_InsertBscDevSuiteCompleted;
            client.UpdateBscDevSuiteCompleted += client_UpdateBscDevSuiteCompleted;

            return client;
        }

        private void CloseClient(BscDevSuiteServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        /// <summary>
        /// 更新安全套件完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateBscDevSuiteCompleted(object sender, UpdateBscDevSuiteCompletedEventArgs e)
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

                ApplicationContext.Instance.Logger.LogException("BscDevSuiteDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                CloseClient(client);
            }
        }


        /// <summary>
        /// 新增安全套件完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_InsertBscDevSuiteCompleted(object sender, InsertBscDevSuiteCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("BscDevSuiteDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                BscDevSuiteServiceClient client = sender as BscDevSuiteServiceClient;
                CloseClient(client);
            }
        }

        private void InitProrocolTypeItems()
        {
            try
            {
                var adapter = new EnumAdapter<Gsafety.PTMS.Bases.Enums.ProtocolTypeEnum>();
                var categorys = adapter.GetEnumInfos();

                ProtocolTypes = new List<EnumModel>();
                foreach (var item in categorys)
                {
                    ProtocolTypes.Add(new EnumModel()
                    {
                        EnumValue = item.Value,
                        EnumName = item.Name,
                        ShowName = item.LocalizedString,
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public new void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            action = viewParameters["action"].ToString();
            switch (action)
            {
                case "view":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_View");
                    IsReadOnly = true;
                    IsEnabled = false;
                    ViewVisibility = Visibility.Collapsed;
                    InitialModel = viewParameters["model"] as DevSuite;
                    InitialFromInitialModel();
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                    IsReadOnly = false;
                    IsEnabled = true;
                    ViewVisibility = Visibility.Visible;
                    InitialModel = viewParameters["model"] as DevSuite;
                    InitialFromInitialModel();
                    CurrentModel = new DevSuite();
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                    IsReadOnly = false;
                    IsEnabled = true;
                    ViewVisibility = Visibility.Visible;
                    CurrentModel = new DevSuite()
                    {
                        SuiteInfoID = Guid.NewGuid().ToString(),
                        ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                        Status = (short)DeviceSuiteStatus.Initial,
                        BscDevSuiteParts = new ObservableCollection<DevSuitePart>(),
                        InstallStatus = Gsafety.PTMS.ServiceReference.BscDevSuiteService.InstallStatusType.UnInstall,
                    };
                    Reset();
                    break;
            }
        }
        protected override void Reset()
        {
            if (action == "add")
            {
                SuiteID = string.Empty;
                MdvrCoreSn = string.Empty;
                MdvrSn = string.Empty;
                MdvrSim = string.Empty;
                MdvrSimMobile = string.Empty;
                UpsSn = string.Empty;
                SdSn = string.Empty;
                SoftwareVersion = string.Empty;
                Model = string.Empty;
                Note = string.Empty;
                if (ProtocolTypes.Count > 0)
                {
                    Protocol = ProtocolTypes.FirstOrDefault().EnumValue;
                }
            }
            else
            {
                if (InitialModel != null)
                {
                    InitialFromInitialModel();
                }

            }
        }

        public void InitialFromInitialModel()
        {
            SuiteID = InitialModel.SuiteID;
            MdvrCoreSn = InitialModel.MdvrCoreSn;
            MdvrSn = InitialModel.MdvrSn;
            MdvrSim = InitialModel.MdvrSim;
            MdvrSimMobile = InitialModel.MdvrSimMobile;
            UpsSn = InitialModel.UpsSn;
            SdSn = InitialModel.SdSn;
            SoftwareVersion = InitialModel.SoftwareVersion;
            Model = InitialModel.Model;
            Protocol = (int)InitialModel.Protocol;
            Note = InitialModel.Note;
        }

        protected override void ValidateAll()
        {
            ValidateSuiteID(ExtractPropertyName(() => SuiteID), _suiteid);
            ValidateMdvrCoreSn(ExtractPropertyName(() => MdvrCoreSn), _mdvrcoresn);
            ValidateMdvrSn(ExtractPropertyName(() => MdvrSn), _mdvrsn);
            ValidateMdvrSimMobile(ExtractPropertyName(() => MdvrSimMobile), _mdvrsimmobile);
            ValidateSdSn(ExtractPropertyName(() => SdSn), _sdsn);
        }

        protected override void OnCommitted()
        {

            CurrentModel.SuiteID = SuiteID;
            CurrentModel.MdvrCoreSn = MdvrCoreSn;
            CurrentModel.MdvrSn = MdvrSn;
            CurrentModel.MdvrSim = MdvrSim;
            CurrentModel.MdvrSimMobile = MdvrSimMobile;
            CurrentModel.UpsSn = UpsSn;
            CurrentModel.SdSn = SdSn;
            CurrentModel.SoftwareVersion = SoftwareVersion;
            CurrentModel.Model = Model;
            CurrentModel.Protocol = (Gsafety.PTMS.ServiceReference.BscDevSuiteService.ProtocolTypeEnum)Protocol;
            CurrentModel.Note = Note;

            if (action.Equals("update"))
            {
                CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                CurrentModel.ClientID = InitialModel.ClientID;
                CurrentModel.Status = InitialModel.Status;
                CurrentModel.InstallStatus = InitialModel.InstallStatus;
                CurrentModel.SuiteInfoID = InitialModel.SuiteInfoID;
                Update();
            }
            else
            {
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                Add();
            }
        }

        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SafeDeviceInfoV, new Dictionary<string, object>() { { "action", "return" } }));
        }

        protected void Add()
        {
            BscDevSuiteServiceClient client = InitialClient();
            client.InsertBscDevSuiteAsync(CurrentModel);
        }

        protected void Update()
        {
            BscDevSuiteServiceClient client = InitialClient();
            client.UpdateBscDevSuiteAsync(CurrentModel);
        }

        #endregion


    }
}

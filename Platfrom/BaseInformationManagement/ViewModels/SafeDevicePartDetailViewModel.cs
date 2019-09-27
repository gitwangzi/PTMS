using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.BscDevSuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.SafeDevicePartDetailVm)]
    public class SafeDevicePartDetailViewModel : DetailViewModel<DevSuitePart>
    {
        public List<EnumModel> PartTypes { get; set; }

        //private BscDevSuitePartServiceClient client;

        public event EventHandler<SaveResultArgs> OnSaveResult;

        public SafeDevicePartDetailViewModel()
        {
            InitPartType();
            //client = ServiceClientFactory.Create<BscDevSuitePartServiceClient>();

            //client.InsertBscDevSuitePartCompleted += client_InsertBscDevSuitePartCompleted;
            //client.UpdateBscDevSuitePartCompleted += client_UpdateBscDevSuitePartCompleted;
        }

        /// <summary>
        /// 初始化安全套件配件服务客户端
        /// </summary>
        /// <returns></returns>
        private BscDevSuitePartServiceClient InitializeBscDevSuitePartServiceClient()
        {
            BscDevSuitePartServiceClient client = null;
            client = ServiceClientFactory.Create<BscDevSuitePartServiceClient>();
            client.InsertBscDevSuitePartCompleted += client_InsertBscDevSuitePartCompleted;
            client.UpdateBscDevSuitePartCompleted += client_UpdateBscDevSuitePartCompleted;
            return client;
        }

        /// <summary>
        /// 关闭与服务端连接
        /// </summary>
        /// <param name="client"></param>
        private void CloseBscDevSuitePartServiceClient(BscDevSuitePartServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void client_UpdateBscDevSuitePartCompleted(object sender, UpdateBscDevSuitePartCompletedEventArgs e)
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
                // InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_UpdateInstallStationCompleted", ex);
            }
            finally
            {
                //if(this.client != null)
                //{
                //    this.client.CloseAsync();
                //}
                //this.client = null;
                BscDevSuitePartServiceClient client = sender as BscDevSuitePartServiceClient;
                this.CloseBscDevSuitePartServiceClient(client);
                //InilitizeViewModel();
            }
        }

        private void client_InsertBscDevSuitePartCompleted(object sender, InsertBscDevSuitePartCompletedEventArgs e)
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
                //InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_UpdateInstallStationCompleted", ex);
            }
            finally
            {
                //if(this.client != null)
                //{
                //    this.client.CloseAsync();
                //}
                //this.client = null;
                BscDevSuitePartServiceClient client = sender as BscDevSuitePartServiceClient;
                this.CloseBscDevSuitePartServiceClient(client);
            }
        }


        private void InitPartType()
        {
            try
            {
                var adapter = new EnumAdapter<Gsafety.PTMS.Enums.BscDevSuitePartTypeEnum>();
                var categorys = adapter.GetEnumInfos();

                PartTypes = new List<EnumModel>();
                foreach (var item in categorys)
                {
                    PartTypes.Add(new EnumModel()
                    {
                        EnumValue = item.Value,
                        EnumName = item.Name,
                        //ShowName = item.LocalizedString
                        ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item.Name),
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
                        InitialModel = viewParameters["model"] as DevSuitePart;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as DevSuitePart;
                        InitialFromInitialModel();
                        CurrentModel = InitialModel;
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        //InitialModel = viewParameters["model"] as BscDevSuitePart;
                        //CurrentModel = InitialModel;
                        CurrentModel = new DevSuitePart();
                        bscDevSuit = viewParameters["model"] as DevSuite;
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
            if (action == "add")
            {
                PartSn = string.Empty;
                Name = string.Empty;
                Model = string.Empty;
                PartType = PartTypes.FirstOrDefault().EnumValue;
                ProduceTime = null;
            }
            else
            {
                InitialFromInitialModel();
            }
        }

        public void InitialFromInitialModel()
        {
            try
            {
                PartSn = InitialModel.PartSn;
                Name = InitialModel.Name;
                Model = InitialModel.Model;
                PartType = (int)InitialModel.PartType;
                ProduceTime = InitialModel.ProduceTime;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void ValidateAll()
        {
            ValidatePartSn(ExtractPropertyName(() => PartSn), _partsn);
            ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateModel(ExtractPropertyName(() => Model), _model);
            ValidateProduceTime(ExtractPropertyName(() => ProduceTime), _producetime);
        }
        protected override void OnCommitted()
        {

            CurrentModel.PartSn = PartSn;
            CurrentModel.Name = Name;
            CurrentModel.Model = Model;
            CurrentModel.PartType = (Gsafety.PTMS.ServiceReference.BscDevSuiteService.BscDevSuitePartTypeEnum)PartType;
            CurrentModel.ProduceTime = ProduceTime.Value.ToUniversalTime();
            if (action.Equals("update"))
            {
                CurrentModel.ID = InitialModel.ID;
                CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                CurrentModel.SuiteInfoID = InitialModel.SuiteInfoID;
                Update();
            }
            else
            {
                CurrentModel.ID = Guid.NewGuid().ToString();
                CurrentModel.SuiteInfoID = bscDevSuit.SuiteInfoID;
                CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                Add();
            }
        }

        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.SafeDeviceInfoDetailV,
            new Dictionary<string, object>() { { "action", "return" } }));
        }
        DevSuite bscDevSuit { get; set; }
        protected void Add()
        {
            BscDevSuitePartServiceClient client = this.InitializeBscDevSuitePartServiceClient();
            client.InsertBscDevSuitePartAsync(CurrentModel);
        }

        protected void Update()
        {
            // Return();
            BscDevSuitePartServiceClient client = this.InitializeBscDevSuitePartServiceClient();
            client.UpdateBscDevSuitePartAsync(CurrentModel);
        }
        #region property.....

        private string _partsn;
        public string PartSn
        {
            get { return _partsn; }
            set
            {
                _partsn = value == null ? null : value.Trim();
                ValidatePartSn(ExtractPropertyName(() => PartSn), _partsn);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PartSn));
            }
        }
        private void ValidatePartSn(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(PartSn))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null ? null : value.Trim();
                ValidateName(ExtractPropertyName(() => Name), _name);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            }
        }
        private void ValidateName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Name))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value == null ? null : value.Trim();
                ValidateModel(ExtractPropertyName(() => Model), _model);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Model));
            }
        }
        private void ValidateModel(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Model))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private int _parttype;
        public int PartType
        {
            get { return _parttype; }
            set
            {
                _parttype = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PartType));
            }
        }

        private DateTime? _producetime = DateTime.Now;
        public DateTime? ProduceTime
        {
            get { return _producetime; }
            set
            {
                _producetime = value;
                ValidateProduceTime(ExtractPropertyName(() => ProduceTime), _producetime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ProduceTime));
            }
        }

        private void ValidateProduceTime(string prop, DateTime? time)
        {
            ClearErrors(prop);
            if (!time.HasValue)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        #endregion
    }
}

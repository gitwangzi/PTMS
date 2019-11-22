using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using PublicServiceManagement;
using System;
using System.Collections.Generic;
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
namespace Gsafety.PTMS.PublicServiceManagement.Views.ViewModels
{
    [ExportAsViewModel(PublicServiceName.FoundRegistryDetailVm)]
    public class FoundRegistryDetailViewModel : DetailViewModel<FoundRegistry>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public FoundRegistryDetailViewModel()
        {
            InitialLanguage();
            InitialStatus();
        }

        private FoundRegistryClient InitialClient()
        {
            FoundRegistryClient clientService = ServiceClientFactory.Create<FoundRegistryClient>();
            clientService.InsertFoundRegistryCompleted += client_InsertFoundRegistryCompleted;
            clientService.UpdateFoundRegistryCompleted += clientService_UpdateFoundRegistryCompleted;

            return clientService;
        }

        private void clientService_UpdateFoundRegistryCompleted(object sender, UpdateFoundRegistryCompletedEventArgs e)
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
                //InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                CloseClient(sender);
                // InilitizeViewModel();
            }
        }

        private void client_InsertFoundRegistryCompleted(object sender, InsertFoundRegistryCompletedEventArgs e)
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
                //InilitizeViewModel();
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                CloseClient(sender);
                // InilitizeViewModel();
            }
        }

        private static void CloseClient(object sender)
        {
            FoundRegistryClient clientService = sender as FoundRegistryClient;
            if (clientService != null)
            {
                clientService.CloseAsync();
                clientService = null;
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
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;

                        InitialModel = viewParameters["model"] as FoundRegistry;
                        InitialFromInitialModel();
                        IsEnable = false;
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        InitialModel = viewParameters["model"] as FoundRegistry;
                        InitialFromInitialModel();
                        CurrentModel = new FoundRegistry();
                        IsEnable = true;
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        CurrentModel = new FoundRegistry();
                        Reset();
                        IsEnable = true;
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
            if (action == "update")
            {
                InitialFromInitialModel();
            }
            else
            {
                Founder = string.Empty;
                FounderIDCard = string.Empty;
                FoundPhone = string.Empty;
                Content = string.Empty;
                Keyword = string.Empty;
                LostName = string.Empty;
                LostPhone = string.Empty;
                Address = string.Empty;
                VehicleID = string.Empty;
                ClaimTime = null;
            }
        }

        public void InitialFromInitialModel()
        {
            Founder = InitialModel.Founder;
            FounderIDCard = InitialModel.FounderIDCard;
            FoundPhone = InitialModel.FoundPhone;
            FoundTime = InitialModel.FoundTime.ToLocalTime().ToString();
            Content = InitialModel.Content;
            Keyword = InitialModel.Keyword;
            LostName = InitialModel.LostName;
            LostPhone = InitialModel.LostPhone;
            Address = InitialModel.Address;
            Status = (int)InitialModel.Status == 0 ? SouLRR[0] : SouLRR[1];
            VehicleID = InitialModel.VehicleID;
            ClaimTime = InitialModel.ClaimTime;

        }

        protected override void ValidateAll()
        {
            ValidateFounder(ExtractPropertyName(() => Founder), _founder);
            ValidateFounderIDCard(ExtractPropertyName(() => FounderIDCard), _founderidcard);
            ValidateFoundTime(ExtractPropertyName(() => FoundTime), _foundtime);
            ValidateFoundPhone(ExtractPropertyName(() => FoundPhone), _foundphone);

            ValidateContent(ExtractPropertyName(() => Content), _content);
            ValidateKeyword(ExtractPropertyName(() => Keyword), _keyword);

            ValidateClaimTime((ExtractPropertyName(() => ClaimTime)), _claimtime);
        }

        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.Founder = Founder;
                CurrentModel.FounderIDCard = FounderIDCard;
                CurrentModel.FoundPhone = FoundPhone;
                CurrentModel.FoundTime = DateTime.Parse(FoundTime).ToUniversalTime();
                CurrentModel.Content = Content;
                CurrentModel.Keyword = Keyword;
                CurrentModel.LostName = LostName;// == "" ? null : LostName;
                CurrentModel.LostPhone = LostPhone;// == "" ? null :LostPhone;
                CurrentModel.Address = Address;// == "" ? null : Address;
                CurrentModel.Status = Status.EnumValue;
                CurrentModel.VehicleID = VehicleID;
                if (ClaimTime.HasValue)
                    CurrentModel.ClaimTime = ClaimTime.Value.ToUniversalTime();

                if (action.Equals("update"))
                {
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    Update();

                }
                else
                {
                    CurrentModel.ID = Guid.NewGuid().ToString();
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    Add();

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Return()
        {
            // EventAggregator.Publish(new ViewNavigationArgs(PublicServiceName.FoundRegistryManageV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", CurrentModel } }));
        }
        protected void Add()
        {
            FoundRegistryClient clientService = InitialClient();
            clientService.InsertFoundRegistryAsync(CurrentModel);
        }
        protected void Update()
        {
            FoundRegistryClient clientService = InitialClient();
            clientService.UpdateFoundRegistryAsync(CurrentModel);
        }

        protected void InitialStatus()
        {
            SouLRR = new List<EnumModel>();
            SouLRR.Add(new EnumModel() { EnumValue = 0, EnumName = "length", ShowName = FoundRegistry_Noresolve });
            SouLRR.Add(new EnumModel() { EnumValue = 1, EnumName = "time", ShowName = FoundRegistry_resolve });

            Status = SouLRR[0];
        }
        string FoundRegistry_resolve;
        string FoundRegistry_Noresolve;
        protected void InitialLanguage()
        {
            FoundRegistry_resolve = ApplicationContext.Instance.StringResourceReader.GetString("FoundRegistry_resolve");
            FoundRegistry_Noresolve = ApplicationContext.Instance.StringResourceReader.GetString("FoundRegistry_Noresolve");
        }


        private List<EnumModel> _souLRR;
        public List<EnumModel> SouLRR
        {
            get { return _souLRR; }
            set
            {
                _souLRR = value;
                RaisePropertyChanged(() => SouLRR);

            }
        }

        #region Button Visible
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

        #endregion


        #region property.....

        private string _founder;
        public string Founder
        {
            get { return _founder; }
            set
            {
                _founder = value == null ? null : value.Trim();
                ValidateFounder(ExtractPropertyName(() => Founder), _founder);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Founder));
            }
        }
        private void ValidateFounder(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Founder))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _founderidcard;
        public string FounderIDCard
        {
            get { return _founderidcard; }
            set
            {
                _founderidcard = value == null ? null : value.Trim();
                ValidateFounderIDCard(ExtractPropertyName(() => FounderIDCard), _founderidcard);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FounderIDCard));
            }
        }
        private void ValidateFounderIDCard(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(FounderIDCard))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _foundphone;
        public string FoundPhone
        {
            get { return _foundphone; }
            set
            {
                _foundphone = value == null ? null : value.Trim();
                ValidateFoundPhone(ExtractPropertyName(() => FoundPhone), _foundphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FoundPhone));
            }
        }
        private void ValidateFoundPhone(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(FoundPhone))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {
                long result = 0;
                if (!long.TryParse(value, out result))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                }
            }
        }
        private string _foundtime;
        public string FoundTime
        {
            get { return _foundtime; }
            set
            {
                _foundtime = value;
                ValidateFoundTime(ExtractPropertyName(() => FoundTime), _foundtime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FoundTime));
            }
        }

        private void ValidateFoundTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(FoundTime))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value == null ? null : value.Trim();
                ValidateContent(ExtractPropertyName(() => Content), _content);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Content));
            }
        }
        private void ValidateContent(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Content))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }
            set
            {
                _keyword = value == null ? null : value.Trim();
                ValidateKeyword(ExtractPropertyName(() => Keyword), _keyword);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Keyword));
            }
        }
        private void ValidateKeyword(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Keyword))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _lostname;
        public string LostName
        {
            get { return _lostname; }
            set
            {
                _lostname = value == null ? null : value.Trim();

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LostName));
            }
        }

        private string _lostphone;
        public string LostPhone
        {
            get { return _lostphone; }
            set
            {
                _lostphone = value == null ? null : value.Trim();
                ValidateLostPhone(ExtractPropertyName(() => LostPhone), _lostphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LostPhone));
            }
        }

        private void ValidateLostPhone(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(LostPhone))
            {
                long result = 0;
                if (!long.TryParse(value, out result))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
                }
            }

        }


        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value == null ? null : value.Trim();

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Address));
            }
        }

        private EnumModel _status;
        public EnumModel Status
        {
            get { return _status; }
            set
            {
                _status = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Status));
            }
        }

        private string _vehicleid;
        public string VehicleID
        {
            get { return _vehicleid; }
            set
            {
                _vehicleid = value == null ? null : value.Trim();

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleID));
            }
        }

        DateTime? _claimtime;

        public DateTime? ClaimTime
        {
            get { return _claimtime; }
            set
            {
                _claimtime = value;
                ValidateClaimTime((ExtractPropertyName(() => ClaimTime)), _claimtime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ClaimTime));
            }
        }

        private void ValidateClaimTime(string prop, DateTime? value)
        {
            ClearErrors(prop);

            if (Status != null && Status.EnumValue == 1 && !value.HasValue)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));
            }


        }
        #endregion
    }
}


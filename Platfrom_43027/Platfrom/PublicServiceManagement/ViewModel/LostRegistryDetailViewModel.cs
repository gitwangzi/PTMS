using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using PublicServiceManagement;
using System;
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
    [ExportAsViewModel(PublicServiceName.LostRegistryDetailVm)]
    public class LostRegistryDetailViewModel : DetailViewModel<LostRegistry>
    {
        
        public event EventHandler<SaveResultArgs> OnSaveResult;
        public LostRegistryDetailViewModel()
        {
            
        }

        private LostRegistryClient InitServiceClient()
        {
            LostRegistryClient client = ServiceClientFactory.Create<LostRegistryClient>();
            client.InsertLostRegistryCompleted += client_InsertLostRegistryCompleted;
            client.UpdateLostRegistryCompleted += client_UpdateLostRegistryCompleted;
            return client;
        }

        private void client_UpdateLostRegistryCompleted(object sender, UpdateLostRegistryCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            LostRegistryClient client = sender as LostRegistryClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        private void client_InsertLostRegistryCompleted(object sender, InsertLostRegistryCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("DriverInfoDetailViewModel.client_AddDriverInfoCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
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
                        IsEnable = false;
                        ViewVisibility = Visibility.Collapsed;
                        SaveButtonVisibility = Visibility.Collapsed;
                        ResertButtonVisibility = Visibility.Collapsed;

                        InitialModel = viewParameters["model"] as LostRegistry;
                        InitialFromInitialModel();
                        IsTrue = false;
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        InitialModel = viewParameters["model"] as LostRegistry;
                        InitialFromInitialModel();
                        CurrentModel = new LostRegistry();
                        IsTrue = true;
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        SaveButtonVisibility = Visibility.Visible;
                        ResertButtonVisibility = Visibility.Visible;

                        CurrentModel = new LostRegistry();
                        Reset();
                        IsTrue = true;
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
            try
            {
                if (action == "update")
                {
                    InitialFromInitialModel();
                }
                else
                {
                    LostName = string.Empty;
                    Content = string.Empty;
                    Keyword = string.Empty;
                    LostIdcard = string.Empty;
                    LostPhone = string.Empty;
                    Address = string.Empty;
                    LostTime = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);                                                                  
            }
        }

        public void InitialFromInitialModel()
        {
            LostName = InitialModel.LostName;
            Content = InitialModel.Content;
            Keyword = InitialModel.Keyword;
            LostIdcard = InitialModel.LostIdcard;
            LostPhone = InitialModel.LostPhone;
            Address = InitialModel.Address;
            LostTime = InitialModel.LostTime.ToString();
        }

        protected override void ValidateAll()
        {
            ValidateLostName(ExtractPropertyName(() => LostName), _lostname);
            ValidateContent(ExtractPropertyName(() => Content), _content);
            ValidateKeyword(ExtractPropertyName(() => Keyword), _keyword);
            ValidateLostIdcard(ExtractPropertyName(() => LostIdcard), _lostidcard);
            ValidateLostPhone(ExtractPropertyName(() => LostPhone), _lostphone);
            ValidateAddress(ExtractPropertyName(() => Address), _address);
            ValidateLostTime(ExtractPropertyName(() => LostTime), _losttime);

        }
        protected override void OnCommitted()
        {

            try
            {
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.LostName = LostName;
                CurrentModel.Content = Content;
                CurrentModel.Keyword = Keyword;
                CurrentModel.LostIdcard = LostIdcard;
                CurrentModel.LostPhone = LostPhone;
                CurrentModel.LostTime = DateTime.Parse(LostTime).ToUniversalTime();
                CurrentModel.Address = Address;


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
            EventAggregator.Publish(new ViewNavigationArgs(PublicServiceName.LostRegistryManageV, new System.Collections.Generic.Dictionary<string, object>() { { "action", "return" }, { "model", CurrentModel } }));
        }
        protected void Add()
        {
            LostRegistryClient clientServer = InitServiceClient();
            clientServer.InsertLostRegistryAsync(CurrentModel);
        }
        protected void Update()
        {
            LostRegistryClient clientServer = InitServiceClient();
            clientServer.UpdateLostRegistryAsync(CurrentModel);
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


        #region property....

        private string _lostname;
        public string LostName
        {
            get { return _lostname; }
            set
            {
                _lostname = value == null ? null : value.Trim();
                ValidateLostName(ExtractPropertyName(() => LostName), _lostname);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LostName));
            }
        }
        private void ValidateLostName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(LostName))
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
        private string _lostidcard;
        public string LostIdcard
        {
            get { return _lostidcard; }
            set
            {
                _lostidcard = value == null ? null : value.Trim();
                ValidateLostIdcard(ExtractPropertyName(() => LostIdcard), _lostidcard);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LostIdcard));
            }
        }
        private void ValidateLostIdcard(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(LostIdcard))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
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
            if (string.IsNullOrEmpty(LostPhone))
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
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Address))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private string _losttime;
        public string LostTime
        {
            get { return _losttime; }
            set
            {
                _losttime = value == null ? null : value.Trim();
                ValidateLostTime(ExtractPropertyName(() => LostTime), _losttime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => LostTime));
            }
        }
        private void ValidateLostTime(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(LostTime))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }


         private bool _isTrue;
         public bool IsTrue
        {
            get { return _isTrue; }
            set
            {
                _isTrue = value;               
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsTrue));
            }
        }
  

        #endregion
    }
}


using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
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
namespace Gsafety.PTMS.Manager.ViewModels
{
    public class HeartbeatRuleDetailViewModel : DetailViewModel<HeartbeatRule>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;
        //Gsafety.PTMS.ServiceReference.CommandManageService.CommandManageServiceClient _client = null;
        public HeartbeatRuleDetailViewModel()
        {

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
                        InitialModel = viewParameters["model"] as HeartbeatRule;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as HeartbeatRule;
                        InitialFromInitialModel();
                        CurrentModel = new HeartbeatRule();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new HeartbeatRule();
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

        public void InitialFromInitialModel()
        {
            try
            {
                Name = InitialModel.Name;
                Interval = InitialModel.Interval.ToString();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected void Add()
        {
            try
            {
                CommandManageServiceClient _client = InitialClient();
                _client.InsertHeartbeatRuleAsync(CurrentModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private CommandManageServiceClient InitialClient()
        {
            CommandManageServiceClient _client = ServiceClientFactory.Create<CommandManageServiceClient>();
            _client.InsertHeartbeatRuleCompleted += _client_InsertHeartbeatRuleCompleted;
            _client.UpdateHeartbeatRuleCompleted += _client_UpdateHeartbeatRuleCompleted;
            return _client;
        }

        void _client_UpdateHeartbeatRuleCompleted(object sender, UpdateHeartbeatRuleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {

                CloseClient(sender);
            }
        }

        private void CloseClient(object sender)
        {
            CommandManageServiceClient client = sender as CommandManageServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void _client_InsertHeartbeatRuleCompleted(object sender, InsertHeartbeatRuleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }
        protected void Update()
        {
            try
            {
                CommandManageServiceClient _client = InitialClient();
                _client.UpdateHeartbeatRuleAsync(CurrentModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateInterval(ExtractPropertyName(() => Interval), _interval);

        }

        protected override void OnCommitted()
        {
            try
            {

                CurrentModel.Name = Name;
                CurrentModel.Interval = Convert.ToInt32(Interval);

                if (action.Equals("update"))
                {
                    CurrentModel.Creator = InitialModel.Creator;
                    CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    CurrentModel.Valid = InitialModel.Valid;
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.ClientID = InitialModel.ClientID;
                    Update();
                }
                else
                {
                    CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    CurrentModel.Valid = true;
                    CurrentModel.ID = Guid.NewGuid().ToString();
                    CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                    Add();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Reset()
        {
            if (InitialModel != null)
            {
                InitialFromInitialModel();
            }
            else
            {
                Name = string.Empty;
                Interval = string.Empty;
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
            ValidateRequire(prop, value);
        }
        private string _interval;
        public string Interval
        {
            get { return _interval; }
            set
            {
                _interval = value == null ? null : value.Trim();
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Interval));
            }
        }
        private void ValidateInterval(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (!string.IsNullOrEmpty(value))
            {
                ValidateIntFormat(prop, value);
            }
        }
    }
}


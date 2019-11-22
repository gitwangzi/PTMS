using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
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

namespace Gsafety.PTMS.Traffic.ViewModels
{
    public class SpeedRuleParameterSettingDetailWindowViewModel : DetailViewModel<SpeedLimit>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;

        protected string action;
        public SpeedRuleParameterSettingDetailWindowViewModel()
        {

        }

        private CommandManageServiceClient InitServiceClient()
        {
            CommandManageServiceClient client = ServiceClientFactory.Create<CommandManageServiceClient>();
            client.InsertSpeedLimitCompleted += client_InsertSpeedLimitCompleted;
            client.UpdateSpeedLimitCompleted += client_UpdateSpeedLimitCompleted;
            return client;
        }

        void client_UpdateSpeedLimitCompleted(object sender, UpdateSpeedLimitCompletedEventArgs e)
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
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseClient(client);
            }
        }

        void client_InsertSpeedLimitCompleted(object sender, InsertSpeedLimitCompletedEventArgs e)
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
                CommandManageServiceClient client = sender as CommandManageServiceClient;
                CloseClient(client);
            }
        }

        private void CloseClient(CommandManageServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
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
                    ViewVisibility = Visibility.Collapsed;
                    InitialModel = viewParameters["view"] as SpeedLimit;
                    InitialFromInitialModel();
                    break;
                case "update":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                    IsReadOnly = false;
                    ViewVisibility = Visibility.Visible;
                    InitialModel = viewParameters["view"] as SpeedLimit;
                    InitialFromInitialModel();
                    CurrentModel = InitialModel;
                    break;
                case "add":
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                    IsReadOnly = false;
                    ViewVisibility = Visibility.Visible;
                    CurrentModel = new SpeedLimit();
                    Reset();
                    break;
                default:
                    break;
            }
        }

        protected override void Reset()
        {
            Name = string.Empty;
            MaxSpeed = string.Empty;
            Duration = string.Empty;
        }

        public void InitialFromInitialModel()
        {
            Name = InitialModel.Name;
            MaxSpeed = InitialModel.MaxSpeed.ToString();
            Duration = InitialModel.Duration.ToString();
            //Valid = InitialModel.Valid;
        }
        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            ValidateMaxSpeed(ExtractPropertyName(() => MaxSpeed), _maxspeed);
            ValidateDuration(ExtractPropertyName(() => Duration), _duration);
        }
        protected override void OnCommitted()
        {
            CurrentModel.Name = Name;
            CurrentModel.MaxSpeed = Convert.ToDecimal(MaxSpeed);
            CurrentModel.Duration = Convert.ToDecimal(Duration);

            //CurrentModel.Valid = Valid;
            if (action.Equals("update"))
            {
                Update();
            }
            else
            {
                CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                CurrentModel.CreateTime = DateTime.UtcNow;
                CurrentModel.Valid = true;
                Add();
            }
        }

        protected override void Return()
        {

        }

        protected void Add()
        {
            CommandManageServiceClient client = InitServiceClient();
            client.InsertSpeedLimitAsync(CurrentModel);
        }

        protected void Update()
        {
            CommandManageServiceClient client = InitServiceClient();
            client.UpdateSpeedLimitAsync(CurrentModel);
        }

        #region Field and Validate


        #region Filed

        string title = "Title";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        #endregion

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
        private string _maxspeed;
        public string MaxSpeed
        {
            get { return _maxspeed; }
            set
            {
                _maxspeed = value == null ? null : value.Trim();
                ValidateMaxSpeed(ExtractPropertyName(() => MaxSpeed), _maxspeed);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaxSpeed));
            }
        }
        private void ValidateMaxSpeed(string prop, string value)
        {
            ClearErrors(prop);

            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                return;
            }

            int intvalue = 0;
            if (!int.TryParse(value, out intvalue))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitSpeed"));

                return;
            }

            if (intvalue <= 0 || intvalue >= 200)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitSpeed"));
                return;
            }
        }

        private string _duration;
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value == null ? null : value.Trim();
                ValidateDuration(ExtractPropertyName(() => Duration), _duration);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Duration));
            }
        }
        private void ValidateDuration(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                return;
            }

            int intvalue = 0;
            if (!int.TryParse(value, out intvalue))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitTime"));

                return;
            }

            if (intvalue <= 0 || intvalue >= 255)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("LimitTime"));
            }

        }

        #endregion

    }
}

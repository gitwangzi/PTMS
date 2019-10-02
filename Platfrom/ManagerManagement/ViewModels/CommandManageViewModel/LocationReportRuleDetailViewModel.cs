using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
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
namespace Gsafety.PTMS.Manager.ViewModels
{
    public class LocationReportRuleDetailViewModel : DetailViewModel<LocationReportRule>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;
        public LocationReportRuleDetailViewModel()
        {
            try
            {
                var temp = new EnumAdapter<ReportStrategyEnum>().GetEnumInfos();


                ReportStrategies = new List<NameValueModel<int>>();

                foreach (var item in temp)
                {
                    ReportStrategies.Add(new NameValueModel<int>
                    {
                        Value = (short)item.Value,
                        Name = item.LocalizedString
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallPlaceDetailViewModel.client_AddInstallStationCompleted", ex);
            }
        }

        public List<NameValueModel<int>> ReportStrategies
        {
            private set;
            get;
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
                        InitialModel = viewParameters["model"] as LocationReportRule;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as LocationReportRule;
                        InitialFromInitialModel();
                        CurrentModel = new LocationReportRule();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new LocationReportRule();
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
                Name = string.Empty;
                Interval = string.Empty;
                Length = string.Empty;
                ReportStrategy = 0;
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
            Name = InitialModel.Name;
            ReportStrategy = InitialModel.ReportStrategy;
            Interval = InitialModel.Interval.ToString();
            Length = InitialModel.Length.ToString();
        }


        public void ValidProperty()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            if (ReportStrategy == 0)
            {
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
            }
            else if (ReportStrategy == 1)
            {
                ValidateLength(ExtractPropertyName(() => Length), _length);
            }
            else if (ReportStrategy == 2)
            {
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
                ValidateLength(ExtractPropertyName(() => Length), _length);
            }
            ValidateLength(ExtractPropertyName(() => Length), _length);
        }

        protected override void ValidateAll()
        {
            ValidateName(ExtractPropertyName(() => Name), _name);
            if (ReportStrategy == 0)
            {
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
            }
            else if (ReportStrategy == 1)
            {
                ValidateLength(ExtractPropertyName(() => Length), _length);
            }
            else if (ReportStrategy == 2)
            {
                ValidateInterval(ExtractPropertyName(() => Interval), _interval);
                ValidateLength(ExtractPropertyName(() => Length), _length);
            }
            ValidateLength(ExtractPropertyName(() => Length), _length);
        }

        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.Name = Name;
                CurrentModel.ReportStrategy = Convert.ToInt32(ReportStrategy);
                if (ReportStrategy == 0)
                {
                    CurrentModel.Interval = Convert.ToInt32(Interval);
                }
                else if (ReportStrategy == 1)
                {
                    CurrentModel.Length = Convert.ToInt32(Length);
                }
                else if (ReportStrategy == 2)
                {
                    CurrentModel.Interval = Convert.ToInt32(Interval);
                    CurrentModel.Length = Convert.ToInt32(Length);
                }

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
                    CurrentModel.Valid = 1;
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

        private CommandManageServiceClient InitialClient()
        {
            CommandManageServiceClient _client = ServiceClientFactory.Create<CommandManageServiceClient>();
            _client.InsertLocationReportRuleCompleted += _client_InsertLocationReportRuleCompleted;
            _client.UpdateLocationReportRuleCompleted += _client_UpdateLocationReportRuleCompleted;
            return _client;
        }

        void _client_UpdateLocationReportRuleCompleted(object sender, UpdateLocationReportRuleCompletedEventArgs e)
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
            CommandManageServiceClient _client = sender as CommandManageServiceClient;
            if (_client != null)
            {
                _client.CloseAsync();
                _client = null;
            }
        }

        void _client_InsertLocationReportRuleCompleted(object sender, InsertLocationReportRuleCompletedEventArgs e)
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

        protected void Add()
        {
            try
            {
                CommandManageServiceClient _client = InitialClient();
                _client.InsertLocationReportRuleAsync(CurrentModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected void Update()
        {
            try
            {
                CommandManageServiceClient _client = InitialClient();
                _client.UpdateLocationReportRuleAsync(CurrentModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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
        private int _reportstrategy;
        public int ReportStrategy
        {
            get { return _reportstrategy; }
            set
            {
                _reportstrategy = value;
                if (_reportstrategy == 0)
                {
                    IsEnable = true;
                    IsTrue = false;
                    Length = string.Empty;
                }
                if (_reportstrategy == 1)
                {
                    IsEnable = false;
                    IsTrue = true;
                    Interval = string.Empty;
                }
                if (_reportstrategy == 2)
                {
                    IsEnable = true;
                    IsTrue = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ReportStrategy));
            }
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
            ClearErrors(prop);
            if (ReportStrategy == 0 || ReportStrategy == 2)
            {
                ValidateRequire(prop, value);

                if (!string.IsNullOrEmpty(value))
                {
                    ValidateLongFormat(prop, value);
                    long result;
                    if (long.TryParse(value, out result)&&(long.Parse(value) <= 0 || long.Parse(value) > 65535))
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(ApplicationContext.Instance.StringResourceReader.GetString("IntervalFormat")));
                }
            }
        }
        private string _length;
        public string Length
        {
            get { return _length; }
            set
            {
                _length = value == null ? null : value.Trim();
                ValidateLength(ExtractPropertyName(() => Length), _length);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Length));
            }
        }
        private void ValidateLength(string prop, string value)
        {
            ClearErrors(prop);
            if (ReportStrategy == 1 || ReportStrategy == 2)
            {
                ValidateRequire(prop, value);

                if (!string.IsNullOrEmpty(value))
                {
                    ValidateLongFormat(prop, value);
                    long result;
                    if (long.TryParse(value, out result)&&(long.Parse(value) <= 0 ||  long.Parse(value) > 65535))
                        base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(ApplicationContext.Instance.StringResourceReader.GetString("DistanceFormat")));
                }
            }
        }
    }
}


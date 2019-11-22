using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.MaintainService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using System;
using System.Reflection;
using System.Windows;
namespace Gsafety.PTMS.Installation.ViewModels
{
    public class MaintainRecordUnfinishedDetailViewModel : DetailViewModel<MaintainRecordUnfinished>
    {
        //MaintainRecordClient client = null;

        public event EventHandler<SaveResultArgs> OnSaveResult;

        #region method.....

        public MaintainRecordUnfinishedDetailViewModel()
        {
            // client = InitialClient();
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
                        InitialModel = viewParameters["model"] as MaintainRecordUnfinished;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as MaintainRecordUnfinished;
                        InitialFromInitialModel();
                        CurrentModel = new MaintainRecordUnfinished();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new MaintainRecordUnfinished();
                        Reset();
                        break;
                    default:
                        break;
                }
                if (action == "view")
                {
                    if (InitialModel.StartTime == null)
                    {
                        StartTime = null;
                    }

                }
                else
                {
                    if (InitialModel.StartTime == null)
                        StartTime = ScheduleDate == null ? DateTime.Now : (DateTime)ScheduleDate;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private MaintainRecordClient InitialClient()
        {
            MaintainRecordClient _client = ServiceClientFactory.Create<MaintainRecordClient>();
            _client.InsertMaintainRecordCompleted += _client_InsertMaintainRecordCompleted;
            // _client.GetMaintainRecordUnfinishedListCompleted += _client_GetMaintainRecordUnfinishedListCompleted;
            return _client;
        }

        private void _client_InsertMaintainRecordCompleted(object sender, InsertMaintainRecordCompletedEventArgs e)
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
                // client.AddInstallStationCompleted -= client_AddInstallStationCompleted;
                CloseClient(sender);
            }
        }

        private void CloseClient(object sender)
        {
            MaintainRecordClient client = sender as MaintainRecordClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        protected override void Reset()
        {
            if (!action.Equals("update"))
            {
                VehcileID = string.Empty;
                Applicant = string.Empty;
                Contact = string.Empty;
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
                VehcileID = InitialModel.VehcileID;
                Applicant = InitialModel.Applicant;
                Work = InitialModel.Worker;
                Contact = InitialModel.Contact;
                Problem = InitialModel.Problem;
                if (InitialModel.ScheduleDate != null)
                {
                    ScheduleDate = InitialModel.ScheduleDate.Value;
                    
                }
                if (InitialModel.StartTime != null)
                {
                    IsChecked = true;
                    StartTime = ((DateTime)InitialModel.StartTime);
                }
                else
                {
                    StartTime = ScheduleDate == null ? DateTime.Now : (DateTime)ScheduleDate;
                    
                }

                if (InitialModel.EndTime != null)
                {
                    EndTime = InitialModel.EndTime.Value;
                    SaveButtonVisibility = Visibility.Collapsed;
                    ResertButtonVisibility = Visibility.Collapsed;
                }
                else
                {
                    EndTime = InitialModel.EndTime;
                }

                //StartTime = StartTime;
                //ScheduleDate = ScheduleDate;
                //EndTime = EndTime;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        protected override void ValidateAll()
        {

        }
        protected override void OnCommitted()
        {
            if (action.Equals("update"))
            {
                Update();
            }
            else
            {
                Add();
            }
        }
        protected override void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs());
        }
        protected void Add()
        {

        }
        protected void Update()
        {
            try
            {
                MaintainRecord mr = new MaintainRecord();
                mr.ID = Guid.NewGuid().ToString();
                if (StartTime != null)
                    mr.StartTime = (DateTime)StartTime.Value.ToUniversalTime();
                if (EndTime.HasValue)
                    mr.EndTime = EndTime.Value.ToUniversalTime();
                mr.ApplicationID = InitialModel.ApplicationID;
                mr.CreateTime = DateTime.Now.ToUniversalTime();
                mr.SetupStation = InitialModel.SetupStation;
                mr.Status = EndTime == null ? 1 : 2;
                MaintainRecordClient client = InitialClient();
                client.InsertMaintainRecordAsync(mr);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #endregion

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


        public bool _isChecked;
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return this._isChecked;
            }
            set
            {
                this._isChecked = value;
                RaisePropertyChanged(() => this.IsChecked);
            }
        }

        #endregion

        #region Visible.....
        private string _vehcileid;
        public string VehcileID
        {
            get { return _vehcileid; }
            set
            {
                _vehcileid = value == null ? null : value.Trim();

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehcileID));
            }
        }

        private string _applicant;
        public string Applicant
        {
            get { return _applicant; }
            set
            {
                _applicant = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Applicant));
            }
        }

        private string _work;
        public string Work
        {
            get { return _work; }
            set
            {
                _work = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Work));
            }
        }

        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            }
        }


        private string _applicationstatus;
        public string ApplicationStatus
        {
            get { return _applicationstatus; }
            set
            {
                _applicationstatus = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ApplicationStatus));
            }
        }

        private string _problem;
        public string Problem
        {
            get { return _problem; }
            set
            {
                _problem = value == null ? null : value.Trim();
                ValidateProblem(ExtractPropertyName(() => Problem), _problem);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Problem));
            }
        }
        private void ValidateProblem(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Problem))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                ValidateStartTime(ExtractPropertyName(() => StartTime), _startTime);
                ValidateBeginAndEndDate(ExtractPropertyName(() => StartTime), StartTime, ExtractPropertyName(() => EndTime), EndTime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => StartTime));
            }
        }
        private void ValidateStartTime(string prop, DateTime? value)
        {
            ClearErrors(prop);
            if (StartTime == null)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            if (StartTime < InitialModel.ScheduleDate)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }


        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                ValidateBeginAndEndDate(ExtractPropertyName(() => StartTime), StartTime, ExtractPropertyName(() => EndTime), EndTime);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EndTime));
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

        private DateTime? _scheduleDate;
        public DateTime? ScheduleDate
        {
            get { return _scheduleDate; }
            set
            {
                _scheduleDate = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ScheduleDate));
            }
        }



        #endregion



    }
}


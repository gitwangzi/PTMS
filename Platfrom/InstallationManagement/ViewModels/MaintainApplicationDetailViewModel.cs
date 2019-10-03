using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.MaintainService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Gsafety.PTMS.Installation.ViewModels
{
    public class MaintainApplicationDetailViewModel : DetailViewModel<MaintainApplication>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;
        //Gsafety.PTMS.ServiceReference.MaintainService.MaintainApplicationClient _client = null;
        public MaintainApplicationDetailViewModel()
        {
            InitialClient();

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
                        InitialModel = viewParameters["model"] as MaintainApplication;
                        InitInstallStation();
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["model"] as MaintainApplication;
                        InitInstallStation();
                        InitialFromInitialModel();
                        CurrentModel = new MaintainApplication();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        ViewVisibility = Visibility.Visible;
                        InitInstallStationClickAdd();
                        CurrentModel = new MaintainApplication();

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

            try
            {
                if (InitialModel != null)
                {
                    ScheduleDate = null;
                    InitialFromInitialModel();
                }
                else
                {
                    Applicant = string.Empty;
                    Note = string.Empty;
                    Problem = string.Empty;
                    Contact = string.Empty;
                    VehicleID = string.Empty;
                    if (IsChecked == true)
                    {
                        Worker = string.Empty;
                        WorkerPhone = string.Empty;
                        ScheduleDate = DateTime.Now;
                        VInstallStation = ZInstallStation.FirstOrDefault();
                    }
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
                Applicant = InitialModel.Applicant;

                if (InitialModel.Status == 2)
                {
                    IsChecked = true;
                }
                else { IsChecked = false; }
                Note = InitialModel.Note;
                Worker = InitialModel.Worker;
                WorkerPhone = InitialModel.WorkerPhone;
                if (InitialModel.ScheduleDate.HasValue)
                    ScheduleDate = InitialModel.ScheduleDate.Value.ToLocalTime();
                Problem = InitialModel.Problem;
                Contact = InitialModel.Contact;
                VehicleID = InitialModel.VehicleID;
                VInstallStation = ZInstallStation.FirstOrDefault(t => t.Key.ID == InitialModel.SetupStation);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void ValidateAll()
        {
            ValidateApplicant(ExtractPropertyName(() => Applicant), _applicant);
            ValidateProblem(ExtractPropertyName(() => Problem), _problem);
            ValidateContact(ExtractPropertyName(() => Contact), _contact);
            ValidateVehicleID(ExtractPropertyName(() => VehicleID), _vehicleid);
            if (IsChecked == true)
            {
                ValidateScheduleDate(ExtractPropertyName(() => ScheduleDate), _scheduledate);
            }

        }
        protected override void OnCommitted()
        {
            try
            {
                CurrentModel.Applicant = Applicant;
                CurrentModel.VehicleID = VehicleID;
                CurrentModel.Contact = Contact;
                CurrentModel.Problem = Problem;
                if (IsChecked)
                {
                    CurrentModel.ScheduleDate = ScheduleDate.Value.ToUniversalTime();
                    CurrentModel.SetupStation = VInstallStation == null ? null : VInstallStation.Key.ID;
                    CurrentModel.Worker = Worker;
                    CurrentModel.WorkerPhone = WorkerPhone;
                    CurrentModel.Note = Note;
                    CurrentModel.Status = 2;
                }
                else
                {
                    CurrentModel.Status = 0;
                }

                if (action.Equals("update"))
                {
                    CurrentModel.ID = InitialModel.ID;
                    CurrentModel.ClientID = InitialModel.ClientID;
                    CurrentModel.Creator = InitialModel.Creator;
                    CurrentModel.CreateTime = InitialModel.CreateTime.ToUniversalTime();
                    Update();
                }
                else
                {
                    CurrentModel.ID = Guid.NewGuid().ToString();
                    CurrentModel.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                    CurrentModel.Creator = ApplicationContext.Instance.AuthenticationInfo.Account;
                    CurrentModel.CreateTime = DateTime.Now.ToUniversalTime();
                    Add();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected void Add()
        {
            MaintainApplicationClient _client = InitialClient();
            _client.InsertMaintainApplicationAsync(CurrentModel);
        }

        private MaintainApplicationClient InitialClient()
        {
            MaintainApplicationClient _client = ServiceClientFactory.Create<MaintainApplicationClient>();
            _client.InsertMaintainApplicationCompleted += _client_InsertMaintainApplicationCompleted;
            _client.UpdateMaintainApplicationCompleted += _client_UpdateMaintainApplicationCompleted;
            _client.GetInstallStationListCompleted += _client_GetInstallStationListCompleted;
            return _client;
        }

        private void _client_GetInstallStationListCompleted(object sender, GetInstallStationListCompletedEventArgs e)
        {
            try
            {
                ZInstallStation = new List<ComboBoxBasicStruct<InstallStation>>();
                foreach (var item in e.Result.Result)
                {
                    ComboBoxBasicStruct<InstallStation> cb = new ComboBoxBasicStruct<InstallStation>();
                    cb.Key = item;
                    cb.Value = item.Name;
                    ZInstallStation.Add(cb);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ZInstallStation));
                VInstallStation = ZInstallStation.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void _client_UpdateMaintainApplicationCompleted(object sender, UpdateMaintainApplicationCompletedEventArgs e)
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
            MaintainApplicationClient _client = sender as MaintainApplicationClient;
            if (_client != null)
            {
                _client.CloseAsync();
                _client = null;
            }
        }

        void _client_InsertMaintainApplicationCompleted(object sender, InsertMaintainApplicationCompletedEventArgs e)
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

        protected void Update()
        {
            try
            {
                MaintainApplicationClient _client = InitialClient();
                _client.UpdateMaintainApplicationAsync(CurrentModel);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void InitInstallStation()
        {
            try
            {
                List<ComboBoxBasicStruct<InstallStation>> listIn = new List<ComboBoxBasicStruct<InstallStation>>();
                var zI = InitialModel.ZInstallStation;
                foreach (var z in zI)
                {
                    ComboBoxBasicStruct<InstallStation> cb = new ComboBoxBasicStruct<InstallStation>();
                    InstallStation insta = new InstallStation();
                    insta.ID = z.Key;
                    cb.Key = insta;
                    cb.Value = z.Value;
                    listIn.Add(cb);
                }
                ZInstallStation = listIn;
                VInstallStation = ZInstallStation.FirstOrDefault(x => x.Key.ID == InitialModel.SetupStation);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void InitInstallStationClickAdd()
        {
            try
            {
                MaintainApplicationClient _client = InitialClient();
                _client.GetInstallStationListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }



        private ComboBoxBasicStruct<InstallStation> vInstallStation; //=new ComboBoxBasicStruct<InstallStation>();
        /// <summary>
        /// 选中的安装点
        /// </summary>
        public ComboBoxBasicStruct<InstallStation> VInstallStation
        {
            get { return vInstallStation; }
            set
            {
                vInstallStation = value;
                RaisePropertyChanged(() => VInstallStation);
            }
        }

        /// <summary>
        /// 安装点
        /// </summary>
        private List<ComboBoxBasicStruct<InstallStation>> _installStation; //= new List<ComboBoxBasicStruct<InstallStation>>();

        /// <summary>
        /// 
        /// </summary>
        public List<ComboBoxBasicStruct<InstallStation>> ZInstallStation
        {
            get { return _installStation; }
            set
            {
                _installStation = value;
                RaisePropertyChanged(() => ZInstallStation);
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                this._isChecked = value;
                RaisePropertyChanged(() => this.IsChecked);
                ValidateScheduleDate(ExtractPropertyName(() => ScheduleDate), _scheduledate);
                ValidateWorker(ExtractPropertyName(() => Worker), _worker);
                ValidateWorkerPhone(ExtractPropertyName(() => WorkerPhone), _workerphone);
            }
        }

        #region Valid.....


        private string _worker;
        public string Worker
        {
            get { return _worker; }
            set
            {
                _worker = value == null ? null : value.Trim();
                ValidateWorker(ExtractPropertyName(() => Worker), _worker);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Worker));
            }
        }
        private void ValidateWorker(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Worker))
            {
                if (IsChecked == true)
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }



        private string _workerphone;
        public string WorkerPhone
        {
            get { return _workerphone; }
            set
            {
                _workerphone = value == null ? null : value.Trim();
                ValidateWorkerPhone(ExtractPropertyName(() => WorkerPhone), _workerphone);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => WorkerPhone));
            }
        }

        private void ValidateWorkerPhone(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(WorkerPhone))
            {
                //base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
                if (value.Length > 20)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
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
            else
            {
                if (IsChecked == true)
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }


        private DateTime? _scheduledate;
        public DateTime? ScheduleDate
        {
            get { return _scheduledate; }
            set
            {
                _scheduledate = value;
                ValidateScheduleDate(ExtractPropertyName(() => ScheduleDate), _scheduledate);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ScheduleDate));
            }
        }
        private void ValidateScheduleDate(string prop, DateTime? value)
        {
            ClearErrors(prop);
            if (!value.HasValue)
            {
                if (IsChecked == true)
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
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

        private string _applicant;
        public string Applicant
        {
            get { return _applicant; }
            set
            {
                _applicant = value == null ? null : value.Trim();
                ValidateApplicant(ExtractPropertyName(() => Applicant), _applicant);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Applicant));
            }
        }
        private void ValidateApplicant(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Applicant))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }




        private string _contact;
        public string Contact
        {
            get { return _contact; }
            set
            {
                _contact = value == null ? null : value.Trim();
                ValidateContact(ExtractPropertyName(() => Contact), _contact);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Contact));
            }
        }
        private void ValidateContact(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(Contact))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {

                if (value.Length > 20)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
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
        }

        private string _vehicleid;
        public string VehicleID
        {
            get { return _vehicleid; }
            set
            {
                _vehicleid = value == null ? null : value.Trim();
                ValidateVehicleID(ExtractPropertyName(() => VehicleID), _vehicleid);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleID));
            }
        }
        private void ValidateVehicleID(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(VehicleID))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
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

        #endregion


    }
}


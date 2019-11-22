using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.ServiceReference.MaintainService;
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
namespace Gsafety.PTMS.Installation.ViewModels
{
    public class MaintainRecordDetailViewModel : DetailViewModel<MaintainRecord>
    {
        public event EventHandler<SaveResultArgs> OnSaveResult;
        public MaintainRecordDetailViewModel()
        {
        }
        public new  void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
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
                        InitialModel = viewParameters["view"] as MaintainRecord;
                        InitialFromInitialModel();
                        break;
                    case "update":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Update");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        InitialModel = viewParameters["view"] as MaintainRecord;
                        InitialFromInitialModel();
                        CurrentModel = new MaintainRecord();
                        break;
                    case "add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("Common_Add");
                        IsReadOnly = false;
                        IsEnable = true;
                        ViewVisibility = Visibility.Visible;
                        CurrentModel = new MaintainRecord();
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
           
        }
        public void InitialFromInitialModel()
        {
            //ID = InitialModel.ID;
            try
            {

                Worker = InitialModel.Worker;
                Note = InitialModel.Note;
                VehicleID = InitialModel.VehicleID;
                SetupStation = InitialModel.SetupStation;
                StartTime = InitialModel.StartTime.ToLocalTime();
                EndTime = InitialModel.EndTime.Value.ToLocalTime();
                CreateTime = InitialModel.CreateTime.ToLocalTime().ToString();
                ScheduleDate = InitialModel.ScheduleDate.ToLocalTime();
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
        }


        private string _vehicleID;
        public string VehicleID
        {
            get { return _vehicleID; }
            set
            {
                _vehicleID = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleID));
            }
        }

        private string _setupStation;
        public string SetupStation
        {
            get { return _setupStation; }
            set
            {
                _setupStation = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupStation));
            }
        }

        private DateTime _scheduleDate;
        public DateTime ScheduleDate
        {
            get { return _scheduleDate; }
            set
            {
                _scheduleDate = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ScheduleDate));
            }
        }


    
        private string _worker;
        public string Worker
        {
            get { return _worker; }
            set
            {
                _worker = value == null ? null : value.Trim();
                
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Worker));
            }
        }
        
        private string _createtime;
        public string CreateTime
        {
            get { return _createtime; }
            set
            {
                _createtime = value == null ? null : value.Trim();               
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CreateTime));
            }
        }




        private DateTime? _starttime;
        public DateTime? StartTime
        {
            get { return _starttime; }
            set
            {
                _starttime = value;
               
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => StartTime));
            }
        }
       

        private DateTime? _endtime;
        public DateTime? EndTime
        {
            get { return _endtime; }
            set
            {
                _endtime = value;
               
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

    }
}


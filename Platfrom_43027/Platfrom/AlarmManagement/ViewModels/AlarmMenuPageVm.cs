/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dd8136a9-6edb-4054-b494-302a53d2ff55      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.ViewModels
/////    Project Description:    
/////             Class Name: AlarmMainPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 8/6/2013 6:00:00 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/6/2013 6:00:00 PM
/////            Modified by:wzs
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Video.Args;
using Jounce.Core.ViewModel;
using Gsafety.PTMS.ServiceReference;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Jounce.Framework.Command;
using Gsafety.PTMS.Share;
using System.Windows.Data;
using Jounce.Framework;
using Jounce.Core.View;
using Jounce.Core.Event;
using System.Windows.Browser;
using Gsafety.PTMS.BasicPage.VideoDisplay;
using GisManagement.Models;
using Gsafety.Common.CommMessage;
using GisManagement.ViewModels;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Alarm.Views;
using Gsafety.PTMS.Alarm;
using Gsafety.PTMS.BasicPage.Monitor.Views;
using System.Reflection;
namespace Gsafety.PTMS.Alarm.ViewModels
{
    [ExportAsViewModel(AlarmName.AlarmMenuViewModel)]
    public class AlarmMenuPageVm : BaseViewModel
    {
        #region Fields
        private VehicleAlarmServiceClient vehicleAlarmServiceClient = null;
        private VedioServiceClient vedioServiceClient = null;
        private int CurrentAccordionSelectIndex = 0;
        private AlarmInfoEx _CurrentUnHandedAlarmInfo;
        private AlarmInfoEx _CurrentHandedAlarmInfo;
        private int _LockFlag = 0;

        public Visibility MenuShow
        {
            get
            {
                if (ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals("SecurityMonitor"))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
        }

        public Visibility DealMenuShow
        {
            get
            {
                if (ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals("SecurityAdmin") || ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals("AlarmFilterCommissioner")
                    || ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals("SecurityManager"))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        private short? _IsTrueAlarm;
        public short? IsTrueAlarm
        {
            get
            {
                return _IsTrueAlarm;
            }
            set
            {
                _IsTrueAlarm = value;
            }
        }

        private string _selecthandled;
        public string selecthandled
        {
            get
            {
                return _selecthandled;
            }
            set
            {
                _selecthandled = value;
                if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("ALARM_False"))
                {
                    IsTrueAlarm = 0;
                }
                else if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("ALARM_True"))
                {
                    IsTrueAlarm = 1;
                }
                else if (_selecthandled == ApplicationContext.Instance.StringResourceReader.GetString("All"))
                {
                    IsTrueAlarm = 2;
                }
                else
                {
                    IsTrueAlarm = 0;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => selecthandled));
            }
        }

        public List<string> ishandled { get; set; }

        public int LockFlag
        {
            get { return _LockFlag; }
            set { _LockFlag = value; }
        }
        private bool _Lockpage = false;

        public bool Lockpage
        {
            get { return _Lockpage; }
            set
            {
            //    _Lockpage = value;
            //    if (Lockpage)
            //    {
            //        ApplicationContext.Instance.BufferManager.AlarmManager.IsLock = true;
            //    }
            //    else
            //    {
            //        ApplicationContext.Instance.BufferManager.AlarmManager.IsLock = false;
            //    }
            }
        }
        /// <summary>
        /// Has tracked the current alarm information
        /// </summary>
        private AlarmInfoEx _CurrentTraceVehicle;

        private string _HandleFiledCarNumber;

        /// <summary>
        /// License plate number
        /// </summary>
        private string _UnHandleFiledCarNumber;




        private Nullable<DateTime> _StartTime;

        private Nullable<DateTime> _EndTime;

        private Nullable<DateTime> _UnhandlerStartTime;


        private Nullable<DateTime> _UnhandlerEndTime;

        /// <summary>
        /// Whether true alarm
        /// </summary>
        private bool _isHandledBusy;


        private bool _IsUnhandledBusy;

        PagedCollectionView _UnHandledAlarmPagedCV;

        #endregion

        #region Attributes
        /// <summary>
        /// Start time
        /// </summary>
        public Nullable<DateTime> StartTime
        {
            get { return _StartTime; }
            set
            {
                _StartTime = value;
            }
        }
        /// <summary>
        /// End Time
        /// </summary>
        public Nullable<DateTime> EndTime
        {
            get { return _EndTime; }
            set
            {
                if (value != null)
                {
                    _EndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }
            }
        }
        /// <summary>
        /// UnhandlerAlarmTime
        /// </summary>
        public Nullable<DateTime> UnhandlerStartTime
        {
            get { return _UnhandlerStartTime; }
            set
            {
                _UnhandlerStartTime = value;
            }
        }
        /// <summary>
        /// End Time
        /// </summary>
        public Nullable<DateTime> UnhandlerEndTime
        {
            get { return _UnhandlerEndTime; }
            set
            {
                if (value != null)
                {
                    _UnhandlerEndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }

            }
        }
        public bool IsHandledBusy
        {
            get
            {
                return this._isHandledBusy;
            }
            set
            {
                this._isHandledBusy = value;
                RaisePropertyChanged(() => this.IsHandledBusy);
            }
        }
        public bool IsUnhandledBusy
        {
            get
            {
                return this._IsUnhandledBusy;
            }
            set
            {
                this._IsUnhandledBusy = value;
                RaisePropertyChanged(() => this.IsUnhandledBusy);
            }
        }
        public string BusyContent
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("ALARM_PleaseWait");
            }
        }
        public PagedCollectionView UnHandledAlarmPagedCV
        {
            get { return _UnHandledAlarmPagedCV; }
        }
        /// <summary>
        /// Processed license plate number
        /// </summary>
        public string HandleFiledCarNumber
        {
            get { return _HandleFiledCarNumber; }
            set
            {
                _HandleFiledCarNumber = value.Trim();
            }
        }
        public string UnHandleFiledCarNumber
        {
            get { return _UnHandleFiledCarNumber; }
            set
            {
                _UnHandleFiledCarNumber = value.Trim();
            }
        }
        #endregion

        #region Command
        /// <summary>
        /// Open the detail page of the switch
        /// </summary>
        public ICommand OpenDetailViewCommand { get; private set; }
        /// <summary>
        /// Lock pages reserved for event
        /// </summary>
        public ICommand LockAlarminfo { get; private set; }
        public ICommand GroupSelectCommand { get; private set; }
        public ICommand GroupHandledSelectCommand { get; private set; }
        ////Untreated a key alarm details
        /// <summary>
        /// Untreated interface
        /// </summary>
        public ICommand GetUnhandleAlarmCommand { get; private set; }
        public ICommand HappenLoctionCommand { get; private set; }
        public ICommand TraceVehicleAlarmCommand { get; private set; }
        public ICommand UnHandedAlarmLocateCommand { get; private set; }
        public ICommand UnHandedAlarmVedio1Command { get; private set; }
        public ICommand UnHandedAlarmVedio2Command { get; private set; }
        public ICommand UnHandedAlarmVedio3Command { get; private set; }
        public ICommand UnHandedAlarmVedio4Command { get; private set; }
        public ICommand UnHandedAlarmVedio1First15SCommand { get; private set; }
        public ICommand UnHistoricalRouteCommand { get; private set; }
        ////A key alarm processing details
        public ICommand GetHandleAlarmCommand { get; private set; }
        public ICommand HandedAlarmVedio1Command { get; private set; }
        public ICommand HandedAlarmVedio2Command { get; private set; }
        public ICommand HandedAlarmVedio3Command { get; private set; }
        public ICommand HandedAlarmVedio4Command { get; private set; }
        public ICommand HandedAlarmVedio1First15SCommand { get; private set; }
        public ICommand HistoricalRouteCommand { get; private set; }
        public ICommand DownloadVideoCommand { get; private set; }
        #endregion

        #region Entity
        ////Select the object row
        public AlarmInfoEx CurrentUnHandedAlarmInfo
        {
            get
            {

                return _CurrentUnHandedAlarmInfo;
            }
            set
            {
                _CurrentUnHandedAlarmInfo = value;
                if (CurrentAccordionSelectIndex == 0 && _CurrentUnHandedAlarmInfo != null)
                {
                    AlarmInfoDisplay displayinfo = new AlarmInfoDisplay();
                    displayinfo.DisPlayInfo = CurrentUnHandedAlarmInfo;
                    EventAggregator.Publish<AlarmInfoDisplay>(displayinfo);
                }

            }
        }
        /// <summary>
        /// The current object has been processed
        /// </summary>
        public AlarmInfoEx CurrentHandedAlarmInfo
        {
            get
            {

                return _CurrentHandedAlarmInfo;
            }
            set
            {
                _CurrentHandedAlarmInfo = value;
                if (CurrentAccordionSelectIndex == 1)
                {
                    if (CurrentHandedAlarmInfo != null)
                    {
                        AlarmInfoDisplay displayinfo = new AlarmInfoDisplay();
                        displayinfo.DisPlayInfo = CurrentHandedAlarmInfo;
                        EventAggregator.Publish<AlarmInfoDisplay>(displayinfo);
                    }

                }
            }
        }

        private ObservableCollection<AlarmInfoEx> _handleAlarms;

        public ObservableCollection<AlarmInfoEx> HandleAlarms
        {
            get { return this._handleAlarms; }
            set
            {
                _handleAlarms = value;
                RaisePropertyChanged(() => this.HandleAlarms);
            }
        }
        #endregion

        #region Pager
        ////A list of the key alarm paging parameters that have been processed
        public int HandleAlarmsPageIndex
        {
            get;
            set;
        }
        public int HandleAlarmsItemCount
        {
            get;
            set;
        }
        private PagedCollectionView _handleAlarmsPageSizeValueDatePager;
        public PagedCollectionView HandleAlarmsDataPager
        {
            get
            {
                return this._handleAlarmsPageSizeValueDatePager;
            }
            set
            {
                this._handleAlarmsPageSizeValueDatePager = value;
                RaisePropertyChanged(() => this.HandleAlarmsDataPager);
            }
        }
        private int _handleAlarmsPageSizeValue = 20;
        public int HandleAlarmsPageSizeValue
        {
            get
            {
                return this._handleAlarmsPageSizeValue;
            }
            set
            {
                this._handleAlarmsPageSizeValue = value;
                RaisePropertyChanged(() => this.HandleAlarmsPageSizeValue);
            }
        }
        #endregion

        #region Init

        public AlarmMenuPageVm()
        {
            try
            {
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                vedioServiceClient = ServiceClientFactory.Create<VedioServiceClient>();

                ////Get a key alarm service
                vehicleAlarmServiceClient.GetHandledAlarmsCompleted += vehicleAlarmServiceClient_GetHandledAlarmsCompleted;
                vedioServiceClient.GetAlarmFiftyVideoAppealCompleted += vedioServiceClient_GetAlarmFiftyVideoAppealCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmMenuPageVm()", ex);
            }

            _UnHandledAlarmPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo);

            selecthandled = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_True");

            ishandled = new List<string>
           {
               ApplicationContext.Instance.StringResourceReader.GetString("All"),
               ApplicationContext.Instance.StringResourceReader.GetString("ALARM_True"),
               ApplicationContext.Instance.StringResourceReader.GetString("ALARM_False"),
           };
            GroupSelectCommand = new ActionCommand<object>(obj => GroupSelectCommand_Event(obj));

            GroupHandledSelectCommand = new ActionCommand<object>(obj => GroupHandledSelectCommand_Event(obj));
            LockAlarminfo = new ActionCommand<object>(obj => LockPageAction(1));
            ////Registration button events
            GetHandleAlarmCommand = new ActionCommand<object>(obj => GetHandleAlarmAction(1));
            GetUnhandleAlarmCommand = new ActionCommand<object>(obj => GetUnhandleAlarmAction(1));

            ////The default screen location and event registration
            UnHandedAlarmLocateCommand = new ActionCommand<object>(obj => UnHandedAlarmLocateAction(obj));
            TraceVehicleAlarmCommand = new ActionCommand<object>(obj => TraceVehicleAlarmAction(obj));

            HappenLoctionCommand = new ActionCommand<object>(obj => HappenLoctionAction(obj));
            OpenDetailViewCommand = new ActionCommand<object>((obj) => OpenDetailViewClick_Event(obj));
            ////A key alarm event registration details have been processed
            //////HandedAlarmDetailCommand = new ActionCommand<object>(obj => HandedAlarmDetailAction());
            HandedAlarmVedio1Command = new ActionCommand<object>(obj => HandedAlarmVedio1Action());
            HandedAlarmVedio2Command = new ActionCommand<object>(obj => HandedAlarmVedio2Action());
            HandedAlarmVedio3Command = new ActionCommand<object>(obj => HandedAlarmVedio3Action());
            HandedAlarmVedio4Command = new ActionCommand<object>(obj => HandedAlarmVedio4Action());

            HandedAlarmVedio1First15SCommand = new ActionCommand<object>(obj => HandedAlarmVedio1First15SAction());

            HistoricalRouteCommand = new ActionCommand<object>(obj => HistoricalRouteCommandAction());

            ////Sign untreated a key alarm event details
            //////UnHandedAlarmDetailCommand = new ActionCommand<object>(obj => UnHandedAlarmDetailAction(obj));
            UnHandedAlarmVedio1Command = new ActionCommand<object>(obj => UnHandedAlarmVedio1Action());
            UnHandedAlarmVedio2Command = new ActionCommand<object>(obj => UnHandedAlarmVedio2Action());
            UnHandedAlarmVedio3Command = new ActionCommand<object>(obj => UnHandedAlarmVedio3Action());
            UnHandedAlarmVedio4Command = new ActionCommand<object>(obj => UnHandedAlarmVedio4Action());
            UnHandedAlarmVedio1First15SCommand = new ActionCommand<object>(obj => UnHandedAlarmVedio1First15SAction());

            UnHistoricalRouteCommand = new ActionCommand<object>(obj => UnHistoricalRouteCommandAction());
            DownloadVideoCommand = new ActionCommand<object>((obj) => DownloadVideo_Event(obj));

            ////Binding object initialization
            HandleAlarms = new ObservableCollection<AlarmInfoEx>();


            ////Processed an alarm paging key
            HandleAlarmsDataPager = new PagedCollectionView(HandleAlarms);
            this.HandleAlarmsDataPager.PageChanged += HandleAlarmsDataPager_PageChanged;
            this.HandleAlarmsDataPager.PageSize = HandleAlarmsPageSizeValue;

            ////A key alarm paging untreated

            HandleAlarmsPageIndex = 1;
        }

        private void vedioServiceClient_GetAlarmFiftyVideoAppealCompleted(object sender, GetAlarmFiftyVideoAppealCompletedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(e.Result.Result))
            //{
            //    Gsafety.PTMS.BasicPage.VideoDisplay.Util.OpenVideoPage(new FileListVideoArgs
            //     {
            //         ChannelId = 0,
            //         FileList = e.Result.Result,
            //         IsAutoPlay = true,
            //         Key = Guid.NewGuid().ToString(),

            //     }, 400, 300);
            //}
            //else
            //{
            //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("History_VideoError"));
            //}
        }



        private void GroupHandledSelectCommand_Event(object obj)
        {
            if (CurrentHandedAlarmInfo != null)
            {
                Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(p => p.VehicleId == CurrentHandedAlarmInfo.VehicleId).FirstOrDefault();
                if (null != vehicle)
                {
                    //Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle, Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance);
                    Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle);
                    window.Closed += groupSelectWindow_Closed;
                    window.Show();
                }
            }

        }
        DateTime dt = new DateTime();
        private void LockPageAction(int p)
        {
            if (LockFlag == 0)
            {
                DateTime dt = DateTime.Now;
                _UnHandledAlarmPagedCV.Filter = new Predicate<object>(trealrealtime);
                LockFlag = 1;
            }
            else if (LockFlag == 1)
            {
                _UnHandledAlarmPagedCV.Filter = null;
                LockFlag = 0;
            }
        }

        private bool trealrealtime(object obj)
        {
            AlarmInfoEx info = obj as AlarmInfoEx;
            if (info.AlarmTime < dt)
            {
                return true;
            }
            return false;
        }


        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            //if (ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo.Count > 0)
            //{
            //    CurrentUnHandedAlarmInfo = ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo[0];
            //}

            GetUnhandleAlarmAction(1); //add by penggl 2014-01-11

        }


        #endregion

        #region Event

        private void GroupSelectCommand_Event(object obj)
        {

            if (null != CurrentUnHandedAlarmInfo)
            {
                Vehicle vehicle = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(p => p.VehicleId == CurrentUnHandedAlarmInfo.VehicleId).FirstOrDefault();
                if (null != vehicle)
                {
                    //Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle, Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance);
                    Gsafety.PTMS.BasicPage.Monitor.Views.GroupSelectWindow window = new GroupSelectWindow(vehicle);
                    window.Closed += groupSelectWindow_Closed;
                    window.Show();
                }
            }
        }

        void groupSelectWindow_Closed(object sender, EventArgs e)
        {
            GroupSelectWindow win = sender as GroupSelectWindow;
            if (null != win && win.DialogResult == true)
            {

                if ((string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID)) || (win.OldGroup == win.model.GroupID))
                {
                    return;
                }
                else if (string.IsNullOrEmpty(win.OldGroup) && !string.IsNullOrEmpty(win.model.GroupID))
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.AddVehicle(win.model, win.model.GroupID);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Add });
                }
                else if (!string.IsNullOrEmpty(win.OldGroup) && string.IsNullOrEmpty(win.model.GroupID))
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.DeleteVehicle(win.model);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Delete });
                }
                else
                {
                    //Gsafety.PTMS.BasicPage.Monitor.ViewModels.MoniterGroupManage.Instance.MoveVehicle(win.model, win.OldGroup, win.model.GroupID);
                    EventAggregator.Publish<UserDefineGroupVehicleDataRefreshMessage>(new UserDefineGroupVehicleDataRefreshMessage() { vehicle = win.model, doOperator = UserDefineGroupOperator.Move, oldGroup = win.OldGroup });
                }

            }
        }
        #region Query Event
        /// <summary>
        /// Get a key alarm has been processed (Status = 4)
        /// </summary>
        /// <param name="pageIndex"></param>
        private void GetHandleAlarmAction(int pageIndex)
        {
            if (!string.IsNullOrEmpty(HandleFiledCarNumber))
            {
                HandleFiledCarNumber = HandleFiledCarNumber.Trim();
            }

            if (StartTime != null && EndTime != null)
            {
                if (StartTime < EndTime)
                {
                    if (EndTime > DateTime.Now.Date.AddDays(1))
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"));
                    return;
                }
            }

            ServiceClientFactory.CreateMessageHeader(vehicleAlarmServiceClient.InnerChannel);
            IsHandledBusy = true;
            this.HandleAlarmsPageIndex = pageIndex;
            vehicleAlarmServiceClient.GetHandledAlarmsAsync(HandleFiledCarNumber, StartTime, EndTime, IsTrueAlarm, new PagingInfo { PageIndex = pageIndex, PageSize = HandleAlarmsPageSizeValue }, ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }
        private void GetHandleAlarmByfilter()
        {
            this.HandleAlarmsDataPager.Filter = null;
        }
        /// <summary>
        /// Get untreated a key alarm (Status = 1)
        /// </summary>
        /// <param name="pageIndex"></param>
        private void GetUnhandleAlarmAction(int pageIndex)
        {
            _UnHandledAlarmPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo);
            _UnHandledAlarmPagedCV.Filter = null;

            if (!string.IsNullOrEmpty(UnHandleFiledCarNumber))
            {
                _UnHandledAlarmPagedCV.Filter = new Predicate<object>(FilterVehicle);
            }

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UnHandledAlarmPagedCV));
        }

        private bool VehicleAlarm(object obj)
        {
            AlarmInfoEx info = obj as AlarmInfoEx;
            if (UnhandlerStartTime != null && UnhandlerEndTime != null)
            {
                if (UnhandlerStartTime <= info.AlarmTime && UnhandlerEndTime >= info.AlarmTime)
                {
                    if (UnHandleFiledCarNumber == null)
                    {
                        return true;
                    }
                    else
                    {
                        return info.VehicleId.ToLower().Contains(UnHandleFiledCarNumber.Trim().ToLower());
                    }
                }
                return false;
            }
            else
            {
                return info.VehicleId.ToLower().Contains(UnHandleFiledCarNumber.Trim().ToLower());
            }
        }

        private bool FilterVehicle(object obj)
        {
            AlarmInfoEx info = obj as AlarmInfoEx;

            return info.VehicleId.ToLower().Contains(UnHandleFiledCarNumber.Trim().ToLower());
        }

        #endregion
        #region Get the data has been processed alarms
        void vehicleAlarmServiceClient_GetHandledAlarmsCompleted(object sender, GetHandledAlarmsCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    IsHandledBusy = false;
                    return;
                }
                else
                {
                    IsHandledBusy = false;
                    HandleAlarms = e.Result.Result;
                    foreach (var item in HandleAlarms)
                    {
                        if (MonitorList.AlarmHappenLocationElements.Exists(item.MdvrCoreId + "@" + item.AlarmTime))
                        {
                            //item.IsMonitor = true;
                        }
                    }
                    if (HandleAlarms != null && HandleAlarms.Count > 0)
                        CurrentHandedAlarmInfo = HandleAlarms[0];
                    if (HandleAlarmsPageIndex == 1)
                    {
                        List<int> pageList = new List<int>(e.Result.TotalRecord);
                        for (int i = 0; i < e.Result.TotalRecord; i++)
                        {
                            pageList.Add(i);
                        }
                        this.HandleAlarmsDataPager = new PagedCollectionView(pageList);
                        this.HandleAlarmsDataPager.PageChanged += HandleAlarmsDataPager_PageChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetHandledAlarmsCompleted", ex);
            }
        }
        #endregion
        #region Untreated a key alarm events
        /// <summary>
        /// Incident to incident
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void HappenLoctionAction(object obj)
        {
            AlarmInfoEx alarminfo = obj as AlarmInfoEx;
            if (alarminfo.GpsValid != "A")
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                return;
            }
            if (alarminfo != null)
            {
                if (!alarminfo.IsMonitor)
                {
                    alarminfo.IsMonitor = true;
                    if (alarminfo.GpsValid == "A")
                    {
                        EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = alarminfo.MdvrCoreId, Direction = alarminfo.Direction, AlarmTime = alarminfo.AlarmTime, GpsTime = alarminfo.GpsTime, Speed = alarminfo.Speed, GpsValid = alarminfo.GpsValid, Latitude = alarminfo.Latitude, Longitude = alarminfo.Longitude, Op = 1, VehicleId = alarminfo.VehicleId });
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                    }
                }
                else if (alarminfo.IsMonitor)
                {
                    EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = alarminfo.MdvrCoreId, AlarmTime = alarminfo.AlarmTime, Direction = alarminfo.Direction, GpsTime = alarminfo.GpsTime, Speed = alarminfo.Speed, GpsValid = alarminfo.GpsValid, Latitude = alarminfo.Latitude, Longitude = alarminfo.Longitude, Op = 0, VehicleId = alarminfo.VehicleId });
                    alarminfo.IsMonitor = false;
                }
                if (null != _CurrentTraceVehicle && _CurrentTraceVehicle.ID != alarminfo.ID)
                {
                    _CurrentTraceVehicle.IsLocate = false;
                    _CurrentTraceVehicle.IsTrace = false;
                    EventAggregator.Publish<TrackCarArgs>(new TrackCarArgs() { VE = ElementLayerDefine.miVEAlarmHappenLocation, UniqueID = string.Empty });
                }

                if (_CurrentTraceVehicle == null || _CurrentTraceVehicle.ID != alarminfo.ID)
                {
                    _CurrentTraceVehicle = alarminfo;
                }
            }
        }
        /// <summary>
        /// Track events
        /// </summary>
        /// <param name="obj"></param>
        private void TraceVehicleAlarmAction(object obj)
        {
            AlarmInfoEx alrminfo = obj as AlarmInfoEx;
            if (alrminfo != null)
            {
                if (!alrminfo.IsTrace)
                {
                    alrminfo.IsTrace = true;
                    EventAggregator.Publish<TrackCarArgs>(new TrackCarArgs() { VE = ElementLayerDefine.miVERealLocation, UniqueID = alrminfo.MdvrCoreId });
                }
                else
                {
                    alrminfo.IsTrace = false;
                    if (_CurrentTraceVehicle != null && _CurrentTraceVehicle.ID == alrminfo.ID)
                    {
                        EventAggregator.Publish<TrackCarArgs>(new TrackCarArgs() { VE = ElementLayerDefine.miVERealLocation, UniqueID = string.Empty });
                        return;
                    }

                }
                if (null != _CurrentTraceVehicle && _CurrentTraceVehicle.ID != alrminfo.ID)
                {
                    _CurrentTraceVehicle.IsLocate = false;
                    _CurrentTraceVehicle.IsTrace = false;
                    //EventAggregator.Publish<TrackCarArgs>(new TrackCarArgs() { VE = VisibleElementDefine.miVEOneKeyAlarm, MDVRCoreSN = string.Empty });

                }
                if (_CurrentTraceVehicle == null || _CurrentTraceVehicle.ID != alrminfo.ID)
                {
                    _CurrentTraceVehicle = alrminfo;
                }
            }
        }

        /// <summary>
        /// Untreated a key position to view alarm
        /// </summary>
        private void UnHandedAlarmLocateAction(object obj)
        {
            AlarmInfoEx alarminfo = obj as AlarmInfoEx;
            if (alarminfo != null)
            {
                List<Vehicle> lst = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.Where(item => item.VehicleId == alarminfo.VehicleId).ToList();
                if ((lst != null) && (lst.Count > 0))
                {
                    Vehicle vehicle = lst.First();
                    EventAggregator.Publish<DisplayCurrentPositionArgs>(new DisplayCurrentPositionArgs()
                    {
                        Prov = alarminfo.Province,
                        CarNo = alarminfo.VehicleId,
                        //Company = alarminfo.CompanyName,
                        VE = (int)ElementLayerDefine.miVERealLocation
                    });
                }
                else
                {
                    EventAggregator.Publish<LocateEventArgs>(new LocateEventArgs() { Operate = MapEventArgs.MapOperateLocateByUniqueID, VE = ElementLayerDefine.miVERealLocation, UniqueID = alarminfo.MdvrCoreId, VehicleID = alarminfo.VehicleId });
                }

                //if (_CurrentTraceVehicle != null)
                //{
                //    _CurrentTraceVehicle.IsTrace = false;
                //}
                if (null != _CurrentTraceVehicle && _CurrentTraceVehicle.ID != alarminfo.ID)
                {
                    _CurrentTraceVehicle.IsLocate = false;
                    _CurrentTraceVehicle.IsTrace = false;
                    //EventAggregator.Publish<TrackCarArgs>(new TrackCarArgs() { VE = VisibleElementDefine.miVEOneKeyAlarm, MDVRCoreSN = string.Empty });

                }

                if (_CurrentTraceVehicle == null || _CurrentTraceVehicle.ID != alarminfo.ID)
                { _CurrentTraceVehicle = alarminfo; }

            }

        }

        /// <summary>
        /// Untreated historical trajectory
        /// </summary>
        public void UnHistoricalRouteCommandAction()
        {
            HistoricalRoute hisRoute = new HistoricalRoute(CurrentUnHandedAlarmInfo.VehicleId, HisGPSDataType.AlarmGPS, false, CurrentUnHandedAlarmInfo.AlarmTime.Value, CurrentUnHandedAlarmInfo.AlarmTime.Value.AddMinutes(3));
            hisRoute.Closed += hisRoute_Closed;
            hisRoute.Show();
        }

        void hisRoute_Closed(object sender, EventArgs e)
        {
            HistoricalRoute window = sender as HistoricalRoute;
            window.Closed -= hisRoute_Closed;
            if (window != null && window.DialogResult == true)
            {
                EventAggregator.Publish<HisTraceArgs>(window.HistraceArgs);
            }
        }

        /// <summary>
        /// Untreated a key alarm 1 channel screen
        /// </summary>
        private void UnHandedAlarmVedio1Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 0;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentUnHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// Untreated a key alarm 2-way screen
        /// </summary>
        private void UnHandedAlarmVedio2Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 1;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentUnHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// Untreated a key alarm 3-way screen
        /// </summary>
        private void UnHandedAlarmVedio3Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 2;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentUnHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// Untreated a key alarm 4-way screen
        /// </summary>
        private void UnHandedAlarmVedio4Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 3;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentUnHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }
        /// <summary>
        /// Untreated one key way 15 seconds before the alarm a screen
        /// </summary>
        private void UnHandedAlarmVedio1First15SAction()
        {
            //AlarmVideo15Args args = new AlarmVideo15Args();
            //args.ChannelId = 0;
            //args.IsAutoPlay = true;
            //args.MdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
            //args.CarNo = CurrentUnHandedAlarmInfo.VehicleId;
            //Util.OpenVideoPage(args, 400, 300);
            string alarmId = CurrentUnHandedAlarmInfo.ID;
            vedioServiceClient.GetAlarmFiftyVideoAppealAsync(alarmId);

        }

        /// <summary>
        /// Alarm event handling
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="flag"></param>
        void AlarmHandleWindowAction(bool msg, int flag)
        {
            if (msg)
            {
                if (flag == 1)
                {
                    GetUnhandleAlarmAction(1);
                }
                if (flag == 0)
                {
                    AlarmInfoDisplay info = new AlarmInfoDisplay();
                    info.DisPlayInfo = CurrentUnHandedAlarmInfo;
                    EventAggregator.Publish<AlarmInfoDisplay>(info);
                }
                GetHandleAlarmAction(this.HandleAlarmsPageIndex);
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_TrealFail"));
            }
        }

        #endregion

        #region A key alarm events have been processed



        /// <summary>
        /// Historical trajectory has been processed
        /// </summary>
        public void HistoricalRouteCommandAction()
        {
            HistoricalRoute hisRoute = new HistoricalRoute(CurrentHandedAlarmInfo.VehicleId, HisGPSDataType.AlarmGPS, false, CurrentHandedAlarmInfo.AlarmTime.Value, CurrentHandedAlarmInfo.AlarmTime.Value.AddMinutes(3));
            hisRoute.Closed += hisRoute_Closed;
            hisRoute.Show();
        }

        /// <summary>
        /// One key way the police handled a screen
        /// </summary>
        private void HandedAlarmVedio1Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 0;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// One key way the police handled two screen
        /// </summary>
        private void HandedAlarmVedio2Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 1;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// One key way the police handled three screen
        /// </summary>
        private void HandedAlarmVedio3Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 2;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }

        /// <summary>
        /// One key way the police handled four screen
        /// </summary>
        private void HandedAlarmVedio4Action()
        {
            AlarmVideoArgs args = new AlarmVideoArgs();
            args.ChannelId = 3;
            args.IsAutoPlay = true;
            args.MdvrId = CurrentHandedAlarmInfo.MdvrCoreId;
            args.CarNo = CurrentHandedAlarmInfo.VehicleId;
            Util.OpenVideoPage(args, 400, 300);
        }
        /// <summary>
        /// A key alarm has been processed before a road 15 seconds Screen
        /// </summary>
        private void HandedAlarmVedio1First15SAction()
        {
            string alarmId = CurrentUnHandedAlarmInfo.ID;
            vedioServiceClient.GetAlarmFiftyVideoAppealAsync(alarmId);
        }

        #endregion

        #region Other events

        internal void OpenDetailViewClick_Event(object obj)
        {
            EventAggregator.Publish<OpenState>(new OpenState() { State = true });
        }
        /// <summary>
        /// Paging events have been processed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HandleAlarmsDataPager_PageChanged(object sender, EventArgs e)
        {
            HandleAlarmsPageIndex = ((PagedCollectionView)sender).PageIndex + 1;
            GetHandleAlarmAction(HandleAlarmsPageIndex);
        }
        /// <summary>
        /// Switching event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Accordion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Accordion accordion = sender as Accordion;
            CurrentAccordionSelectIndex = accordion.SelectedIndex;
            if (CurrentAccordionSelectIndex == 0)
            {
                if (CurrentUnHandedAlarmInfo != null)
                {
                    AlarmInfoDisplay info = new AlarmInfoDisplay();
                    info.DisPlayInfo = CurrentUnHandedAlarmInfo;
                    EventAggregator.Publish<AlarmInfoDisplay>(info);

                }
                else
                {
                    EventAggregator.Publish<AlarmInfoDisplay>(new AlarmInfoDisplay());
                }
                EventAggregator.Publish<int>(0);
            }
            else
            {
                if (CurrentHandedAlarmInfo != null)
                {
                    AlarmInfoDisplay info = new AlarmInfoDisplay();
                    info.DisPlayInfo = CurrentHandedAlarmInfo;
                    EventAggregator.Publish<AlarmInfoDisplay>(info);

                }
                else
                {
                    EventAggregator.Publish<AlarmInfoDisplay>(new AlarmInfoDisplay());
                }
                EventAggregator.Publish<int>(1);
            }

        }

        /// <summary>
        /// Download surveillance video from the MDVR
        /// </summary>
        private void DownloadVideo_Event(object obj)
        {
            int option = Convert.ToInt32(obj);
            var vehId = "";
            var mdvrId = "";
            var st = DateTime.Now;
            var et = DateTime.Now;
            if (option == 0)
            {
                vehId = CurrentHandedAlarmInfo.VehicleId;
                mdvrId = CurrentHandedAlarmInfo.MdvrCoreId;
                st = (CurrentHandedAlarmInfo.AlarmTime ?? DateTime.Now).AddHours(-1);
                et = (CurrentHandedAlarmInfo.AlarmTime ?? DateTime.Now).AddHours(1);
            }
            else
            {
                vehId = CurrentUnHandedAlarmInfo.VehicleId;
                mdvrId = CurrentUnHandedAlarmInfo.MdvrCoreId;
                st = (CurrentUnHandedAlarmInfo.AlarmTime ?? DateTime.Now).AddHours(-1);
                et = (CurrentUnHandedAlarmInfo.AlarmTime ?? DateTime.Now).AddHours(1);
            }

            DownloadVideo window = new DownloadVideo(vehId, mdvrId, st, et);
            window.Closed += new EventHandler(DownloadVideo_Closed);
            window.Show();

        }


        private void DownloadVideo_Closed(object sender, EventArgs e)
        {
            DownloadVideo window = sender as DownloadVideo;
            window.Closed -= DownloadVideo_Closed;
            if (window != null && window.DialogResult == true)
            {
                var args = new System.Collections.Generic.Dictionary<string, object>();
                args.Add(Guid.NewGuid().ToString(), window.videoDownLoadArgs);
                ApplicationContext.Instance.NavigateManager.Navigate("VideoDownLoad_View", Gsafety.PTMS.Constants.NavigationFrame.CentralPlatMainContentFrame);
                EventAggregator.Publish(new ViewNavigationArgs("VideoDownLoad_View", args));

                //if (window.videoDownLoadArgs.ArgType == QueryType.QueryMdvrFileList)
                //{
                //    ApplicationContext.Instance.NavigateManager.Navigate("V_MdvrFile", "VideoFrame");
                //    EventAggregator.Publish(new ViewNavigationArgs("V_MdvrFile", args));
                //}
                //else if (window.videoDownLoadArgs.ArgType == QueryType.QueryServerDownloadFileList)
                //{
                //    ApplicationContext.Instance.NavigateManager.Navigate("V_DownloadFile", "VideoFrame");
                //    EventAggregator.Publish(new ViewNavigationArgs("V_DownloadFile", args));
                //}
                //else if (window.videoDownLoadArgs.ArgType == QueryType.QueryDownloadingFielList)
                //{
                //    ApplicationContext.Instance.NavigateManager.Navigate("V_Downloading", "VideoFrame");
                //    EventAggregator.Publish(new ViewNavigationArgs("V_Downloading", args));
                //}
            }
        }
        #endregion
        #endregion


    }
}

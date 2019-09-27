/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ee421383-d264-436e-a750-5171237c8506      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.ViewModels
/////    Project Description:    
/////             Class Name: AlarmInfoVm
/////          Class Version: v1.0.0.0
/////            Create Time: 9/15/2013 10:29:41 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/15/2013 10:29:41 AM
/////            Modified by:
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
using Jounce.Core.ViewModel;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using VehicleAlarmService = Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using Jounce.Framework.Command;
using Gsafety.PTMS.Alarm;
using System.Reflection;

namespace Gsafety.PTMS.Alarm.ViewModels
{
    [ExportAsViewModel(AlarmName.AlarmInfoViewModle)]
    public class AlarmInfoVm : BaseViewModel, IEventSink<AlarmInfoDisplay>, IEventSink<int>, IPartImportsSatisfiedNotification, IEventSink<OpenState>
    {
        const string _strike = "-";
        private VehicleAlarmServiceClient vehicleAlarmServiceClient = null;

        #region Filed And Attr

        public string Title { get; set; }
        public string PicUrl { get; set; }

        //private string _VehicleType;
        //public string VehicleType
        //{
        //    get { return _VehicleType; }
        //    set { _VehicleType = value; }
        //}

        private string _Latitude;
        public string Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        private string _AlarmId;
        public string AlarmId
        {
            get { return _AlarmId; }
            set { _AlarmId = value; }
        }

        private string _GPSValid;
        /// <summary>
        /// 
        /// </summary>
        public string GPSValid
        {
            get { return _GPSValid; }
            set { _GPSValid = value; }
        }

        private string _Longitude;
        /// <summary>
        /// 
        /// </summary>
        public string Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        private string _DisTime;
        public string DisTime
        {
            get { return _DisTime; }
            set
            {
                _DisTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisTime));
            }
        }

        private string _DisStaff;
        public string DisStaff
        {
            get { return _DisStaff; }
            set
            {
                _DisStaff = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisStaff));
            }
        }

        private short? _IfTrueAlarm;
        public short? IfTrueAlarm
        {
            get { return _IfTrueAlarm; }
            set
            {
                _IfTrueAlarm = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IfTrueAlarm));
            }
        }

        /// <summary>
        /// The currently selected row
        /// </summary>
        public AlarmTreatment CurrentAlarmInfo
        {
            get { return _CurrentAlarmInfo; }
            set
            {
                _CurrentAlarmInfo = value;

                RaisePropertyChanged(() => CurrentAlarmInfo);
                if (CurrentAlarmInfo != null)
                {
                    DisContent = CurrentAlarmInfo.Content;
                    DisTime = CurrentAlarmInfo.DisposeTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
                    DisStaff = CurrentAlarmInfo.DisposeStaff;
                    //       IfTrueAlarm = CurrentAlarmInfo.IsTrueAlarm;
                    RaisePropertyChanged(() => DisContent);
                }
            }
        }

        public VehicleAlarmService.AlarmInfoEx AlarmInfo
        {
            get
            {
                return _AlarmInformation;
            }
        }

        private string _DisContent;
        public string DisContent
        {
            get { return _DisContent; }
            set { _DisContent = value; }
        }

        private string _veAlarmid;
        public string VeAlarmid
        {
            get { return _veAlarmid; }
            set { _veAlarmid = value; }
        }

        private Visibility _RefeshVisible;
        public Visibility RefeshVisible
        {
            get { return _RefeshVisible; }
            set { _RefeshVisible = value; }
        }

        private Visibility _AntVisible = Visibility.Collapsed;
        public Visibility AntVisible
        {
            get { return _AntVisible; }
            set { _AntVisible = value; }
        }

        private Visibility _ECU911Visible = Visibility.Collapsed;
        public Visibility ECU911Visible
        {
            get { return _ECU911Visible; }
            set { _ECU911Visible = value; }
        }

        VehicleAlarmService.AlarmInfoEx _AlarmInformation;

        //private Alarm911Treatment _EcuTreatment;
        ///// <summary>
        ///// 
        ///// </summary>
        //public Alarm911Treatment EcuTreatment
        //{
        //    get { return _EcuTreatment; }
        //    set { _EcuTreatment = value; }
        //}

        private bool _IsVisual = false;
        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                if (IsOpen)
                {
                    IsOpen = _IsVisual = false;
                }
                else
                {
                    IsOpen = _IsVisual = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
        }

        private string _VehicleId;
        public string VehicleId
        {
            get { return _VehicleId; }
            set { _VehicleId = value; }
        }

        private string _AlarmTime;
        public string AlarmTime
        {
            get { return _AlarmTime; }
            set { _AlarmTime = value; }
        }

        private ObservableCollection<AlarmTreatment> _AlarmList;
        public ObservableCollection<AlarmTreatment> AlarmList
        {
            get { return _AlarmList; }
            set { _AlarmList = value; }
        }

        private AlarmTreatment _CurrentAlarmInfo;

        //private Returninfo _ReturnInfo911;
        //public Returninfo ReturnInfo911
        //{
        //    get { return _ReturnInfo911; }
        //    set { _ReturnInfo911 = value; }
        //}

        private string _Ecu911Content = _strike;
        public string Ecu911Content
        {
            get { return _Ecu911Content; }
            set { _Ecu911Content = value; }
        }

        private string _Ecu911AlarmDetail = _strike;
        public string Ecu911AlarmDetail
        {
            get { return _Ecu911AlarmDetail; }
            set { _Ecu911AlarmDetail = value; }
        }


        private string _INCIDENT_ID;
        /// <summary>
        /// Alarm ID
        /// </summary>
        public string INCIDENT_ID
        {
            get { return _INCIDENT_ID; }
            set { _INCIDENT_ID = value; }
        }

        private string _FORWARDED_FLAG = _strike;
        /// <summary>
        /// 
        /// </summary>
        public string FORWARDED_FLAG
        {
            get { return _FORWARDED_FLAG; }
            set
            {
                _FORWARDED_FLAG = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FORWARDED_FLAG));
            }
        }

        private string _ALARM_FLAG = _strike;
        /// <summary>
        /// 
        /// </summary>
        public string ALARM_FLAG
        {
            get { return _ALARM_FLAG; }
            set
            {
                _ALARM_FLAG = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ALARM_FLAG));
            }
        }

        private string _DisposeTime;
        public string DisposeTime
        {
            get { return _DisposeTime; }
            set { _DisposeTime = value; Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisposeTime)); }
        }

        private string _FORWARD_TIME;
        public string FORWARD_TIME
        {
            get { return _FORWARD_TIME; }
            set { _FORWARD_TIME = value; Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FORWARD_TIME)); }
        }

        private int _SelectItemIndex = 0;
        public int SelectItemIndex
        {
            get
            {
                return _SelectItemIndex;
            }
            set
            {
                _SelectItemIndex = value;
            }
        }

        private bool m_IsOpen = true;
        public bool IsOpen
        {
            get { return m_IsOpen; }
            set
            {
                m_IsOpen = value;
            }
        }
        #endregion

        #region Command
        public ICommand QueryCommand { get; private set; }
        public ICommand Query911Command { get; private set; }
        public ICommand GetInfoCommand { get; private set; }
        #endregion

        #region  init
        /// <summary>
        /// Constructors
        /// </summary>
        public AlarmInfoVm()
        {
            try
            {
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                vehicleAlarmServiceClient.GetAlarmTreatmentsCompleted += vehicleAlarmServiceClient_GetAlarmTreatmentsCompleted;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmInfoVm()", ex);
            }

            GetInfoCommand = new ActionCommand<object>(obj => Get911Info());
            QueryCommand = new ActionCommand<object>(obj => QueryAction());

        }

        /// <summary>
        /// Subscribe to event
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<AlarmInfoDisplay>(this);
            EventAggregator.SubscribeOnDispatcher<int>(this);
            EventAggregator.SubscribeOnDispatcher<OpenState>(this);
        }
        #endregion

        #region method
        private void Get911Info()
        {
            
        }

        /// <summary>
        /// Inquiry
        /// </summary>
        private void QueryAction()
        {
            vehicleAlarmServiceClient.GetAlarmTreatmentsAsync(VeAlarmid);
        }
        #endregion

        #region wcf completed
        void vehicleAlarmServiceClient_GetAlarmTreatmentsCompleted(object sender, GetAlarmTreatmentsCompletedEventArgs e)
        {
            if (e.Result.Result != null)
            {
                AlarmList = e.Result.Result;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmList));
                if (AlarmList.Count > 0)
                {
                    CurrentAlarmInfo = AlarmList[0];
                    // INCIDENT_ID = AlarmList[0].;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentAlarmInfo));
                }
                else
                {
                    DisContent = string.Empty;

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisContent));
                }


            }
        }

        //void vehicleAlarmServiceClient_GetAlarm911TreatmentsCompleted(object sender, GetAlarm911TreatmentsCompletedEventArgs e)
        //{
        //    //从数据库911Dispose中取出来的数据
        //    if (e.Result.Result != null)
        //    {
        //        EcuTreatment = e.Result.Result;
        //        //if (EcuTreatment.fo)
        //        //{

        //        //}
        //        if (EcuTreatment.FORWARDED_FLAG != null)
        //        {
        //            if (EcuTreatment.FORWARDED_FLAG == 0)
        //            {
        //                FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnSend");
        //                INCIDENT_ID = EcuTreatment.INCIDENT_ID;
        //                // ALARM_SendFailure  ALARM_SendSuc
        //            }
        //            else if (EcuTreatment.FORWARDED_FLAG == 1)
        //            {
        //                FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc");

        //            }
        //            else if (EcuTreatment.FORWARDED_FLAG == 2)
        //            {
        //                FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendFailure");
        //            }
        //            else if (EcuTreatment.FORWARDED_FLAG == 3)
        //            {
        //                FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Sending");
        //            }
        //            else
        //            {
        //                FORWARDED_FLAG = _strike;
        //            }
        //        }
        //        else
        //        {
        //            FORWARDED_FLAG = _strike;
        //        }

        //        if (EcuTreatment.ALARM_FLAG != null)
        //        {
        //            if (EcuTreatment.ALARM_FLAG.Value == 1)
        //            {
        //                ALARM_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("TRUE");
        //            }
        //            else if (EcuTreatment.ALARM_FLAG.Value == 0)
        //            {
        //                ALARM_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("FALSE");
        //            }
        //        }
        //        else
        //        {
        //            ALARM_FLAG = _strike;
        //        }

        //        if (EcuTreatment.Ecu911Center == null)
        //        {
        //            EcuTreatment.Ecu911Center = _strike;
        //        }

        //        if (EcuTreatment.DisposeStaff == null)
        //        {
        //            EcuTreatment.DisposeStaff = _strike;
        //        }

        //        if (EcuTreatment.ALARM_ADDRESS == null)
        //        {
        //            EcuTreatment.ALARM_ADDRESS = _strike;
        //        }

        //        if (EcuTreatment.DisposeTime == null)
        //        {
        //            DisposeTime = _strike;
        //        }
        //        else
        //        {
        //            DisposeTime = EcuTreatment.DisposeTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
        //        }

        //        if (EcuTreatment.FORWARD_TIME == null)
        //        {
        //            FORWARD_TIME = _strike;
        //        }
        //        else
        //        {
        //            FORWARD_TIME = EcuTreatment.FORWARD_TIME.Value.ToString("dd-MM-yyyy HH:mm:ss");
        //        }

        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EcuTreatment));
        //    }
        //    else
        //    {
        //        FORWARDED_FLAG = _strike;
        //        ALARM_FLAG = _strike;
        //        EcuTreatment = new Alarm911Treatment();
        //        EcuTreatment.Ecu911Center = _strike;
        //        EcuTreatment.DisposeStaff = _strike;
        //        EcuTreatment.ALARM_ADDRESS = _strike;
        //        EcuTreatment.FORWARD_DEST = _strike;
        //        DisposeTime = _strike;
        //        FORWARD_TIME = _strike;
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EcuTreatment));
        //    }
        //}

        //void vehicleAlarmServiceClient_GetAlarm911ResultCompleted(object sender, GetAlarm911ResultCompletedEventArgs e)
        //{
        //    //访问911接口返回回来的信息
        //    // 界面显示如下信息
        //    // 1 处理状态    Content
        //    // 2, 警情状态   Incidentstatus
        //    //感觉这里个显示的信息差不多，不明白之前为什么这样处理
        //    if (e.Error != null)
        //    {
        //        return;
        //    }
        //    else
        //    {

        //        ReturnInfo911 = e.Result.Result;
        //        if (ReturnInfo911 == null)
        //        {
        //            return;
        //        }
        //        //类型
        //        if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfSuper"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfSuper");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfPolice"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfPolice");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfRiskControl"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfRiskControl");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfTraffic"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfTraffic");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfMedical"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfMedical");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfArmy"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfArmy");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfMunicipal"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfMunicipal");
        //        }
        //        else if (ReturnInfo911.Content != null && ReturnInfo911.Content.Contains("IncidentTypeOfFireFighting"))
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfFireFighting");
        //        }
        //        else
        //        {
        //            Ecu911Content = ApplicationContext.Instance.StringResourceReader.GetString("IncidentTypeOfSuper");
        //        }
        //        // Ecu911AlarmDetail=ReturnInfo911.status
        //        //转警状态
        //        if (ReturnInfo911.Incidentstatus == 1)
        //        {
        //            Ecu911AlarmDetail = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnHandle");
        //        }
        //        else if (ReturnInfo911.Incidentstatus == 2)
        //        {
        //            Ecu911AlarmDetail = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Handing");
        //        }
        //        else if (ReturnInfo911.Incidentstatus == 3)
        //        {
        //            Ecu911AlarmDetail = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_TransAlarming");

        //        }
        //        else if (ReturnInfo911.Incidentstatus == 4)
        //        {
        //            Ecu911AlarmDetail = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Transfered");
        //        }
        //        else if (ReturnInfo911.Incidentstatus == 5)
        //        {
        //            Ecu911AlarmDetail = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_EndHandle");
        //        }

        //        if (!string.IsNullOrEmpty( ReturnInfo911.IncidentAddress) )
        //        {
        //            this.EcuTreatment.ALARM_ADDRESS = ReturnInfo911.IncidentAddress;
        //        }
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Ecu911Content));
        //        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Ecu911AlarmDetail));
        //    }
        //}
        #endregion

        #region HandleEvent
        /// <summary>
        /// For more information on alarms
        /// </summary>
        /// <param name="alarmInfo"></param>
        public void HandleEvent(AlarmInfoDisplay alarmInfo)
        {
            if (alarmInfo.DisPlayInfo != null)
            {
                _AlarmInformation = alarmInfo.DisPlayInfo;

                //if (!string.IsNullOrEmpty(_AlarmInformation.ECUId))
                //{
                //    IfTrueAlarm = (short)1;

                //    ECU911Visible = Visibility.Visible;
                //}
                //else
                //{
                //    IfTrueAlarm = (short)0;
                //    ECU911Visible = Visibility.Collapsed;
                //}

                VehicleId = alarmInfo.DisPlayInfo.VehicleId;
                VeAlarmid = alarmInfo.DisPlayInfo.ID;
                if (alarmInfo.DisPlayInfo.GpsValid != "A")
                {
                    Longitude = _strike;
                    Latitude = _strike;
                }
                else if (alarmInfo.DisPlayInfo.GpsValid == "A")
                {
                    Longitude = alarmInfo.DisPlayInfo.Longitude;
                    Latitude = alarmInfo.DisPlayInfo.Latitude;
                }
                AlarmTime = alarmInfo.DisPlayInfo.AlarmTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
                //VehicleType = ApplicationContext.Instance.StringResourceReader.GetString(((Gsafety.PTMS.Bases.Enums.VehicleType)alarmInfo.DisPlayInfo.VehicleType).ToString());
                AlarmId = alarmInfo.DisPlayInfo.ID;
                vehicleAlarmServiceClient.GetAlarmTreatmentsAsync(alarmInfo.DisPlayInfo.ID);
            }
            else
            {
                _AlarmInformation = new AlarmInfoEx();
                IfTrueAlarm = null;
                VehicleId = string.Empty;
                VeAlarmid = string.Empty;
                Longitude = string.Empty;
                Latitude = string.Empty; 
                AlarmTime = string.Empty;
                this.DisposeTime = string.Empty;
                this.DisTime = string.Empty;
                this.DisStaff = string.Empty;
                //VehicleType = ApplicationContext.Instance.StringResourceReader.GetString(((Gsafety.PTMS.Bases.Enums.VehicleType)alarmInfo.DisPlayInfo.VehicleType).ToString());
                AlarmId = string.Empty;
            }
            SelectItemIndex = 0;
         
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Latitude));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Longitude));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmTime));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmInfo));
            //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleType));
            //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ApealVisible));

            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
            Get911Info();
        }

        /// <summary>
        /// Control the refresh button visibility (untreated visible handled invisible)
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(int publishedEvent)
        {
            if (publishedEvent == 0)
            {
                RefeshVisible = Visibility.Visible;
                AntVisible = Visibility.Collapsed;
                //ApealVisible = Visibility.Collapsed;
                SelectItemIndex = 0;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + " (" + ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnHandleAlarm") + ")";
                PicUrl = "/ExternalResource;component/Images/MainPage_menu_info_orange.png";
            }
            if (publishedEvent == 1)
            {
                RefeshVisible = Visibility.Collapsed;
                AntVisible = Visibility.Visible;
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + " (" + ApplicationContext.Instance.StringResourceReader.GetString("ALARM_HandledAlarm") + ")";
                PicUrl = "/ExternalResource;component/Images/MainPage_menu_info.png";
            }
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RefeshVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AntVisible));
            //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ApealVisible));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PicUrl));
        }

        /// <summary>
        /// Processed form of switch
        /// </summary>
        /// <param name="publishedEvent"></param>
        public void HandleEvent(OpenState publishedEvent)
        {
            IsVisual = publishedEvent.State;
            if (string.IsNullOrEmpty(Title))
            {
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnHandleAlarm") + ")";
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                PicUrl = "/ExternalResource;component/Images/MainPage_menu_info_orange.png";
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PicUrl));
            }
        }
        #endregion
    }

    /// <summary>
    /// Forms open
    /// </summary>
    public class OpenState
    {
        public bool State { get; set; }
    }

    public class AlarmInfoDisplay
    {
        public AlarmInfoEx DisPlayInfo { get; set; }
    }
}

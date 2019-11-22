using Gsafety.Ant.Monitor.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using VehicleAlarmService = Gsafety.PTMS.ServiceReference.VehicleAlarmService;
namespace Gsafety.PTMS.Alarm.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorAlarmInfoViewModel)]
    public class AlarmInfoVm : BaseViewModel, IEventSink<MonitorAlarmInfoDisplay>, IPartImportsSatisfiedNotification
    {
        private DisplayLonConvert _lonConverter = new DisplayLonConvert();
        private DisplayLatConvert _latConverter = new DisplayLatConvert();
        const string _strike = "-";
        private VehicleAlarmServiceClient vehicleAlarmServiceClient = null;

        public string Title { get; set; }
        public string PicUrl { get; set; }

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

        VehicleAlarmService.AlarmInfoEx _AlarmInformation;
        public VehicleAlarmService.AlarmInfoEx AlarmInfo
        {
            get
            {
                return _AlarmInformation;
            }
        }

        /// <summary>
        /// 安全套件编号
        /// </summary>
        private string _veAlarmid;
        public string VeAlarmid
        {
            get { return _veAlarmid; }
            set { _veAlarmid = value; }
        }


        private Visibility _appealVisible = Visibility.Collapsed;
        public Visibility AppealVisible
        {
            get { return _appealVisible; }
            set { _appealVisible = value; }
        }

        private Visibility _diposeVisible = Visibility.Collapsed;
        public Visibility DisposeVisible
        {
            get { return _diposeVisible; }
            set { _diposeVisible = value; }
        }

        private bool _IsVisual = true;
        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                //if (IsOpen != value)
                //{
                //    if (IsOpen)
                //    {
                //        IsOpen = _IsVisual = false;
                //    }
                //    else
                //    {
                //        IsOpen = _IsVisual = true;
                //    }
                //}
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
        }

        private string _AlarmTime;
        public string AlarmTime
        {
            get { return _AlarmTime; }
            set { _AlarmTime = value; }
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
                //m_IsOpen = value;
                // _IsVisual = value;
            }
        }

        private string alarmsource;

        public string AlarmSource
        {
            get { return alarmsource; }
            set
            {
                alarmsource = value;
                RaisePropertyChanged("AlarmSource");
            }
        }

        private string _alarmcontent;
        public string AlarmContent
        {
            get
            {
                return _alarmcontent;
            }
            set
            {
                _alarmcontent = value;
                RaisePropertyChanged("AlarmContent");
            }
        }

        private string _keyword;
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                _keyword = value;
                RaisePropertyChanged("Keyword");
            }
        }

        private string _alarmMobile;

        public string AlarmMobile
        {
            get { return _alarmMobile; }
            set
            {
                _alarmMobile = value;
                RaisePropertyChanged("AlarmMobile");
            }
        }

        private string _apealalarmflag;
        public string ApealAlarmFlag
        {
            get
            {
                return _apealalarmflag;
            }
            set
            {
                _apealalarmflag = value;
                RaisePropertyChanged("ApealAlarmFlag");
            }
        }

        private string _apealcontent;
        public string ApealContent
        {
            get
            {
                return _apealcontent;
            }
            set
            {
                _apealcontent = value;
                RaisePropertyChanged("ApealContent");
            }
        }

        private string _apealdisposestaff;
        public string ApealDisposeStaff
        {
            get
            {
                return _apealdisposestaff;
            }
            set
            {
                _apealdisposestaff = value;
                RaisePropertyChanged("ApealDisposeStaff");
            }
        }

        private string _apealdisposetime;
        public string ApealDisposeTime
        {
            get
            {
                return _apealdisposetime;
            }
            set
            {
                _apealdisposetime = value;
                RaisePropertyChanged("ApealDisposeTime");
            }
        }

        private string _transferalarmflag;
        public string TransferAlarmFlag
        {
            get
            {
                return _transferalarmflag;
            }
            set
            {
                _transferalarmflag = value;
                RaisePropertyChanged("TransferAlarmFlag");
            }
        }

        private string _transferdisposetime;
        public string TransferDisposeTime
        {
            get
            {
                return _transferdisposetime;
            }
            set
            {
                _transferdisposetime = value;
                RaisePropertyChanged("TransferDisposeTime");
            }
        }

        private string _transferdisposestaff;
        public string TransferDisposeStaff
        {
            get
            {
                return _transferdisposestaff;
            }
            set
            {
                _transferdisposestaff = value;
                RaisePropertyChanged("TransferDisposeStaff");
            }
        }

        private string _transfertransfercenter;
        public string TransferTransferCenter
        {
            get
            {
                return _transfertransfercenter;
            }
            set
            {
                _transfertransfercenter = value;
                RaisePropertyChanged("TransferTransferCenter");
            }
        }

        private string _transferalarmaddress;
        public string TransferAlarmAddress
        {
            get
            {
                return _transferalarmaddress;
            }
            set
            {
                _transferalarmaddress = value;
                RaisePropertyChanged("TransferAlarmAddress");
            }
        }

        private string _transferincidenttype;
        public string TransferIncidentType
        {
            get
            {
                return _transferincidenttype;
            }
            set
            {
                _transferincidenttype = value;
                RaisePropertyChanged("TransferIncidentType");
            }
        }

        private string _transferdisposecontent;
        public string TransferDisposeContent
        {
            get
            {
                return _transferdisposecontent;
            }
            set
            {
                _transferdisposecontent = value;
                RaisePropertyChanged("TransferDisposeContent");
            }
        }

        private string _speed;
        public string Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                RaisePropertyChanged("Speed");
            }
        }

        private string _direction;
        public string Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                RaisePropertyChanged("Direction");
            }
        }

        private string _districtCodeDescribe;
        /// <summary>
        /// 行政区划的文字描述
        /// </summary>
        public string DistrictCodeDescribe
        {
            get
            {
                return this._districtCodeDescribe;
            }
            set
            {
                this._districtCodeDescribe = value;
                RaisePropertyChanged(() => DistrictCodeDescribe);
            }
        }

        private string _locate;
        /// <summary>
        /// 位置
        /// </summary>
        public string Locate
        {
            get
            {
                return this._locate;
            }
            set
            {
                this._locate = value;
                RaisePropertyChanged(() => Locate);
            }
        }

        private ChauffeurServiceClient client;
        private ObservableCollection<Chauffeur> _chauffeurList;
        /// <summary>
        /// 驾驶员列表
        /// </summary>
        public ObservableCollection<Chauffeur> ChauffeurList
        {
            get
            {
                return this._chauffeurList;
            }
            set
            {
                this._chauffeurList = value;
                RaisePropertyChanged(() => this.ChauffeurList);
            }
        }

        private Visibility _isNoteVisibility;
        /// <summary>
        /// 备注标签的显示和隐藏控制属性
        /// </summary>
        public Visibility IsNoteVisibility
        {
            get
            {
                return this._isNoteVisibility;
            }
            set
            {
                this._isNoteVisibility = value;
                RaisePropertyChanged(() => this.IsNoteVisibility);
            }
        }

        private Visibility _isAnswerNoteVisibility;
        /// <summary>
        /// 反馈信息中的Note是否显示
        /// </summary>
        public Visibility IsAnswerNoteVisibility
        {
            get
            {
                return this._isAnswerNoteVisibility;
            }
            set
            {
                this._isAnswerNoteVisibility = value;
                RaisePropertyChanged(() => IsAnswerNoteVisibility);
            }
        }

        private string _appealStatus;
        /// <summary>
        /// 处置信息处置状态标示
        /// </summary>
        public string AppealStatus
        {
            get
            {
                return this._appealStatus;
            }
            set
            {
                this._appealStatus = value;
                RaisePropertyChanged(() => AppealStatus);
            }
        }

        /// <summary>
        /// Constructors
        /// </summary>
        public AlarmInfoVm()
        {
            try
            {
                vehicleAlarmServiceClient = ServiceClientFactory.Create<VehicleAlarmServiceClient>();
                vehicleAlarmServiceClient.GetTransferDisposeByAlarmIDCompleted += vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted;
                vehicleAlarmServiceClient.GetApealDisposeByAlarmIDCompleted += vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted;
                //IsVisual = false;
                this.DistrictCodeDescribe = "";
                this.AppealStatus = "";
                this.Locate = "";
                this.ChauffeurList = new ObservableCollection<Chauffeur>();
                m_IsOpen = true;
                _IsVisual = true;
                this.IsNoteVisibility = Visibility.Collapsed;
                this.IsAnswerNoteVisibility = Visibility.Collapsed;
                this.InilitClient();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmInfoVm()", ex);
            }
        }

        private void InilitClient()
        {
            client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            client.GetChauffeurByVehicleCompleted += client_GetChauffeurByVehicleCompleted;
        }

        void client_GetChauffeurByVehicleCompleted(object sender, GetChauffeurByVehicleCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if (e.Result.IsSuccess == false)
                {
                    if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ErrorMsg);
                    }
                    else
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                            e.Result.ExceptionMessage);
                    }
                }
                else
                {
                    this.ChauffeurList = new ObservableCollection<Chauffeur>();
                    if (e.Result.Result.Count > 0)
                    {
                        this.ChauffeurList = e.Result.Result;
                    }
                    else
                    {
                        this.ChauffeurList.Insert(0, new Chauffeur()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Name = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
                            Phone = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel.client_GetChauffeurVehicleCompleted", ex);
            }
            finally
            {
                if (this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;
            }
        }

        void vehicleAlarmServiceClient_GetApealDisposeByAlarmIDCompleted(object sender, GetApealDisposeByAlarmIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && e.Result.IsSuccess)
                {
                    ApealDisposeTime = e.Result.Result.DisposeTime.ToLocalTime().ToString();
                    ApealDisposeStaff = e.Result.Result.DisposeStaff;

                    ApealAlarmFlag = e.Result.Result.AlarmFlag.ToString();

                    ApealContent = e.Result.Result.Content;
                    if (!string.IsNullOrEmpty(ApealContent))
                    {
                        this.IsNoteVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this.IsNoteVisibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted(object sender, GetTransferDisposeByAlarmIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && e.Result.IsSuccess)
                {
                    TransferTransferCenter = e.Result.Result.TransferCenter;
                    TransferAlarmAddress = e.Result.Result.AlarmAddress;
                    TransferAlarmFlag = e.Result.Result.AlarmFlag.ToString();
                    TransferDisposeStaff = e.Result.Result.DisposeStaff;
                    if (e.Result.Result.DisposeTime.HasValue)
                        TransferDisposeTime = e.Result.Result.DisposeTime.Value.ToLocalTime().ToString();
                    TransferIncidentType = e.Result.Result.IncidentType;
                    TransferDisposeContent = e.Result.Result.DisposeContent;
                    if (!string.IsNullOrEmpty(TransferDisposeContent))
                    {
                        this.IsAnswerNoteVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this.IsAnswerNoteVisibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// Subscribe to event
        /// </summary>
        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MonitorAlarmInfoDisplay>(this);
        }

        //private void Get911Info()
        //{
        //    vehicleAlarmServiceClient.GetAlarm911ResultAsync(AlarmInfo.ID);
        //}

        //void vehicleAlarmServiceClient_GetAlarm911TreatmentsCompleted(object sender, GetAlarm911TreatmentsCompletedEventArgs e)
        //{
        //从数据库911Dispose中取出来的数据
        //if (e.Result.Result != null)
        //{
        //    EcuTreatment = e.Result.Result;

        //    if (EcuTreatment.FORWARDED_FLAG != null)
        //    {
        //        if (EcuTreatment.FORWARDED_FLAG == 0)
        //        {
        //            FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_UnSend");
        //            INCIDENT_ID = EcuTreatment.INCIDENT_ID;
        //        }
        //        else if (EcuTreatment.FORWARDED_FLAG == 1)
        //        {
        //            FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc");

        //        }
        //        else if (EcuTreatment.FORWARDED_FLAG == 2)
        //        {
        //            FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendFailure");
        //        }
        //        else if (EcuTreatment.FORWARDED_FLAG == 3)
        //        {
        //            FORWARDED_FLAG = ApplicationContext.Instance.StringResourceReader.GetString("ALARM_Sending");
        //        }
        //        else
        //        {
        //            FORWARDED_FLAG = _strike;
        //        }
        //    }
        //    else
        //    {
        //        FORWARDED_FLAG = _strike;
        //    }

        //    if (EcuTreatment.Ecu911Center == null)
        //    {
        //        EcuTreatment.Ecu911Center = _strike;
        //    }

        //    if (EcuTreatment.FORWARD_TIME == null)
        //    {
        //        FORWARD_TIME = _strike;
        //    }
        //    else
        //    {
        //        FORWARD_TIME = EcuTreatment.FORWARD_TIME.Value.ToString("dd-MM-yyyy HH:mm:ss");
        //    }

        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EcuTreatment));
        //}
        //else
        //{
        //    FORWARDED_FLAG = _strike;
        //    EcuTreatment = new Alarm911Treatment();
        //    EcuTreatment.Ecu911Center = _strike;
        //    EcuTreatment.FORWARD_DEST = _strike;
        //    FORWARD_TIME = _strike;
        //    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => EcuTreatment));
        //}
        //}

        /// <summary>
        /// For more information on alarms
        /// </summary>
        /// <param name="alarmInfo"></param>
        public void HandleEvent(MonitorAlarmInfoDisplay alarmInfo)
        {
            try
            {
                if (alarmInfo.DisPlayInfo != null)
                {
                    _AlarmInformation = alarmInfo.DisPlayInfo;
                    AlarmMobile = alarmInfo.DisPlayInfo.AlarmMobile;

                    if (_AlarmInformation.AppealStatus != 0)
                    {
                        AppealVisible = Visibility.Visible;
                        //默认已处置
                        this.AppealStatus = ApplicationContext.Instance.StringResourceReader.GetString("Disposed");
                    }
                    else
                    {
                        AppealVisible = Visibility.Collapsed;
                        //未处置
                        this.AppealStatus = ApplicationContext.Instance.StringResourceReader.GetString("NoDisposed");
                    }

                    if (_AlarmInformation.TransferStatus != 0)
                    {
                        DisposeVisible = Visibility.Visible;
                    }
                    else
                    {
                        DisposeVisible = Visibility.Collapsed;
                    }

                    VeAlarmid = alarmInfo.DisPlayInfo.AlarmGuid;
                    if (alarmInfo.DisPlayInfo.GpsValid != null)
                    {
                        if (alarmInfo.DisPlayInfo.GpsValid != "A")
                        {
                            Longitude = _strike;
                            Latitude = _strike;
                            Speed = _strike;
                            Direction = _strike;
                        }
                        else if (alarmInfo.DisPlayInfo.GpsValid == "A")
                        {
                            Longitude = alarmInfo.DisPlayInfo.Longitude;
                            Latitude = alarmInfo.DisPlayInfo.Latitude;
                            Speed = alarmInfo.DisPlayInfo.Speed;
                            Direction = alarmInfo.DisPlayInfo.Direction;

                            var tempLat = _latConverter.ConvertBack(Latitude, null, null, null);
                            var resultLat = _latConverter.ConvertToWESN(tempLat, null, null, null);

                            var tempLon = _lonConverter.ConvertBack(Longitude, null, null, null);
                            var resultLon = _lonConverter.ConvertToWESN(tempLon, null, null, null);

                            Longitude = resultLon as string;
                            Latitude = resultLat as string;
                        }
                    }

                    AlarmTime = alarmInfo.DisPlayInfo.AlarmTime.Value.ToLocalTime().ToString();
                    AlarmId = alarmInfo.DisPlayInfo.ID;
                    AlarmSource = alarmInfo.DisPlayInfo.Source.ToString();
                    Keyword = alarmInfo.DisPlayInfo.Keyword;
                    AlarmContent = alarmInfo.DisPlayInfo.AlarmContent;

                    if (alarmInfo.DisPlayInfo.AppealStatus != 0)
                    {
                        vehicleAlarmServiceClient.GetApealDisposeByAlarmIDAsync(_AlarmId);
                    }

                    if (alarmInfo.DisPlayInfo.TransferStatus != 0)
                    {
                        vehicleAlarmServiceClient.GetTransferDisposeByAlarmIDAsync(_AlarmId);
                    }
                    this.Locate = Latitude + " " + Longitude;
                }
                else
                {
                    _AlarmInformation = new AlarmInfoEx();
                    VeAlarmid = string.Empty;
                    Longitude = string.Empty;
                    Latitude = string.Empty;
                    AlarmTime = string.Empty;
                    AlarmId = string.Empty;
                }

                if (alarmInfo.DisPlayInfo != null)
                {
                    if (this.client == null)
                    {
                        this.InilitClient();
                    }
                    this.client.GetChauffeurByVehicleAsync(alarmInfo.DisPlayInfo.VehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    if (!string.IsNullOrEmpty(alarmInfo.DisPlayInfo.DistrictCode.Trim()))
                    {
                        if (alarmInfo.DisPlayInfo.DistrictCode.Length == 5)
                        {
                            string provicecode = alarmInfo.DisPlayInfo.DistrictCode.Substring(0, 2);
                            var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == provicecode);

                            if (province != null)
                            {
                                this.DistrictCodeDescribe = province.Name + "/";
                            }
                            var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(n => n.Code == alarmInfo.DisPlayInfo.DistrictCode);
                            if (city != null)
                            {
                                this.DistrictCodeDescribe = this.DistrictCodeDescribe + city.Name;
                            }
                        }
                    }
                }

                SelectItemIndex = 0;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Latitude));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Longitude));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmTime));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmInfo));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AppealVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmSource));

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DisposeVisible));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }

    public class AlarmInfoDisplay
    {
        public AlarmInfoEx DisPlayInfo { get; set; }
    }
}

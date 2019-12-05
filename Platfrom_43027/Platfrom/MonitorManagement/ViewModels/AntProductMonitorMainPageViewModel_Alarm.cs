using GisManagement.Models;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.ServiceReference.EmailService;
using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Jounce.Framework;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using System.Reflection;
using Gsafety.Common.Controls;
using System.Linq;
using Gsafety.Ant.Monitor.Models;
using Gsafety.PTMS.Share.Model;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Common.Data.Enum;  
using Gsafety.Common.Converts;
using System.Text;
using Gsafety.PTMS.Monitor.Views;
using ESRI.ArcGIS.Client.Geometry;

namespace Gsafety.Ant.Monitor.ViewModels
{
    public partial class AntProductMonitorMainPageViewModel
    {
        private VedioServiceClient vedioServiceClient = null;
        private EmailServiceClient emailclient = null;

        private AlarmInfoEx _CurrentUnHandedAlarmInfo;

        /// <summary>
        /// License plate number
        /// </summary>
        private string _UnHandleFiledCarNumber;

        private bool _IsUnhandledBusy;

        PagedCollectionView _UnHandledAlarmPagedCV;


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

        public string UnHandleFiledCarNumber
        {
            get { return _UnHandleFiledCarNumber; }
            set
            {
                _UnHandleFiledCarNumber = value.Trim();
            }
        }

        public ICommand GetUnhandleAlarmCommand { get; private set; }
        public ICommand HappenLoctionCommand { get; private set; }
        public ICommand UnHandedAlarmVedio1First15SCommand { get; private set; }
        public ICommand HandleAlarmCommand { get; private set; }
        public ICommand AlarmSendEmailCommond { get; private set; }
        public Visibility HandleAlarmVisibility { get; set; }

        ////Select the object row
        public AlarmInfoEx CurrentUnHandedAlarmInfo
        {
            get
            {
                return _CurrentUnHandedAlarmInfo;
            }
            set
            {
                if (_CurrentUnHandedAlarmInfo != value)
                {
                    AlarmInfoEx older = _CurrentUnHandedAlarmInfo;
                    _CurrentUnHandedAlarmInfo = value;
                    RaisePropertyChanged(() => CurrentUnHandedAlarmInfo);
                    if (value == null)
                    {
                        return;
                    }

                    LocateCar(value.VehicleId);


                    OnSelectedAlarmChanged(older, _CurrentUnHandedAlarmInfo);
                    MonitorAlarmInfoDisplay displayinfo = new MonitorAlarmInfoDisplay();
                    displayinfo.DisPlayInfo = CurrentUnHandedAlarmInfo;
                    EventAggregator.Publish<MonitorAlarmInfoDisplay>(displayinfo);                  
                }
            }
        }

        private void OnSelectedAlarmChanged(AlarmInfoEx oldValue, AlarmInfoEx newValue)
        {
            try
            {
                if ((oldValue != null) && (newValue != null))
                {
                    //清除之前的事发地定位
                    EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = oldValue.VehicleId, Direction = oldValue.Direction, AlarmTime = oldValue.AlarmTime, GpsTime = oldValue.GpsTime, Speed = oldValue.Speed, GpsValid = oldValue.GpsValid, Latitude = oldValue.Latitude, Longitude = oldValue.Longitude, Op = 0, VehicleId = oldValue.VehicleId });

                    if (oldValue.VehicleId == newValue.VehicleId)
                    {
                        LocateCar(newValue.VehicleId);
                        return;
                    }
                }
                if (oldValue != null)
                {
                    //如果不是监控列表中则取消订阅
                    //if (CanUnbindGPS(oldValue.VehicleId)) MonitorGPS(oldValue.VehicleId, string.Empty, false, false, false);
                }

                if (newValue != null)
                {
                    //订阅
                    string departmentname = GetOrganizationName(newValue.VehicleId);
                    bool hasalert = ApplicationContext.Instance.BufferManager.VehicleAlertManager.HasAlert(newValue.VehicleId);
                    bool hasalarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(newValue.VehicleId);
                    MonitorGPS(newValue.VehicleId, departmentname, true, hasalarm, hasalert);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private string GetOrganizationName(string VehicleId)
        {
            foreach (var item in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
            {
                if (item.VehicleId == VehicleId)
                {
                    foreach (var org in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                    {
                        if (org.ID == item.OrganizationID)
                        {
                            return org.Name;
                        }
                    }
                }
            }
            return string.Empty;
        }
        protected FunItemVisibilityConverter converter = new FunItemVisibilityConverter();
        private void InitalAlarm()
        {
            try
            {
                vedioServiceClient = ServiceClientFactory.Create<VedioServiceClient>();
                vedioServiceClient.GetAlarmFiftyVideoAppealCompleted += vedioServiceClient_GetAlarmFiftyVideoAppealCompleted;
                emailclient = ServiceClientFactory.Create<EmailServiceClient>();
                emailclient.SendEmailCompleted += emailclient_SendEmailCompleted;
                Visibility visibility = (Visibility)converter.Convert("02-01-01-02-08", null, "02-01-01-02-08", null);

                if (ApplicationContext.Instance.AuthenticationInfo.TransferMode != 1 && visibility == Visibility.Visible)
                {
                    HandleAlarmVisibility = Visibility.Visible;
                }
                else
                {
                    HandleAlarmVisibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlarmMenuPageVm()", ex);
            }

            _UnHandledAlarmPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo);

            GetUnhandleAlarmCommand = new ActionCommand<object>(obj => GetUnhandleAlarmAction(1));

            ////Sign untreated a key alarm event details
            UnHandedAlarmVedio1First15SCommand = new ActionCommand<object>(obj => UnHandedAlarmVedio1First15SAction());

            HandleAlarmCommand = new ActionCommand<object>((obj) => HandleAlarm_Event(obj));
            AlarmSendEmailCommond = new ActionCommand<object>((obj) => AlarmSendEmail_Event(obj));
            HappenLoctionCommand = new ActionCommand<object>((obj) => HappenLoctionCommand_Event(obj));
        }

        void emailclient_SendEmailCompleted(object sender, SendEmailCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendFailure"), MessageDialogButton.Ok);           
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {

                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendFailure"), MessageDialogButton.Ok);

                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ALARM_SendSuc"), MessageDialogButton.Ok);

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private void AlarmSendEmail_Event(object obj)
        {
            EmailInfo email = new EmailInfo();
            string spaces = "   ";
            string br1 = "<br>";
            string br2 = "</br>";
            if (CurrentUnHandedAlarmInfo != null)
            {
                StringBuilder strBuilder = new System.Text.StringBuilder();

              

                string strVehicleType = string.Empty;
                if (CurrentUnHandedAlarmInfo.VehicleType != null)
                {
                    strVehicleType = CurrentUnHandedAlarmInfo.VehicleType;
                   
                }

                strBuilder.Append("<table width=680>");
                strBuilder.Append("<tr><td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_ProvinceName"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.Province);
                strBuilder.Append("</td>");
                strBuilder.Append("<td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_CityName"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.City);
                strBuilder.Append("</td></tr>");

                strBuilder.Append("<tr><td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_VehicleType"));
                strBuilder.Append(spaces);
                strBuilder.Append(strVehicleType);
                strBuilder.Append("</td>");
                strBuilder.Append("<td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleId"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.VehicleId);
                strBuilder.Append("</td></tr>");

                strBuilder.Append("<tr><td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_SecuritySuitID"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.MdvrCoreId);
                strBuilder.Append("</td>");
                strBuilder.Append("<td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_AlarmTime"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.AlarmTime);
                strBuilder.Append("</td></tr>");

                strBuilder.Append("<tr><td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lon"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.Longitude);
                strBuilder.Append("</td>");
                strBuilder.Append("<td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Lat"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.Latitude);
                strBuilder.Append("</td></tr>");

                strBuilder.Append("<tr><td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Speed"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.Speed);
                strBuilder.Append("</td>");
                strBuilder.Append("<td>");
                strBuilder.Append(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_Dir"));
                strBuilder.Append(spaces);
                strBuilder.Append(CurrentUnHandedAlarmInfo.Direction);
                strBuilder.Append("</td></tr>");
               

                strBuilder.Append("</table>");

                email.MailBody = strBuilder.ToString();

                email.IsbodyHtml = true; //Whether it is HTML

                email.MailToArray = new ObservableCollection<string> { };
                //ChildSendMail child = new ChildSendMail(email);
                //child.Closed += (m, n) =>
                //{
                //    if (child.DialogResult == true)
                //    {

                //        try
                //        {
                //            email = child.mail;

                //            emailclient.SendEmailAsync(email);
                
                //        }
                //        catch (Exception ex)
                //        {
                            
                //        }
                      

                //    }
                //};
                //child.Show();
               
            }
        }


        private void HappenLoctionCommand_Event(object obj)
        {
            AlarmInfoEx alarminfo = obj as AlarmInfoEx;
            if (alarminfo != null)
            {
                if (!GPSState.Valid(alarminfo.GpsValid))
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                    return;
                }

                EventAggregator.Publish<AlarmLocationAddRemoveArgs>(new AlarmLocationAddRemoveArgs() { MdvrCoreId = alarminfo.VehicleId, Direction = alarminfo.Direction, AlarmTime = alarminfo.AlarmTime, GpsTime = alarminfo.GpsTime, Speed = alarminfo.Speed, GpsValid = alarminfo.GpsValid, Latitude = alarminfo.Latitude, Longitude = alarminfo.Longitude, Op = 1, VehicleId = alarminfo.VehicleId });
            }
        }

        private void HandleAlarm_Event(object obj)
        {
            if (_CurrentUnHandedAlarmInfo != null)
            {
                EventAggregator.Publish<AlarmHandlerDispayArgs>(new AlarmHandlerDispayArgs() { Show = true });
            }
        }


        public void HandleEvent(AlarmHandleResult publishedEvent)
        {
            try
            {
                //处理完成，发送通知
                AlarmInfoEx alarminfo = ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo.First(n => n.ID == publishedEvent.AlarmID);
                Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm ca = new Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm();
                ca.AlarmGuid = alarminfo.ID;
                ca.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                ca.AlarmTime = alarminfo.AlarmTime.Value.ToUniversalTime();
                ca.CompleteTime = publishedEvent.HandleTime;
                ca.HandlerID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                ca.IsRealAlarm = publishedEvent.AlarmFlag;
                ca.MdvrCoreId = alarminfo.MdvrCoreId;
                ca.VehicleID = alarminfo.VehicleId;

                alarminfo.AlarmStatus = 4;
                //alarminfo.HandleResult = 2;
                //alarminfo. = ca.CompleteTime;
                alarminfo.AppealStatus = 4;
                if (publishedEvent.IsTransfer)
                    alarminfo.TransferStatus = 4;
                alarminfo.IsHandled = true;
                alarminfo.AlarmContent = publishedEvent.Note;
                alarminfo.IncidentAddress = publishedEvent.IncidentAddress;
                alarminfo.IncidentLevel = publishedEvent.IncidentLevel;
                //_CurrentUnHandedAlarmInfo.IsAlive = false;
                alarminfo.IncidentType = publishedEvent.IncidentType;

                foreach (var item in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                {
                    if (item.VehicleId == ca.VehicleID)
                    {
                        ca.Organizations = new System.Collections.ObjectModel.ObservableCollection<string>();
                        ca.Organizations.Add(item.OrganizationID);
                        break;
                    }
                }

                if (publishedEvent.IsTransfer)
                {
                    PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx alarminfoex = new PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx();
                    alarminfoex.AdditionalInfo = alarminfo.AdditionalInfo;
                    alarminfoex.AlarmGuid = alarminfo.AlarmGuid;
                    alarminfoex.AlarmStatus = alarminfo.AlarmStatus;
                    alarminfoex.AlarmTime = alarminfo.AlarmTime;
                    alarminfoex.AppealStatus = alarminfo.AppealStatus;
                    alarminfoex.ButtonNum = alarminfo.ButtonNum;
                    alarminfoex.City = alarminfo.City;
                    alarminfoex.ClientId = alarminfo.ClientId;
                    alarminfoex.Direction = alarminfo.Direction;
                    alarminfoex.DisposalStatus = alarminfo.DisposalStatus;
                    alarminfoex.DistrictCode = alarminfo.DistrictCode;
                    alarminfoex.GpsTime = alarminfo.GpsTime;
                    alarminfoex.GpsValid = alarminfo.GpsValid;
                    alarminfoex.Height = alarminfo.Height;
                    alarminfoex.ID = alarminfo.ID;
                    alarminfoex.Latitude = alarminfo.Latitude;
                    alarminfoex.Longitude = alarminfo.Longitude;

                    if (alarminfoex.Longitude != null && alarminfoex.Latitude != null)
                    {

                        MapPoint mapPoint = Gsafety.Common.Transform.GeographicToWebMercator(new MapPoint(
                            Convert.ToDouble(alarminfoex.Longitude, System.Globalization.CultureInfo.InvariantCulture),
                            Convert.ToDouble(alarminfoex.Latitude, System.Globalization.CultureInfo.InvariantCulture)));

                        alarminfoex.Longitude = mapPoint.X.ToString().Replace(",",".");
                        alarminfoex.Latitude = mapPoint.Y.ToString().Replace(",", ".");
                    }
                    alarminfoex.MdvrCoreId = alarminfo.MdvrCoreId;
                    alarminfoex.OperationLincese = alarminfo.OperationLincese;
                    alarminfoex.Organizations = alarminfo.Organizations;
                    alarminfoex.OwnerPhone = alarminfo.OwnerPhone;
                    alarminfoex.Province = alarminfo.Province;
                    alarminfoex.Source = alarminfo.Source;
                    alarminfoex.Speed = alarminfo.Speed;
                    alarminfoex.SuiteID = alarminfo.SuiteID;
                    alarminfoex.SuiteInfoID = alarminfo.SuiteInfoID;
                    alarminfoex.SuiteStatus = alarminfo.SuiteStatus;
                    alarminfoex.TransferStatus = alarminfo.TransferStatus;
                    alarminfoex.User = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    alarminfoex.VehicleId = alarminfo.VehicleId;
                    alarminfoex.VehicleOwner = alarminfo.VehicleOwner;
                    alarminfoex.VehicleType = alarminfo.VehicleType;
                    alarminfoex.VehicleSn = alarminfo.VehicleSn;
                    alarminfoex.BrandModel = alarminfo.BrandModel;
                    alarminfoex.IncidentAddress = alarminfo.IncidentAddress;
                    alarminfoex.IncidentLevel = alarminfo.IncidentLevel;
                    alarminfoex.IncidentType = alarminfo.IncidentType;
                    alarminfoex.AlarmContent = alarminfo.AlarmContent;
                    alarminfoex.VehicleType = alarminfo.VehicleInfo.VehicleTypeDescribe;
                    alarminfoex.UserName = ApplicationContext.Instance.AuthenticationInfo.Account;
                    
                    ApplicationContext.Instance.MessageClient.TransferAlarm(alarminfoex);
                }

                ApplicationContext.Instance.MessageClient.SendCompleteAlarm(ca);
                ApplicationContext.Instance.BufferManager.AlarmManager.FireIfNoAlarm(alarminfo.VehicleId);

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void vedioServiceClient_GetAlarmFiftyVideoAppealCompleted(object sender, GetAlarmFiftyVideoAppealCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(LProxy.ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    if (result.Result.Count == 0)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("NoAlarmVideo"));
                        return;
                    }

                    var info = new MediaInfo()
                    {
                        MediaInfoItems = result.Result.Select(t => new MediaInfo.MediaInfoItem()
                        {
                            StartTime = t.StartTime.ToLocalTime(),
                            EndTime = t.EndTime.ToLocalTime(),
                            Url = t.FileID,
                            Channel = (int)t.Channel,
                            IsRealVideo = false,
                            IsShowControlBar = false,
                            IsShowProcessBar = false,
                            ShowRemoveBtn = false,
                        }).ToList(),
                        IsHideProgressControl = false,
                        VehicleId = _CurrentUnHandedAlarmInfo.VehicleId,
                        ShowHistoryLine = false,
                        AutoPlay = false,
                        Orientation = Orientation.Horizontal
                    };

                    LogVideoClient client = null;
                    try
                    {
                        var videoLogs = new ObservableCollection<LogVideo>();
                        foreach (var item in result.Result)
                        {
                            var log = new LogVideo()
                            {
                                ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                                Channel = (short)item.Channel,
                                ID = Guid.NewGuid().ToString(),
                                LogType = (short)VideoLogTypeEnum.Play,
                                MdvrCoreSn = CurrentUnHandedAlarmInfo.MdvrCoreId,
                                OperateTime = DateTime.UtcNow,
                                OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID,
                                OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName,
                                VehicleID = CurrentUnHandedAlarmInfo.VehicleId,
                            };
                            videoLogs.Add(log);
                        }

                        client = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.PTMSLogManageService.LogVideoClient>();
                        client.InsertVideoPlayLogAsync(videoLogs);
                    }
                    catch (System.Exception ex)
                    {
                    }
                    finally
                    {
                        if (client != null)
                        {
                            client.CloseAsync();
                        }
                    }

                    ApplicationContext.Instance.EventAggregator.Publish<MediaInfo>(info);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void ActivateAlarmView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                if (ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo.Count > 0)
                {
                    CurrentUnHandedAlarmInfo = ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo[0];
                }

                EventAggregator.Publish(MonitorName.MonitorAlarmHandleView.AsViewNavigationArgs());

                GetUnhandleAlarmAction(1); //add by penggl 2014-01-11

                EventAggregator.Publish(MonitorName.MonitorAlarmInfoView.AsViewNavigationArgs());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// Get untreated a key alarm (Status = 1)
        /// </summary>
        /// <param name="pageIndex"></param>
        private void GetUnhandleAlarmAction(int pageIndex)
        {
            try
            {
                _UnHandledAlarmPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.AlarmManager.AllAlarmInfo);
                _UnHandledAlarmPagedCV.Filter = null;

                if (!string.IsNullOrEmpty(UnHandleFiledCarNumber))
                {
                    _UnHandledAlarmPagedCV.Filter = new Predicate<object>(FilterVehicle);
                }

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UnHandledAlarmPagedCV));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private bool FilterVehicle(object obj)
        {
            AlarmInfoEx info = obj as AlarmInfoEx;

            return info.VehicleId.ToLower().Contains(UnHandleFiledCarNumber.Trim().ToLower());
        }

        /// <summary>
        /// Untreated one key way 15 seconds before the alarm a screen
        /// </summary>
        private void UnHandedAlarmVedio1First15SAction()
        {
            string alarmId = CurrentUnHandedAlarmInfo.ID;
            vedioServiceClient.GetAlarmFiftyVideoAppealAsync(alarmId);
        }

        public void HandleEvent(AlarmCountChange publishedEvent)
        {
            RaisePropertyChanged("UnHandledAlarmPagedCV");
        }
    }

    public class MonitorAlarmInfoDisplay
    {
        public AlarmInfoEx DisPlayInfo { get; set; }
    }

    public class MonitorAlertInfoDisplay
    {
        public BusinessAlertEx DisPlayInfo { get; set; }
    }

    public class MonitorDeviceAlertInfoDisplay
    {
        public Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx DisPlayInfo { get; set; }
    }
}

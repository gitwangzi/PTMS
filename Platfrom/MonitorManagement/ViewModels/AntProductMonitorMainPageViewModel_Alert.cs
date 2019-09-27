using GisManagement.Models;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.Models;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.ServiceReference.EmailService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Text;
using Gsafety.PTMS.Monitor.Views;
namespace Gsafety.Ant.Monitor.ViewModels
{
    public partial class AntProductMonitorMainPageViewModel
    {
        VehicleAlertServiceClient vehicleAlertServiceClient = null;

        public VehicleAlertType SelectedAlertType { get; set; }

        /// <summary>
        /// Unhandled Page
        /// </summary>
        PagedCollectionView _VehicleAlertUnHandledPagedCV;
        public PagedCollectionView VehicleAlertUnHandledPagedCV
        {
            get { return _VehicleAlertUnHandledPagedCV; }
        }

        //alert type list
        public ObservableCollection<VehicleAlertType> VehicleAlertTypeList { get; set; }

        /// <summary>
        /// Untreated Selected Row
        /// </summary>
        private Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx _selectedVehicleAlertModel;
        public Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx SelectedVehicleAlertModel
        {
            get
            {
                return this._selectedVehicleAlertModel;
            }
            set
            {
                if (_selectedVehicleAlertModel != value)
                {
                    var old = _selectedVehicleAlertModel;
                    _selectedVehicleAlertModel = value;
                    RaisePropertyChanged(() => this.SelectedVehicleAlertModel);

                    if (value == null)
                    {
                        return;
                    }

                    LocateCar(value.VehicleId);
                    OnSelectedAlertChanged(old, _selectedVehicleAlertModel);

                    MonitorAlertInfoDisplay info = new MonitorAlertInfoDisplay();
                    info.DisPlayInfo = SelectedVehicleAlertModel;
                    EventAggregator.Publish<MonitorAlertInfoDisplay>(info);                

                }
            }
        }

        private void OnSelectedAlertChanged(BusinessAlertEx oldValue, BusinessAlertEx newValue)
        {
            try
            {
                if ((oldValue != null) && (newValue != null))
                {
                    //清除之前的事发地定位
                    EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = oldValue.Direction, AlertTime = oldValue.AlertTime, GpsTime = oldValue.GpsTime, Speed = oldValue.Speed, Valid = oldValue.GpsValid, Latitude = oldValue.Latitude, Longitude = oldValue.Longitude, Op = 0, VehicleId = oldValue.VehicleId });

                    if (oldValue.VehicleId == newValue.VehicleId)
                    {
                        LocateCar(newValue.VehicleId);

                        return;
                    }
                }
                if (oldValue != null)
                {
                    //如果不是监控列表中则取消订阅
                   // if (CanUnbindGPS(oldValue.VehicleId)) MonitorGPS(oldValue.VehicleId, string.Empty, false, false, false);
                }

                if (newValue != null)
                {
                    //订阅
                    string departmentname = GetOrganizationName(newValue.VehicleId);
                    bool hasalarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(newValue.VehicleId);
                    MonitorGPS(newValue.VehicleId, departmentname, true, hasalarm, true);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private string _alertcarNumber;
        public string AlertCarNumber
        {
            get
            {
                return this._alertcarNumber;
            }
            set
            {
                this._alertcarNumber = value.Trim();
                RaisePropertyChanged(() => this.AlertCarNumber);
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get { return _EndTime; }
            set
            {
                _EndTime = value;
                if (value != null)
                {
                    _EndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }
            }
        }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        private VehicleAlertDetail currentalertdetail;
        Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx currentalert = new Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx();
        private int CurrentAccordionSelectIndex = 0;

        /// <summary>
        /// unhandle
        /// </summary>
        public ICommand GetVehicleAlertCommand { get; private set; }
        /// <summary>
        /// locate
        /// </summary>
        public ICommand LocatePositionCommand { get; private set; }

        public ICommand HandleAlertCommand { get; private set; }

        public ICommand AlertSendEmailCommond { get; private set; }

        public void InitalAlert()
        {
            try
            {
                //initialize client
                vehicleAlertServiceClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();

                vehicleAlertServiceClient.GetVehicleAlertDetailCompleted += vehicleAlertServiceClient_GetVehicleAlertDetailCompleted;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

            _VehicleAlertUnHandledPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.VehicleAlertManager.VehicleAlert);

            //initialize command
            GetVehicleAlertCommand = new ActionCommand<string>(obj => GetVehicleAlertAction(1));
            LocatePositionCommand = new ActionCommand<object>(obj => locatepositionAction(obj));
            HandleAlertCommand = new ActionCommand<string>(obj => HandleAction(obj));
            AlertSendEmailCommond = new ActionCommand<object>((obj) => AlertSendEmail_Event(obj));
            var temp = new EnumAdapter<BusinessAlertType>().GetEnumInfos();

            VehicleAlertTypeList = new ObservableCollection<VehicleAlertType>();

            foreach (var item in temp)
            {
                if (item.Value < 4)
                {
                    VehicleAlertTypeList.Add(new VehicleAlertType
                    {
                        Code = (short)item.Value,
                        Name = item.LocalizedString
                    });
                }
            }
            //VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = -1, Name = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect") });
            VehicleAlertTypeList.Insert(0, new VehicleAlertType { Code = -1, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
            SelectedAlertType = VehicleAlertTypeList[0];
        }      


        private void AlertSendEmail_Event(object obj)
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

        protected void ActivateAlertView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                ApplicationContext.Instance.CurrentView = 2;
                EventAggregator.Publish(MonitorName.MonitorAlertInfoView.AsViewNavigationArgs());
                EventAggregator.Publish(MonitorName.MonitorAlertHandleView.AsViewNavigationArgs());

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void locatepositionAction(object obj)
        {
            try
            {
                currentalert = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx;

                if (currentalert != null)
                {
                    if (!GPSState.Valid(currentalert.GpsValid))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                        return;
                    }

                    EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = currentalert.Direction, AlertTime = currentalert.AlertTime, GpsTime = currentalert.GpsTime, Speed = currentalert.Speed, Valid = currentalert.GpsValid, Latitude = currentalert.Latitude, Longitude = currentalert.Longitude, Op = 1, VehicleId = currentalert.VehicleId });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }

        private void vehicleAlertServiceClient_GetVehicleAlertDetailCompleted(object sender, GetVehicleAlertDetailCompletedEventArgs e)
        {
            try
            {
                currentalertdetail = new VehicleAlertDetail();//Add By penggl
                var Alertdeatil = new VehicleAlertDetail();
                if (e.Error == null || e.Result != null)
                {
                    if (e.Result.Result != null)
                    {
                        currentalertdetail = e.Result.Result;//Add By penggl
                        Alertdeatil = e.Result.Result;
                        EventAggregator.Publish<VehicleAlertDetail>(Alertdeatil);
                        if (CurrentAccordionSelectIndex == 1)
                        {
                            //EventAggregator.Publish<VehicleAlertEx>(SelectedVehicleAlertHandledModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void GetDeatilAlertModel(Gsafety.PTMS.ServiceReference.VehicleAlertService.VehicleAlert _selectedVehicleAlertModel)
        {
            vehicleAlertServiceClient.GetVehicleAlertDetailAsync(_selectedVehicleAlertModel.Id);
        }

        private void GetDeatilAlertModel(VehicleAlertEx _selectedVehicleAlertHandledModel)
        {
            vehicleAlertServiceClient.GetVehicleAlertDetailAsync(_selectedVehicleAlertHandledModel.Id1);
        }

        private void GetVehicleAlertAction(int pageIndex)
        {
            try
            {
                if (this.SelectedAlertType.Code == -1)
                {
                    if (!string.IsNullOrEmpty(AlertCarNumber))
                    {
                        VehicleAlertUnHandledPagedCV.Filter = null;
                        VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterCarNum);
                    }
                    else
                    {
                        VehicleAlertUnHandledPagedCV.Filter = null;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(AlertCarNumber))
                    {
                        VehicleAlertUnHandledPagedCV.Filter = null;
                        VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterCarNumAndAlerttype);
                    }
                    else
                    {
                        VehicleAlertUnHandledPagedCV.Filter = null;
                        VehicleAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterAlerttype);
                    }
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlertUnHandledPagedCV));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region FilterActions
        private bool FileterCarNumAndAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx;

            return alertinfo.VehicleId.Contains(AlertCarNumber.Trim()) && (alertinfo.AlertType.Equals(this.SelectedAlertType.Code));
        }

        private bool FileterAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx;
            return alertinfo.AlertType.Equals(this.SelectedAlertType.Code);
        }

        private bool FileterCarNum(object obj)
        {
            Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.VehicleAlertService.BusinessAlertEx;
            return alertinfo.VehicleId.ToLower().Contains(AlertCarNumber.Trim().ToLower());
        }
        #endregion

        private void HandleAction(string obj)
        {
            try
            {
                if (SelectedVehicleAlertModel != null)
                {
                    EventAggregator.Publish<AlertHandleDisplayArgs>(new AlertHandleDisplayArgs() { Show = true });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }

        private void TrealUnhandleAction()
        {
            try
            {
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleAlertUnHandledPagedCV));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }

        }

        public void HandleEvent(AlertHandleResult publishedEvent)
        {
            try
            {
                BusinessAlertEx alertinfo = ApplicationContext.Instance.BufferManager.VehicleAlertManager.VehicleAlert.FirstOrDefault(n => n.Id == publishedEvent.AlertID);
                if (alertinfo != null)
                {
                    alertinfo.Status = 4;
                    alertinfo.Note = publishedEvent.Content;
                    alertinfo.HandleTime = publishedEvent.HandleTime;

                    //处理完成，发送通知
                    Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlert ca = new Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlert();
                    ca.AlertID = alertinfo.Id;
                    ca.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                    ca.MdvrCoreId = alertinfo.MdvrCoreId;
                    ca.VehicleID = alertinfo.VehicleId;

                    foreach (var item in ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList)
                    {
                        if (item.VehicleId == ca.VehicleID)
                        {
                            ca.Organizations = new System.Collections.ObjectModel.ObservableCollection<string>();
                            ca.Organizations.Add(item.OrganizationID);
                            break;
                        }
                    }

                    ApplicationContext.Instance.MessageClient.SendCompleteAlert(ca);

                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}

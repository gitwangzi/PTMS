using BaseLib.Model;
using BaseLib.ViewModels;
using GisManagement.Models;
using Gsafety.Ant.Monitor.Models;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.Monitor.Models;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Text;
using Gsafety.PTMS.ServiceReference.EmailService;
using Gsafety.PTMS.Monitor.Views;
namespace Gsafety.Ant.Monitor.ViewModels
{
    public partial class AntProductMonitorMainPageViewModel
    {
        DeviceAlertServiceClient vehicleDeviceAlertServiceClient = null;

        Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx currentdevicealert = new Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx();


        private ObservableCollection<VehicleAlertType> _zAlertTypes = new ObservableCollection<VehicleAlertType>();
        /// <summary>
        /// 车辆情况
        /// </summary>
        public ObservableCollection<VehicleAlertType> ZAlertTypes
        {
            get { return _zAlertTypes; }
            set
            {
                _zAlertTypes = value;
                RaisePropertyChanged(() => ZAlertTypes);
            }
        }

        private VehicleAlertType searchByAlertType;
        /// <summary>
        /// 
        /// </summary>
        public VehicleAlertType SearchByAlertType
        {
            get
            {
                return searchByAlertType;
            }
            set
            {
                this.searchByAlertType = value;
                RaisePropertyChanged(() => this.SearchByAlertType);
            }
        }

        /// <summary>
        /// Unhandled Page
        /// </summary>
        PagedCollectionView _VehicleDeviceAlertUnHandledPagedCV;
        public PagedCollectionView VehicleDeviceAlertUnHandledPagedCV
        {
            get { return _VehicleDeviceAlertUnHandledPagedCV; }
        }



        /// <summary>
        /// Untreated Selected Row
        /// </summary>
        private Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx _selectedVehicleDeviceAlertModel;
        public Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx SelectedVehicleDeviceAlertModel
        {
            get
            {
                return this._selectedVehicleDeviceAlertModel;
            }
            set
            {
                if (_selectedVehicleDeviceAlertModel != value)
                {
                    var old = _selectedVehicleDeviceAlertModel;
                    _selectedVehicleDeviceAlertModel = value;
                    RaisePropertyChanged(() => this.SelectedVehicleDeviceAlertModel);

                    if (value == null)
                    {
                        return;
                    }

                     LocateCar(value.VehicleId);
                    OnSelectedDeviceAlertChanged(old, _selectedVehicleDeviceAlertModel);

                    MonitorDeviceAlertInfoDisplay info = new MonitorDeviceAlertInfoDisplay();
                    info.DisPlayInfo = _selectedVehicleDeviceAlertModel;
                    EventAggregator.Publish<MonitorDeviceAlertInfoDisplay>(info);

                }
            }
        }

        private void OnSelectedDeviceAlertChanged(DeviceAlertEx oldValue, DeviceAlertEx newValue)
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
                  //  if (CanUnbindGPS(oldValue.VehicleId)) MonitorGPS(oldValue.VehicleId, string.Empty, false, false, false);
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

        private string _devicealertcarNumber;
        public string DeviceAlertCarNumber
        {
            get
            {
                return this._devicealertcarNumber;
            }
            set
            {
                this._devicealertcarNumber = value.Trim();
                RaisePropertyChanged(() => this.DeviceAlertCarNumber);
            }
        }

        private DateTime? _DeviceEndTime;
        public DateTime? DeviceEndTime
        {
            get { return _DeviceEndTime; }
            set
            {
                _DeviceEndTime = value;
                if (value != null)
                {
                    _DeviceEndTime = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
                }
            }
        }

        private DateTime? _DeviceStartTime;
        public DateTime? DeviceStartTime
        {
            get { return _DeviceStartTime; }
            set { _DeviceStartTime = value; }
        }


        private int DeviceCurrentAccordionSelectIndex = 0;

        /// <summary>
        /// unhandle
        /// </summary>
        public ICommand GetVehicleDeviceAlertCommand { get; private set; }
        /// <summary>
        /// locate
        /// </summary>
        public ICommand DeviceLocatePositionCommand { get; private set; }

        public ICommand DeviceAlertSendEmailCommond { get; private set; }

        public void InitalDeviceAlert()
        {
            try
            {
                //initialize client
                vehicleDeviceAlertServiceClient = ServiceClientFactory.Create<DeviceAlertServiceClient>();

                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.MAINTAIN_Full_N,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("All")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSModelError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSModelError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSNoAntenna,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSNoAntenna")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.GNSSCircuit,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("GNSSCircuit")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.PowerSourceNoVoltage,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoVoltage")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.PowerSourceNoPower,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("PowerSourceNoPower")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.LEDError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("LEDError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.TTSError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("TTSError")
                });
                ZAlertTypes.Add(new VehicleAlertType()
                {
                    Code = (int)DeviceAlertTypes.VidiconError,
                    Name = ApplicationContext.Instance.StringResourceReader.GetString("VidiconError")
                });

                SearchByAlertType = ZAlertTypes[0];
                _VehicleDeviceAlertUnHandledPagedCV = new PagedCollectionView(ApplicationContext.Instance.BufferManager.VehicleDeviceAlertManage.VehicleDeviceAlert);
                DeviceAlertSendEmailCommond = new ActionCommand<object>((obj) => DeviceAlertSendEmail_Event(obj));
                //initialize command
                GetVehicleDeviceAlertCommand = new ActionCommand<string>(obj => GetVehicleDeviceAlertAction(1));
                DeviceLocatePositionCommand = new ActionCommand<object>(obj => devicelocatepositionAction(obj));

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }
       


        private void DeviceAlertSendEmail_Event(object obj)
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

        protected void ActivateDeviceAlertView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                ApplicationContext.Instance.CurrentView = 2;
                EventAggregator.Publish(MonitorName.MonitorDeviceAlertInfoView.AsViewNavigationArgs());
                //EventAggregator.Publish(MonitorName.MonitorAlertHandleView.AsViewNavigationArgs());

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void devicelocatepositionAction(object obj)
        {
            try
            {
                currentdevicealert = obj as Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx;

                if (currentdevicealert != null)
                {
                    if (!GPSState.Valid(currentdevicealert.GpsValid))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_UNValidXY"));
                        return;
                    }

                    EventAggregator.Publish<AlertAddRemoveCurrentPosition>(new AlertAddRemoveCurrentPosition() { Direction = currentdevicealert.Direction, AlertTime = currentdevicealert.AlertTime, GpsTime = currentdevicealert.GpsTime, Speed = currentdevicealert.Speed, Valid = currentdevicealert.GpsValid, Latitude = currentdevicealert.Latitude, Longitude = currentdevicealert.Longitude, Op = 1, VehicleId = currentdevicealert.VehicleId });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleAlertViewModel", ex);
            }
        }


        private void GetVehicleDeviceAlertAction(int pageIndex)
        {
            try
            {
                if ((int)this.SearchByAlertType.Code == -1)
                {
                    if (!string.IsNullOrEmpty(DeviceAlertCarNumber))
                    {
                        VehicleDeviceAlertUnHandledPagedCV.Filter = null;
                        VehicleDeviceAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterDeviceCarNum);
                    }
                    else
                    {
                        VehicleDeviceAlertUnHandledPagedCV.Filter = null;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(DeviceAlertCarNumber))
                    {
                        VehicleDeviceAlertUnHandledPagedCV.Filter = null;
                        VehicleDeviceAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterDeviceCarNumAndAlerttype);
                    }
                    else
                    {
                        VehicleDeviceAlertUnHandledPagedCV.Filter = null;
                        VehicleDeviceAlertUnHandledPagedCV.Filter = new Predicate<object>(FileterDeviceAlerttype);
                    }
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleDeviceAlertUnHandledPagedCV));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region FilterActions
        private bool FileterDeviceCarNumAndAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx;

            return alertinfo.VehicleId.Contains(DeviceAlertCarNumber.Trim()) && (alertinfo.AlertType.Equals(this.SearchByAlertType.Code));
        }

        private bool FileterDeviceAlerttype(object obj)
        {
            Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx;
            return alertinfo.AlertType.Equals(this.SearchByAlertType.Code);
        }

        private bool FileterDeviceCarNum(object obj)
        {
            Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx alertinfo = obj as Gsafety.PTMS.ServiceReference.DeviceAlertService.DeviceAlertEx;
            return alertinfo.VehicleId.ToLower().Contains(DeviceAlertCarNumber.Trim().ToLower());
        }
        #endregion


    }

    public enum DeviceAlertTypes
    {
        MAINTAIN_Full_N = -1,
        GNSSModelError = 0,
        GNSSNoAntenna = 1,
        GNSSCircuit = 2,
        PowerSourceNoVoltage = 3,
        PowerSourceNoPower = 4,
        LEDError = 5,
        TTSError = 6,
        VidiconError = 7,

    }
}

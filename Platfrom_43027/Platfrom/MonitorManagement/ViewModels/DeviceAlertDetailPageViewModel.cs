using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorDeviceAlertInfoViewModel)]
    public class DeviceAlertDetailPageViewModel : BaseViewModel, IEventSink<MonitorDeviceAlertInfoDisplay>, IEventSink<int>, IPartImportsSatisfiedNotification
    {

        #region 构造函数



        public DeviceAlertDetailPageViewModel()
        {
            m_IsOpen = true;
            _IsVisual = true;
            this.DistrictCodeDescribe = "";
            this.Locate = "";
            this.OrganizationName = "";
            this.ChauffeurList = new ObservableCollection<Chauffeur>();
            HandleresultVisible = Visibility.Collapsed;
            this.Note = "";
            this.AlertHandlePerson = "";
            this.AlertAlertTime = "";
            this.IsNoteVisibility = Visibility.Collapsed;
            this.InilitClient();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HandleresultVisible));
        }


        #endregion

        #region 属性

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

        public string Title { get; set; }

        public string PicUrl { get; set; }

        public string _alertID = string.Empty;

        private int _SelectItemIndex = 0;
        public int SelectItemIndex
        {
            get { return _SelectItemIndex; }
            set { _SelectItemIndex = value; }
        }

        private Visibility _HandleresultVisible;
        public Visibility HandleresultVisible
        {
            get { return _HandleresultVisible; }
            set { _HandleresultVisible = value; }
        }

        private Visibility _AlertDetailVisible;
        public Visibility AlertDetailVisible
        {
            get { return _AlertDetailVisible; }
            set { _AlertDetailVisible = value; }
        }

        private string _vehicleid;
        public string VehicleId
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                _vehicleid = value;
                RaisePropertyChanged("VehicleId");
            }
        }

        private string _vehicleowner;
        public string VehicleOwner
        {
            get
            {
                return _vehicleowner;
            }
            set
            {
                _vehicleowner = value;
                RaisePropertyChanged("VehicleOwner");
            }
        }

        private string _ownerphone;
        public string OwnerPhone
        {
            get
            {
                return _ownerphone;
            }
            set
            {
                _ownerphone = value;
                RaisePropertyChanged("OwnerPhone");
            }
        }

        private string _province;
        public string Province
        {
            get
            {
                return _province;
            }
            set
            {
                _province = value;
                RaisePropertyChanged("Province");
            }
        }

        private string _city;
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                RaisePropertyChanged("City");
            }
        }

        private int? _alerttype;
        public int? AlertType
        {
            get
            {
                return _alerttype;
            }
            set
            {
                _alerttype = value;
                RaisePropertyChanged("AlertType");
            }
        }

        private string _gpsvalid;
        public string GpsValid
        {
            get
            {
                return _gpsvalid;
            }
            set
            {
                _gpsvalid = value;
                RaisePropertyChanged("GpsValid");
            }
        }

        private string _longitude;
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        private string _latitude;
        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                RaisePropertyChanged("Latitude");
            }
        }

        private string _height;
        public string Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
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

        private string _alerttime;
        public string AlertTime
        {
            get
            {
                return _alerttime;
            }
            set
            {
                _alerttime = value;
                RaisePropertyChanged("AlertTime");
            }
        }

        private string _suiteid;
        public string SuiteID
        {
            get
            {
                return _suiteid;
            }
            set
            {
                _suiteid = value;
                RaisePropertyChanged("SuiteID");
            }
        }



        private bool m_IsOpen = false;
        public bool IsOpen
        {
            get { return m_IsOpen; }
            set
            {
                // m_IsOpen = value;
                // _IsVisual = value;
            }
        }

        private bool _IsVisual = false;
        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                //if (IsOpen)
                //{
                //    IsOpen = _IsVisual = false;
                //}
                //else
                //{
                //    IsOpen = _IsVisual = true;
                //}
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
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

        private string _organizationName;
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
                RaisePropertyChanged(() => OrganizationName);
            }
        }

        private string _note;
        /// <summary>
        /// 车辆告警处置内容
        /// </summary>
        public string Note
        {
            get
            {
                return this._note;
            }
            set
            {
                this._note = value;
                RaisePropertyChanged(() => this.Note);
            }
        }

        private Visibility _isNoteVisibiltiy;
        public Visibility IsNoteVisibility
        {
            get
            {
                return this._isNoteVisibiltiy;
            }
            set
            {
                this._isNoteVisibiltiy = value;
                RaisePropertyChanged(() => IsNoteVisibility);
            }
        }

        private string _alertHandlePerson;
        /// <summary>
        /// 车辆告警处置人
        /// </summary>
        public string AlertHandlePerson
        {
            get
            {
                return this._alertHandlePerson;
            }
            set
            {
                this._alertHandlePerson = value;
                RaisePropertyChanged(() => AlertHandlePerson);
            }
        }

        private string _alertAlertTime;
        /// <summary>
        /// 车辆告警处置时间
        /// </summary>
        public string AlertAlertTime
        {
            get
            {
                return this._alertAlertTime;
            }
            set
            {
                this._alertAlertTime = value;
                RaisePropertyChanged(() => AlertAlertTime);
            }
        }


        #endregion

        private ChauffeurServiceClient client;
        private VehicleAlertServiceClient _vehicleAlertClient;
        private DisplayLonConvert _lonConverter = new DisplayLonConvert();
        private DisplayLatConvert _latConverter = new DisplayLatConvert();

        //private GPSLonToSixtyConvert _slonConverter = new GPSLonToSixtyConvert();
        //private GPSLatToSixtyConvert _slatConverter = new GPSLatToSixtyConvert();
        #region  方法


        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MonitorDeviceAlertInfoDisplay>(this);
            EventAggregator.SubscribeOnDispatcher<int>(this);
        }

        private void InilitClient()
        {
            //client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            //client.GetChauffeurByVehicleCompleted += client_GetChauffeurByVehicleCompleted;
            //this._vehicleAlertClient = ServiceClientFactory.Create<VehicleAlertServiceClient>();
            //this._vehicleAlertClient.GetVehicleAlertDisposeInfoCompleted += _vehicleAlertClient_GetVehicleAlertDisposeInfoCompleted;
        }

        //void _vehicleAlertClient_GetVehicleAlertDisposeInfoCompleted(object sender, GetVehicleAlertDisposeInfoCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Cancelled)
        //        {
        //            return;
        //        }
        //        if (e.Error != null)
        //        {
        //            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
        //            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
        //        }
        //        if (e.Result.IsSuccess == false)
        //        {
        //            if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
        //            {
        //                ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
        //                    e.Result.ErrorMsg);
        //            }
        //            else
        //            {
        //                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
        //                    e.Result.ExceptionMessage);
        //            }
        //        }
        //        else
        //        {
        //            var target = e.Result.Result;

        //            if (target != null)
        //            {
        //                this.Note = target.Note;
        //                this.AlertAlertTime = target.HandleTime.HasValue ? target.HandleTime.Value.ToLocalTime().ToString() : "";
        //                this.AlertHandlePerson = target.HandlePerson;
        //            }
        //            else
        //            {
        //                this.Note = "";
        //                this.AlertHandlePerson = "";
        //                this.AlertAlertTime = "";
        //            }
        //            if (!string.IsNullOrEmpty(this.Note))
        //            {
        //                this.IsNoteVisibility = Visibility.Visible;
        //            }
        //            else
        //            {
        //                this.IsNoteVisibility = Visibility.Collapsed;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel._vehicleAlertClient_GetVehicleAlertDisposeInfoCompleted", ex);
        //    }
        //    finally
        //    {
        //        if (this.client != null)
        //        {
        //            this.client.CloseAsync();
        //        }
        //        this.client = null;
        //    }
        //}

        //void client_GetChauffeurByVehicleCompleted(object sender, GetChauffeurByVehicleCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Cancelled)
        //        {
        //            return;
        //        }
        //        if (e.Error != null)
        //        {
        //            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
        //            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
        //        }
        //        if (e.Result.IsSuccess == false)
        //        {
        //            if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
        //            {
        //                ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
        //                    e.Result.ErrorMsg);
        //            }
        //            else
        //            {
        //                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
        //                    e.Result.ExceptionMessage);
        //            }
        //        }
        //        else
        //        {
        //            this.ChauffeurList = new ObservableCollection<Chauffeur>();
        //            if (e.Result.Result.Count > 0)
        //            {
        //                this.ChauffeurList = e.Result.Result;
        //            }
        //            else
        //            {
        //                this.ChauffeurList.Insert(0, new Chauffeur()
        //                {
        //                    ID = Guid.NewGuid().ToString(),
        //                    Name = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
        //                    Phone = ApplicationContext.Instance.StringResourceReader.GetString("Null"),
        //                });
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel.client_GetChauffeurVehicleCompleted", ex);
        //    }
        //    finally
        //    {
        //        if (this.client != null)
        //        {
        //            this.client.CloseAsync();
        //        }
        //        this.client = null;
        //    }
        //}

        public void HandleEvent(MonitorDeviceAlertInfoDisplay alertinfo)
        {
            try
            {
                //if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "V"))
                //{
                //    GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSERROR");
                //}
                //if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "A"))
                //{
                //    GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSAvailable");
                //}
                //if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "N"))
                //{
                //    GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSNone");
                //}

                Longitude = "-";
                Latitude = "-";
                if (alertinfo.DisPlayInfo.GpsValid != null)
                {
                    if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "V"))
                    {
                        GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSERROR");
                        Height = "-";
                        Speed = "-";
                        Direction = "-";
                    }
                    if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "A"))
                    {
                        GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSAvailable");
                        Longitude = alertinfo.DisPlayInfo.Longitude;
                        Latitude = alertinfo.DisPlayInfo.Latitude;
                        Direction = alertinfo.DisPlayInfo.Direction;
                        Speed = alertinfo.DisPlayInfo.Speed;

                        var tempLat = _latConverter.ConvertBack(Latitude, null, null, null);
                        var resultLat = _latConverter.ConvertToWESN(tempLat, null, null, null);

                        var tempLon = _lonConverter.ConvertBack(Longitude, null, null, null);
                        var resultLon = _lonConverter.ConvertToWESN(tempLon, null, null, null);

                        //resultLat = _slatConverter.LatStrTodegree(resultLat.ToString());
                        //resultLon = _slonConverter.LonStrTodegree(resultLon.ToString());

                        Longitude = resultLon as string;
                        Latitude = resultLat as string;
                    }
                    if (string.Equals(alertinfo.DisPlayInfo.GpsValid, "N"))
                    {
                        GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSNone");
                        Height = "-";
                        Speed = "-";
                        Direction = "-";
                    }
                }
                else
                {
                    Height = "-";
                    Speed = "-";
                    Direction = "-";
                }
                AlertType = alertinfo.DisPlayInfo.AlertType.Value;
                SuiteID = alertinfo.DisPlayInfo.MdvrCoreId;
                VehicleId = alertinfo.DisPlayInfo.VehicleId;
                VehicleOwner = alertinfo.DisPlayInfo.VehicleOwner;
                OwnerPhone = alertinfo.DisPlayInfo.OwnerPhone;
                Province = alertinfo.DisPlayInfo.Province;
                //City = alertinfo.DisPlayInfo.City;
                //Longitude = alertinfo.DisPlayInfo.Longitude;
                // Latitude = alertinfo.DisPlayInfo.Latitude;


                AlertTime = alertinfo.DisPlayInfo.AlertTime.HasValue ? alertinfo.DisPlayInfo.AlertTime.Value.ToLocalTime().ToString() : "";
                this.DistrictCodeDescribe = "";
                this.Locate = Latitude + " " + Longitude;
                this.OrganizationName = alertinfo.DisPlayInfo.OrganizationName;

                //if (!string.IsNullOrEmpty(alertinfo.DisPlayInfo.Note))
                //{
                //    this.IsNoteVisibility = Visibility.Visible;
                //}
                //else
                //{
                    this.IsNoteVisibility = Visibility.Collapsed;
                //}
                    this.DistrictCodeDescribe = alertinfo.DisPlayInfo.Province;

              

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertType));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel", ex);
            }
        }

        public void HandleEvent(int publishedEvent)
        {
            try
            {
                if (publishedEvent == 3)
                {
                    HandleresultVisible = Visibility.Visible;
                    AlertDetailVisible = Visibility.Visible;
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALERT_HandledAlert") + ")";
                    PicUrl = "/ExternalResource;component/Images/MainPage_menu_info.png";
                }
                
                if (publishedEvent == 2)
                {
                    SelectItemIndex = 0;
                    HandleresultVisible = Visibility.Collapsed;
                    AlertDetailVisible = Visibility.Visible;
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALERT_UnHandleAlert") + ")";
                    PicUrl = "/ExternalResource;component/Images/MainPage_menu_info_orange.png";
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HandleresultVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertDetailVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PicUrl));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel", ex);
            }
        }
        #endregion
    }
}

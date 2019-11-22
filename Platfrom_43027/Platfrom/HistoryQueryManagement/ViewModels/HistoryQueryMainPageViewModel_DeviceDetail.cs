using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DeviceAlertService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
        DeviceAlertServiceClient deviceAlertClient = null;

        #region 属性


        private string _devicealertvehicleid;
        public string DeviceAlertVehicleId
        {
            get
            {
                return _devicealertvehicleid;
            }
            set
            {
                _devicealertvehicleid = value;
                RaisePropertyChanged("DeviceAlertVehicleId");
            }
        }

        private string _devicealertvehicleowner;
        public string DeviceAlertVehicleOwner
        {
            get
            {
                return _devicealertvehicleowner;
            }
            set
            {
                _devicealertvehicleowner = value;
                RaisePropertyChanged("DeviceAlertVehicleOwner");
            }
        }

        private string _devicealertownerphone;
        public string DeviceAlertOwnerPhone
        {
            get
            {
                return _devicealertownerphone;
            }
            set
            {
                _devicealertownerphone = value;
                RaisePropertyChanged("DeviceAlertOwnerPhone");
            }
        }

        private string _devicealertprovince;
        public string DeviceAlertProvince
        {
            get
            {
                return _devicealertprovince;
            }
            set
            {
                _devicealertprovince = value;
                RaisePropertyChanged("DeviceAlertProvince");
            }
        }

        private string _devicealertcity;
        public string DeviceAlertCity
        {
            get
            {
                return _devicealertcity;
            }
            set
            {
                _devicealertcity = value;
                RaisePropertyChanged("DeviceAlertCity");
            }
        }

        private int _devicealerttype;
        public int DeviceAlertType
        {
            get
            {
                return _devicealerttype;
            }
            set
            {
                _devicealerttype = value;
                RaisePropertyChanged("DeviceAlertType");
            }
        }

        private string _devicealertgpsvalid;
        public string DeviceAlertGpsValid
        {
            get
            {
                return _devicealertgpsvalid;
            }
            set
            {
                _devicealertgpsvalid = value;
                RaisePropertyChanged("DeviceAlertGpsValid");
            }
        }

        private string _devicealertlongitude;
        public string DeviceAlertLongitude
        {
            get
            {
                return _devicealertlongitude;
            }
            set
            {
                _devicealertlongitude = value;
                RaisePropertyChanged("DeviceAlertLongitude");
            }
        }

        private string _devicealertlatitude;
        public string DeviceAlertLatitude
        {
            get
            {
                return _devicealertlatitude;
            }
            set
            {
                _devicealertlatitude = value;
                RaisePropertyChanged("DeviceAlertLatitude");
            }
        }

        private string _devicealertheight;
        public string DeviceAlertHeight
        {
            get
            {
                return _devicealertheight;
            }
            set
            {
                _devicealertheight = value;
                RaisePropertyChanged("DeviceAlertHeight");
            }
        }

        private string _devicealertspeed;
        public string DeviceAlertSpeed
        {
            get
            {
                return _devicealertspeed;
            }
            set
            {
                _devicealertspeed = value;
                RaisePropertyChanged("DeviceAlertSpeed");
            }
        }

        private string _devicealertdirection;
        public string DeviceAlertDirection
        {
            get
            {
                return _devicealertdirection;
            }
            set
            {
                _devicealertdirection = value;
                RaisePropertyChanged("DeviceAlertDirection");
            }
        }

        private string _devicealerttime;
        public string DeviceAlertTime
        {
            get
            {
                return _devicealerttime;
            }
            set
            {
                _devicealerttime = value;
                RaisePropertyChanged("DeviceAlertTime");
            }
        }

        private string _devicealertsuiteid;
        public string DeviceAlertSuiteID
        {
            get
            {
                return _devicealertsuiteid;
            }
            set
            {
                _devicealertsuiteid = value;
                RaisePropertyChanged("DeviceAlertSuiteID");
            }
        }

        private string _devicealertprovincename;
        public string DeviceAlertProvinceName
        {
            get
            {
                return _devicealertprovincename;
            }
            set
            {
                _devicealertprovincename = value;
                RaisePropertyChanged("DeviceAlertProvinceName");
            }
        }
        private string _devicealertcityname;
        public string DeviceAlertCityName
        {
            get
            {
                return _devicealertcityname;
            }
            set
            {
                _devicealertcityname = value;
                RaisePropertyChanged("DeviceAlertCityName");
            }
        }



        private string _devicedistrictCodeDescribe;
        /// <summary>
        /// 行政区划的文字描述
        /// </summary>
        public string DeviceDistrictCodeDescribe
        {
            get
            {
                return this._devicedistrictCodeDescribe;
            }
            set
            {
                this._devicedistrictCodeDescribe = value;
                RaisePropertyChanged(() => DeviceDistrictCodeDescribe);
            }
        }

        private string _devicelocate;
        /// <summary>
        /// 位置
        /// </summary>
        public string DeviceLocate
        {
            get
            {
                return this._devicelocate;
            }
            set
            {
                this._devicelocate = value;
                RaisePropertyChanged(() => DeviceLocate);
            }
        }

        private string _deviceorganizationName;
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string DeviceOrganizationName
        {
            get
            {
                return this._deviceorganizationName;
            }
            set
            {
                this._deviceorganizationName = value;
                RaisePropertyChanged(() => DeviceOrganizationName);
            }
        }

        //private Visibility _isAlertNoteVisibility;
        //public Visibility IsAlertNoteVisibility
        //{
        //    get
        //    {
        //        return this._isAlertNoteVisibility;
        //    }
        //    set
        //    {
        //        this._isAlertNoteVisibility = value;
        //        RaisePropertyChanged(() => IsAlertNoteVisibility);
        //    }
        //}

        /// <summary>
        /// 驾驶员服务客户端
        /// </summary>
        //private ChauffeurServiceClient client;
        private void InitialDeviceAlertDetail()
        {
            deviceAlertClient = InitialDeviceAlertClient();

        }

        //private Visibility _isNoteVisibiltiy;
        ///// <summary>
        ///// 处置信息是否显示
        ///// </summary>
        //public Visibility IsNoteVisibility
        //{
        //    get
        //    {
        //        return this._isNoteVisibiltiy;
        //    }
        //    set
        //    {
        //        this._isNoteVisibiltiy = value;
        //        RaisePropertyChanged(() => IsNoteVisibility);
        //    }
        //}

        //private string _alertHandlePerson;
        ///// <summary>
        ///// 车辆告警处置人
        ///// </summary>
        //public string AlertHandlePerson
        //{
        //    get
        //    {
        //        return this._alertHandlePerson;
        //    }
        //    set
        //    {
        //        this._alertHandlePerson = value;
        //        RaisePropertyChanged(() => AlertHandlePerson);
        //    }
        //}

        //private string _alertAlertTime;
        ///// <summary>
        ///// 车辆告警处置时间
        ///// </summary>
        //public string AlertAlertTime
        //{
        //    get
        //    {
        //        return this._alertAlertTime;
        //    }
        //    set
        //    {
        //        this._alertAlertTime = value;
        //        RaisePropertyChanged(() => AlertAlertTime);
        //    }
        //}

        //private string _note;
        ///// <summary>
        ///// 车辆告警处置内容
        ///// </summary>
        //public string Note
        //{
        //    get
        //    {
        //        return this._note;
        //    }
        //    set
        //    {
        //        this._note = value;
        //        RaisePropertyChanged(() => this.Note);
        //    }
        //}

        #endregion

        #region 方法



        public void OnDeviceAlertSelectChange(DeviceAlertEx alertinfo)
        {
            try
            {
                if(alertinfo != null)
                {
                    if(alertinfo.GpsValid != null)
                    {
                        if(alertinfo.GpsValid.Equals("V"))
                        {
                            DeviceAlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSERROR");
                        }
                        if(alertinfo.GpsValid.Equals("A"))
                        {
                            DeviceAlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSAvailable");
                            DeviceAlertLongitude = alertinfo.Longitude;
                            DeviceAlertLatitude = alertinfo.Latitude;

                            var tempLat = _latConverter.ConvertBack(DeviceAlertLatitude, null, null, null);
                            var resultLat = _latConverter.ConvertToWESN(tempLat, null, null, null);

                            var tempLon = _lonConverter.ConvertBack(DeviceAlertLongitude, null, null, null);
                            var resultLon = _lonConverter.ConvertToWESN(tempLon, null, null, null);


                            //resultLat = _slatConverter.LatStrTodegree(resultLat.ToString());
                            //resultLon = _slonConverter.LonStrTodegree(resultLon.ToString());

                            DeviceAlertLongitude = resultLon as string;
                            DeviceAlertLatitude = resultLat as string;
                        }
                        if(alertinfo.GpsValid.Equals("N"))
                        {
                            DeviceAlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSNone");
                        }
                    }
                    else
                    {
                        DeviceAlertLongitude = "-";
                        DeviceAlertLatitude = "-";
                    }
                    DeviceAlertType = alertinfo.AlertType.Value;
                    DeviceAlertSuiteID = alertinfo.MdvrCoreId;
                    DeviceAlertVehicleId = alertinfo.VehicleId;
                    DeviceAlertVehicleOwner = alertinfo.VehicleOwner;
                    DeviceAlertOwnerPhone = alertinfo.OwnerPhone;
                    DeviceAlertProvince = alertinfo.Province;
                    //DeviceAlertCity = alertinfo.City;
                    //AlertLongitude = alertinfo.Longitude;
                    //AlertLatitude = alertinfo.Latitude;
                    //DeviceAlertHeight = alertinfo.;
                    DeviceAlertSpeed = alertinfo.Speed;
                    DeviceAlertDirection = alertinfo.Direction;
                    DeviceAlertTime = alertinfo.AlertTime.HasValue == true ? alertinfo.AlertTime.Value.ToString() : "";
                    this.DeviceLocate = "";
                    this.DeviceLocate = DeviceAlertLatitude + " " + DeviceAlertLongitude;
                   // this.DistrictCodeDescribe = "";
                    this.DeviceOrganizationName = alertinfo.VehicleInfo.OrganizationName;

                    if (string.IsNullOrEmpty(DeviceAlertSpeed) || DeviceAlertSpeed == "0.0")
                    {
                        DeviceAlertSpeed = "-.-";
                    }
                    if (string.IsNullOrEmpty(DeviceAlertDirection) || DeviceAlertDirection == "0")
                    {
                        DeviceAlertDirection = "-";
                    }


                    this.DeviceDistrictCodeDescribe = this.DeviceAlertProvince;


                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DeviceAlertType));
                }
                else
                {
                    DeviceAlertGpsValid = string.Empty;
                    DeviceAlertType = -1;
                    DeviceAlertSuiteID = string.Empty;
                    DeviceAlertVehicleId = string.Empty;
                    DeviceAlertVehicleOwner = string.Empty;
                    DeviceAlertOwnerPhone = string.Empty;
                    DeviceAlertProvince = string.Empty;
                    DeviceAlertCity = string.Empty;
                    DeviceAlertLongitude = string.Empty;
                    DeviceAlertLatitude = string.Empty;
                    DeviceAlertHeight = string.Empty;
                    DeviceAlertSpeed = string.Empty;
                    DeviceAlertDirection = string.Empty;
                    DeviceAlertTime = string.Empty;
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnDeviceAlertSelectChange", ex);
            }
        }

        #endregion
    }
}

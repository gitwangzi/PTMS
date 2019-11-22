using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.ServiceReference.VehicleAlertService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
namespace HistoryQueryManagement.ViewModels
{
    public partial class HistoryQueryMainPageViewModel
    {
        VehicleAlertServiceClient vehicleAlertClient = null;

        #region 属性


        private string _alertvehicleid;
        public string AlertVehicleId
        {
            get
            {
                return _alertvehicleid;
            }
            set
            {
                _alertvehicleid = value;
                RaisePropertyChanged("AlertVehicleId");
            }
        }

        private string _alertvehicleowner;
        public string AlertVehicleOwner
        {
            get
            {
                return _alertvehicleowner;
            }
            set
            {
                _alertvehicleowner = value;
                RaisePropertyChanged("AlertVehicleOwner");
            }
        }

        private string _alertownerphone;
        public string AlertOwnerPhone
        {
            get
            {
                return _alertownerphone;
            }
            set
            {
                _alertownerphone = value;
                RaisePropertyChanged("AlertOwnerPhone");
            }
        }

        private string _alertprovince;
        public string AlertProvince
        {
            get
            {
                return _alertprovince;
            }
            set
            {
                _alertprovince = value;
                RaisePropertyChanged("AlertProvince");
            }
        }

        private string _alertcity;
        public string AlertCity
        {
            get
            {
                return _alertcity;
            }
            set
            {
                _alertcity = value;
                RaisePropertyChanged("AlertCity");
            }
        }

        private int _alerttype;
        public int AlertType
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

        private string _alertgpsvalid;
        public string AlertGpsValid
        {
            get
            {
                return _alertgpsvalid;
            }
            set
            {
                _alertgpsvalid = value;
                RaisePropertyChanged("AlertGpsValid");
            }
        }

        private string _alertlongitude;
        public string AlertLongitude
        {
            get
            {
                return _alertlongitude;
            }
            set
            {
                _alertlongitude = value;
                RaisePropertyChanged("AlertLongitude");
            }
        }

        private string _alertlatitude;
        public string AlertLatitude
        {
            get
            {
                return _alertlatitude;
            }
            set
            {
                _alertlatitude = value;
                RaisePropertyChanged("AlertLatitude");
            }
        }

        private string _alertheight;
        public string AlertHeight
        {
            get
            {
                return _alertheight;
            }
            set
            {
                _alertheight = value;
                RaisePropertyChanged("AlertHeight");
            }
        }

        private string _alertspeed;
        public string AlertSpeed
        {
            get
            {
                return _alertspeed;
            }
            set
            {
                _alertspeed = value;
                RaisePropertyChanged("AlertSpeed");
            }
        }

        private string _alertdirection;
        public string AlertDirection
        {
            get
            {
                return _alertdirection;
            }
            set
            {
                _alertdirection = value;
                RaisePropertyChanged("AlertDirection");
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

        private string _alertsuiteid;
        public string AlertSuiteID
        {
            get
            {
                return _alertsuiteid;
            }
            set
            {
                _alertsuiteid = value;
                RaisePropertyChanged("AlertSuiteID");
            }
        }

        private string _alertprovincename;
        public string AlertProvinceName
        {
            get
            {
                return _alertprovincename;
            }
            set
            {
                _alertprovincename = value;
                RaisePropertyChanged("AlertProvinceName");
            }
        }
        private string _alertcityname;
        public string AlertCityName
        {
            get
            {
                return _alertcityname;
            }
            set
            {
                _alertcityname = value;
                RaisePropertyChanged("AlertCityName");
            }
        }

        private string _alertdisposestaff;
        public string AlertDisposeStaff
        {
            get
            {
                return _alertdisposestaff;
            }
            set
            {
                _alertdisposestaff = value;
                RaisePropertyChanged("AlertDisposeStaff");
            }
        }
        private string _alertdisposetime;
        public string AlertDisposeTime
        {
            get
            {
                return _alertdisposetime;
            }
            set
            {
                _alertdisposetime = value;
                RaisePropertyChanged("AlertDisposeTime");
            }
        }
        private string _alertcontent;
        public string AlertContent
        {
            get
            {
                return _alertcontent;
            }
            set
            {
                _alertcontent = value;
                RaisePropertyChanged("AlertContent");
            }
        }

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

        private Visibility _isAlertNoteVisibility;
        public Visibility IsAlertNoteVisibility
        {
            get
            {
                return this._isAlertNoteVisibility;
            }
            set
            {
                this._isAlertNoteVisibility = value;
                RaisePropertyChanged(() => IsAlertNoteVisibility);
            }
        }

        /// <summary>
        /// 驾驶员服务客户端
        /// </summary>
        private ChauffeurServiceClient client;
        private void InitialAlertDetail()
        {
            vehicleAlertClient = InitialAlertClient();

        }

        private Visibility _isNoteVisibiltiy;
        /// <summary>
        /// 处置信息是否显示
        /// </summary>
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

        #endregion

        #region 方法

        private void InilitClient()
        {
            client = ServiceClientFactory.Create<ChauffeurServiceClient>();
            client.GetChauffeurByVehicleCompleted += client_GetChauffeurByVehicleCompleted;
        }

        void client_GetChauffeurByVehicleCompleted(object sender, GetChauffeurByVehicleCompletedEventArgs e)
        {
            try
            {
                if(e.Cancelled)
                {
                    return;
                }
                if(e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                if(e.Result.IsSuccess == false)
                {
                    if(!string.IsNullOrEmpty(e.Result.ErrorMsg))
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
                    if(e.Result.Result.Count > 0)
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
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel.client_GetChauffeurVehicleCompleted", ex);
            }
            finally
            {
                if(this.client != null)
                {
                    this.client.CloseAsync();
                }
                this.client = null;
            }
        }

        void vehicleAlertClient_GetBusinessAlertHandleByAlertIDCompleted(object sender, GetBusinessAlertHandleByAlertIDCompletedEventArgs e)
        {
            try
            {
                if(e.Error == null && e.Result.Result != null)
                {
                    AlertDisposeStaff = e.Result.Result.HandleUser;
                    AlertDisposeTime = e.Result.Result.HandleTime.ToLocalTime().ToString();
                    AlertContent = e.Result.Result.Content;
                    if(!string.IsNullOrEmpty(AlertContent))
                    {
                        this.IsAlertNoteVisibility = Visibility.Visible;
                    }
                    else
                    {
                        this.IsAlertNoteVisibility = Visibility.Collapsed;
                    }
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("vehicleAlarmServiceClient_GetTransferDisposeByAlarmIDCompleted", ex);

                CloseAlertClient(vehicleAlertClient);

                vehicleAlertClient = InitialAlertClient();
            }
        }


        public void OnAlertSelectChange(BusinessAlertEx alertinfo)
        {
            try
            {
                if(alertinfo != null)
                {
                    if(alertinfo.GpsValid != null)
                    {
                        if(alertinfo.GpsValid.Equals("V"))
                        {
                            AlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSERROR");
                        }
                        if(alertinfo.GpsValid.Equals("A"))
                        {
                            AlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSAvailable");
                            AlertLongitude = alertinfo.Longitude;
                            AlertLatitude = alertinfo.Latitude;

                            var tempLat = _latConverter.ConvertBack(AlertLatitude, null, null, null);
                            var resultLat = _latConverter.ConvertToWESN(tempLat, null, null, null);

                            var tempLon = _lonConverter.ConvertBack(AlertLongitude, null, null, null);
                            var resultLon = _lonConverter.ConvertToWESN(tempLon, null, null, null);

                            AlertLongitude = resultLon as string;
                            AlertLatitude = resultLat as string;
                        }
                        if(alertinfo.GpsValid.Equals("N"))
                        {
                            AlertGpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSNone");
                        }
                    }
                    else
                    {
                        AlertLongitude = "-";
                        AlertLatitude = "-";
                    }
                    AlertType = alertinfo.AlertType.Value;
                    AlertSuiteID = alertinfo.SuiteID;
                    AlertVehicleId = alertinfo.VehicleId;
                    AlertVehicleOwner = alertinfo.VehicleOwner;
                    AlertOwnerPhone = alertinfo.OwnerPhone;
                    AlertProvince = alertinfo.Province;
                    AlertCity = alertinfo.City;
                    //AlertLongitude = alertinfo.Longitude;
                    //AlertLatitude = alertinfo.Latitude;
                    AlertHeight = alertinfo.Height;
                    AlertSpeed = alertinfo.Speed;
                    AlertDirection = alertinfo.Direction;
                    AlertTime = alertinfo.AlertTime.HasValue == true ? alertinfo.AlertTime.Value.ToString() : "";
                    this.Locate = "";
                    this.Locate = AlertLatitude + " " + AlertLongitude;
                    this.DistrictCodeDescribe = "";
                    this.OrganizationName = alertinfo.OrganizationName;
                    if(this.client == null)
                    {
                        this.InilitClient();
                    }
                    this.client.GetChauffeurByVehicleAsync(alertinfo.VehicleId, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    if(string.IsNullOrEmpty(AlertSpeed) || AlertSpeed == "0.0")
                    {
                        AlertSpeed = "-.-";
                    }
                    if(string.IsNullOrEmpty(AlertDirection) || AlertDirection == "0")
                    {
                        AlertDirection = "-";
                    }
                    //string provinceName = "";
                    //string cityName = "";
                    //if (!string.IsNullOrEmpty(AlertProvince.Trim()))
                    //{
                    //    var province = ApplicationContext.Instance.BufferManager.DistrictManager.Provinces.FirstOrDefault(n => n.Code == AlertProvince);

                    //    if (province != null)
                    //    {
                    //        provinceName = province.Name;

                    //    }
                    //}
                    //else
                    //{
                    //    provinceName = "";
                    //}

                    //if (!string.IsNullOrEmpty(AlertCity.Trim()))
                    //{
                    //    var city = ApplicationContext.Instance.BufferManager.DistrictManager.Cities.FirstOrDefault(c => c.Code == AlertCity);
                    //    if (city != null)
                    //    {
                    //        cityName = city.Name;
                    //    }
                    //}
                    //else
                    //{
                    //    cityName = "";
                    //}

                    this.DistrictCodeDescribe = this.AlertProvince + "/" + this.AlertCity;

                    if(alertinfo.Status != 1)
                    {
                        vehicleAlertClient.GetBusinessAlertHandleByAlertIDAsync(alertinfo.Id);
                    }
                    else
                    {
                        AlertDisposeStaff = string.Empty;
                        AlertDisposeTime = string.Empty;
                        AlertContent = string.Empty;
                    }

                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertType));
                }
                else
                {
                    AlertGpsValid = string.Empty;
                    AlertType = -1;
                    AlertSuiteID = string.Empty;
                    AlertVehicleId = string.Empty;
                    AlertVehicleOwner = string.Empty;
                    AlertOwnerPhone = string.Empty;
                    AlertProvince = string.Empty;
                    AlertCity = string.Empty;
                    AlertLongitude = string.Empty;
                    AlertLatitude = string.Empty;
                    AlertHeight = string.Empty;
                    AlertSpeed = string.Empty;
                    AlertDirection = string.Empty;
                    AlertTime = string.Empty;
                }
            }
            catch(Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OnAlertSelectChange", ex);
            }
        }

        #endregion
    }
}

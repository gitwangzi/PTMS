using Gsafety.PTMS.ServiceReference.VehicleService;
using System;
using System.ComponentModel;

namespace Gsafety.PTMS.Bases.Models
{
    [System.Runtime.Serialization.DataContract]
    public class Vehicle : INotifyPropertyChanged
    {
        #region Fields
        public delegate void OnOffLineChangeEvent();
        public event OnOffLineChangeEvent onOffLineChangeEvent;

        private bool? _IsMonitor = false;
        private string _UniqueId;
        private string _CityCode;
        private string _GroupId;
        private bool _IsOnLine;
        private string _VehicleSn;
        private string _ProvinceName;
        private string _CityName;
        private string _EngineId;
        private string _BrandModel;
        private string _Owner;
        private string _StartYear;
        private bool m_ElectricFence;

        private string _organizationName;
        private string _contactPhone;
        private string _note;
        private string _ficha;
        private DateTime? _productDate;

        #endregion

        #region Attributes

        public string CityName
        {
            get { return _CityName; }
            set { _CityName = value; }
        }

        /// <summary>
        /// 行政区划编号
        /// </summary>
        public string DistrictCode { get; set; }

        public string ProvinceName
        {
            get { return _ProvinceName; }
            set { _ProvinceName = value; }
        }

        public string VehicleSn
        {
            get { return _VehicleSn; }
            set { _VehicleSn = value; }
        }

        public string Ficha
        {
            get { return _ficha; }
            set { _ficha = value; }
        }

        public DateTime? ProductDate
        {
            get { return _productDate; }
            set { _productDate = value; }
        }

        public string EngineId
        {
            get { return _EngineId; }
            set { _EngineId = value; }
        }

        public string BrandModel
        {
            get { return _BrandModel; }
            set { _BrandModel = value; }
        }

        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        public string StartYear
        {
            get { return _StartYear; }
            set { _StartYear = value; }
        }

        public string Name
        {
            get
            {
                return VehicleId;
            }
        }

        public string VehicleId { get; set; }

        /// <summary>
        /// 车辆类型文字描述
        /// </summary>
        public string VehicleTypeDescribe { get; set; }

        public string VehicleTypeImage { get; set; }
        public string ID { get; set; }

        public string UniqueId
        {
            get
            {
                return _UniqueId;
            }
            set
            {
                _UniqueId = value;
                //RaisePropertyChanged(() => MDVRID);
            }
        }

        /// <summary>
        /// Whether Display ElectricFence
        /// </summary>
        public bool ElectricFence
        {
            get { return m_ElectricFence; }
            set
            {
                m_ElectricFence = value;
                //RaisePropertyChanged(() => ElectricFence);
            }
        }

        public bool IsOnLine
        {
            get { return _IsOnLine; }
            private set
            {
                if (value != _IsOnLine)
                {
                    _IsOnLine = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChangedEventArgs args = new PropertyChangedEventArgs("IsOnLine");
                        PropertyChanged(this, args);
                    }
                    if (null != onOffLineChangeEvent)
                    {
                        onOffLineChangeEvent();
                    }
                }
            }
        }

        private void SetIsOnLine()
        {
            IsOnLine = false;
            if (MDVROnline.HasValue && MDVROnline.Value)
            {
                IsOnLine = true;
                return;
            }
            if (GPSOnline.HasValue && GPSOnline.Value)
            {
                IsOnLine = true;
                return;
            }
            if (MobileOnline.HasValue && MobileOnline.Value)
            {
                IsOnLine = true;
                return;
            }
        }

        public bool HasMdvr
        {
            get { return MDVROnline != null; }
        }

        public bool HasMoblie
        {
            get { return MobileOnline != null; }
        }

        private bool? _mdvrOnline;
        /// <summary>
        /// Null 未安装MDVR
        /// True 在线
        /// False不在线
        /// </summary>
        public bool? MDVROnline
        {
            get { return _mdvrOnline; }
            set
            {
                _mdvrOnline = value;
                SetIsOnLine();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MDVROnline"));
                    PropertyChanged(this, new PropertyChangedEventArgs("HasMdvr"));
                }
            }
        }

        private bool? _gpsOnline;
        public bool? GPSOnline
        {
            get { return _gpsOnline; }
            set
            {
                _gpsOnline = value;
                SetIsOnLine();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("GPSOnline"));
                }
            }
        }

        private bool? _mobileOnline;
        public bool? MobileOnline
        {
            get { return _mobileOnline; }
            set
            {
                _mobileOnline = value;
                SetIsOnLine();
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MobileOnline"));
                    PropertyChanged(this, new PropertyChangedEventArgs("HasMoblie"));
                }
            }
        }

        public string CityCode
        {
            get
            {
                return _CityCode;
            }
            set
            {
                _CityCode = value;
            }
        }

        public string GroupID
        {
            get
            {
                return _GroupId;
            }
            set
            {
                _GroupId = value;
            }
        }

        public bool? IsMonitor
        {
            get
            {
                return _IsMonitor;
            }
            set
            {
                if (value != _IsMonitor)
                {
                    _IsMonitor = value;
                }
            }
        }

        private string _organizationID;

        public string OrganizationID
        {
            get { return _organizationID; }
            set { _organizationID = value; }
        }

        /// <summary>
        /// 所属组织机构名称
        /// </summary>
        public string OrganizationName
        {

            get { return this._organizationName; }
            set { _organizationName = value; }
        }

        public string ContactPhone
        {
            get { return this._contactPhone; }
            set { _contactPhone = value; }
        }

        public string Note
        {
            get { return this._note; }
            set { this._note = value; }
        }

        public VehicleSeviceType VehicleServiceType { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

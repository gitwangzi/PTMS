using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using Gsafety.PTMS.Common.Enum;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///GPS设备
    ///</summary>
    [DataContract]
    public class DevGps
    {
        string _id;
        ///<summary>
        ///主键
        ///</summary>
        [DataMember]
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        string _clientid;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientId
        {
            get
            {
                return _clientid;
            }
            set
            {
                _clientid = value;
            }
        }

        string _gpssn;
        ///<summary>
        ///设备编号,对外的标签
        ///</summary>
        [DataMember]
        public string GpsSn
        {
            get
            {
                return _gpssn;
            }
            set
            {
                _gpssn = value;
            }
        }

        decimal _status;
        ///<summary>
        ///安全套件状态
        ///</summary>
        [DataMember]
        public decimal Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        string _districtcode;
        ///<summary>
        ///行政区域
        ///</summary>
        [DataMember]
        public string DistrictCode
        {
            get
            {
                return _districtcode;
            }
            set
            {
                _districtcode = value;
            }
        }

        string _gpssim;
        ///<summary>
        ///SIM卡号
        ///</summary>
        [DataMember]
        public string GpsSim
        {
            get
            {
                return _gpssim;
            }
            set
            {
                _gpssim = value;
            }
        }

        string _creator;
        ///<summary>
        ///创建人
        ///</summary>
        [DataMember]
        public string Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///创建时间
        ///</summary>
        [DataMember]
        public DateTime CreateTime
        {
            get
            {
                return _createtime;
            }
            set
            {
                _createtime = value;
            }
        }

        decimal _valid;
        ///<summary>
        ///是否有效
        ///</summary>
        [DataMember]
        public decimal Valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }

        string _gpsuid;
        ///<summary>
        ///数据接入唯一码
        ///</summary>
        [DataMember]
        public string GpsUid
        {
            get
            {
                return _gpsuid;
            }
            set
            {
                _gpsuid = value;
            }
        }

        /// <summary>
        /// 安装状态
        /// </summary>
        [DataMember]
        public InstallStatusType InstallStatus = InstallStatusType.UnInstall;

        string displayInstallStatus;
        [DataMember]
        public string DisplayInstallStatus
        {
            get { return displayInstallStatus; }
            set { displayInstallStatus = value; }
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientId)))
            {
                builder.AppendLine("ClientId:" + ClientId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsSn)))
            {
                builder.AppendLine("GpsSn:" + GpsSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DistrictCode)))
            {
                builder.AppendLine("DistrictCode:" + DistrictCode.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsSim)))
            {
                builder.AppendLine("GpsSim:" + GpsSim.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }

            return builder.ToString();
        }

    }
}


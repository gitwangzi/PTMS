using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///GPS工作
    ///</summary>
    [DataContract]
    public class GPSWorking
    {
        string _gpsid;
        ///<summary>
        ///唯一标识
        ///</summary>
        [DataMember]
        public string GPSID
        {
            get
            {
                return _gpsid;
            }
            set
            {
                _gpsid = value;
            }
        }

        string _clientid;
        ///<summary>
        ///客户账号
        ///</summary>
        [DataMember]
        public string ClientID
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

        decimal _onlineflag;
        ///<summary>
        ///是否在线
        ///</summary>
        [DataMember]
        public decimal OnlineFlag
        {
            get
            {
                return _onlineflag;
            }
            set
            {
                _onlineflag = value;
            }
        }

        DateTime _switchtime;
        ///<summary>
        ///上下线切换时间
        ///</summary>
        [DataMember]
        public DateTime SwitchTime
        {
            get
            {
                return _switchtime;
            }
            set
            {
                _switchtime = value;
            }
        }

        decimal _status;
        ///<summary>
        ///状态
        //0 正常
        //1 异常
        //3 等待维修
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

        DateTime _faulttime;
        ///<summary>
        ///异常时间
        ///</summary>
        [DataMember]
        public DateTime FaultTime
        {
            get
            {
                return _faulttime;
            }
            set
            {
                _faulttime = value;
            }
        }

        string _abnormalcause;
        ///<summary>
        ///设备异常原因，参考设备异常类型
        ///</summary>
        [DataMember]
        public string AbnormalCause
        {
            get
            {
                return _abnormalcause;
            }
            set
            {
                _abnormalcause = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///车牌号
        ///</summary>
        [DataMember]
        public string VehicleID
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                _vehicleid = value;
            }
        }

        string _organizationid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string OrganizationID
        {
            get
            {
                return _organizationid;
            }
            set
            {
                _organizationid = value;
            }
        }

        string _gpssn;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string GPSSN
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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(GPSID)))
            {
                builder.AppendLine("GpsID:" + GPSID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
            {
                builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SwitchTime)))
            {
                builder.AppendLine("SwitchTime:" + SwitchTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FaultTime)))
            {
                builder.AppendLine("FaultTime:" + FaultTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalCause)))
            {
                builder.AppendLine("AbnormalCause:" + AbnormalCause.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrganizationID)))
            {
                builder.AppendLine("OrganizationID:" + OrganizationID.ToString());
            }

            return builder.ToString();
        }

    }
}


using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///套件工作
    ///</summary>
    [DataContract]
    public class SuiteWorking
    {
        string _suiteinfoid;
        ///<summary>
        ///唯一标识
        ///</summary>
        [DataMember]
        public string SuiteInfoID
        {
            get
            {
                return _suiteinfoid;
            }
            set
            {
                _suiteinfoid = value;
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

        string _mdvrcoresn;
        ///<summary>
        ///ＭＤＶＲ芯片号
        ///</summary>
        [DataMember]
        public string MdvrCoreSn
        {
            get
            {
                return _mdvrcoresn;
            }
            set
            {
                _mdvrcoresn = value;
            }
        }

        decimal _status;
        ///<summary>
        ///套件状态
        //0 正常
        //1 异常
        //2 等待维修
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


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteInfoID)))
            {
                builder.AppendLine("SuiteInfoID:" + SuiteInfoID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SwitchTime)))
            {
                builder.AppendLine("SwitchTime:" + SwitchTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
            {
                builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FaultTime)))
            {
                builder.AppendLine("FaultTime:" + FaultTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalCause)))
            {
                builder.AppendLine("AbnormalCause:" + AbnormalCause.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrganizationID)))
            {
                builder.AppendLine("OrganizationID:" + OrganizationID.ToString());
            }

            return builder.ToString();
        }

    }
}
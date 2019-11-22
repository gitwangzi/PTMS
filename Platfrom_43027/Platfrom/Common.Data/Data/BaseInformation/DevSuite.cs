using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///安全套件
    ///</summary>
    [DataContract]
    public class DevSuite
    {
        public DevSuite()
        {
            BscDevSuiteParts = new List<DevSuitePart>();
        }

        string _suiteinfoid;
        ///<summary>
        ///主键
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

        string _suiteid;
        ///<summary>
        ///套件号
        ///</summary>
        [DataMember]
        public string SuiteID
        {
            get
            {
                return _suiteid;
            }
            set
            {
                _suiteid = value;
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

        string _mdvrsn;
        ///<summary>
        ///CEIEC主机（MDVR上的条形码）
        ///</summary>
        [DataMember]
        public string MdvrSn
        {
            get
            {
                return _mdvrsn;
            }
            set
            {
                _mdvrsn = value;
            }
        }

        string _mdvrsim;
        ///<summary>
        ///MDVR SIM卡号
        ///</summary>
        [DataMember]
        public string MdvrSim
        {
            get
            {
                return _mdvrsim;
            }
            set
            {
                _mdvrsim = value;
            }
        }

        string _mdvrsimmobile;
        ///<summary>
        ///MDVR 电话卡号
        ///</summary>
        [DataMember]
        public string MdvrSimMobile
        {
            get
            {
                return _mdvrsimmobile;
            }
            set
            {
                _mdvrsimmobile = value;
            }
        }

        string _sdsn;
        ///<summary>
        ///SD卡
        ///</summary>
        [DataMember]
        public string SdSn
        {
            get
            {
                return _sdsn;
            }
            set
            {
                _sdsn = value;
            }
        }

        string _softwareversion;
        ///<summary>
        ///软件版本号
        ///</summary>
        [DataMember]
        public string SoftwareVersion
        {
            get
            {
                return _softwareversion;
            }
            set
            {
                _softwareversion = value;
            }
        }

        string _model;
        ///<summary>
        ///设备型号
        ///</summary>
        [DataMember]
        public string Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        ProtocolTypeEnum _protocol;
        ///<summary>
        ///协议类型
        ///</summary>
        [DataMember]
        public ProtocolTypeEnum Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                _protocol = value;
            }
        }

        int _status;
        ///<summary>
        ///安全套件状态
        ///</summary>
        [DataMember]
        public int Status
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

        string _note;
        ///<summary>
        ///备注
        ///</summary>
        [DataMember]
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }

        string _upssn;
        ///<summary>
        ///UPS
        ///</summary>
        [DataMember]
        public string UpsSn
        {
            get
            {
                return _upssn;
            }
            set
            {
                _upssn = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///创建日期
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

        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }

        [DataMember]
        public List<DevSuitePart> BscDevSuiteParts { get; set; }

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
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrSn)))
            {
                builder.AppendLine("MdvrSn:" + MdvrSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrSim)))
            {
                builder.AppendLine("MdvrSim:" + MdvrSim.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrSimMobile)))
            {
                builder.AppendLine("MdvrSimMobile:" + MdvrSimMobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SdSn)))
            {
                builder.AppendLine("SdSn:" + SdSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SoftwareVersion)))
            {
                builder.AppendLine("SoftwareVersion:" + SoftwareVersion.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Model)))
            {
                builder.AppendLine("Model:" + Model.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Protocol)))
            {
                builder.AppendLine("Protocol:" + Protocol.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UpsSn)))
            {
                builder.AppendLine("UpsSn:" + UpsSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }
    }
}


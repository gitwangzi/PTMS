using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///超速车辆关联表
    ///</summary>
    [DataContract]
    public class VehicleSpeed
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
        ///客户账户
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

        string _speedid;
        ///<summary>
        ///速度设置ID
        ///</summary>
        [DataMember]
        public string SpeedID
        {
            get
            {
                return _speedid;
            }
            set
            {
                _speedid = value;
            }
        }

        string _mdvrcoresn;
        ///<summary>
        ///芯片号
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
        ///状态
        //状态
        //0 等待下发
        //1 下发中
        //2 下发失败
        //3 下发成功
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

        decimal _packetseq;
        ///<summary>
        ///下发包流水号
        ///</summary>
        [DataMember]
        public decimal PacketSeq
        {
            get
            {
                return _packetseq;
            }
            set
            {
                _packetseq = value;
            }
        }

        string _creator;
        ///<summary>
        ///创建或更新人
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

        bool _vehicleBtnEnable=true;
         [DataMember]
        public bool VehicleBtnEnable
        {
            get
            {
                return _vehicleBtnEnable;
            }
            set
            {
                _vehicleBtnEnable = value;
            }
        }
        

        private string _organization = string.Empty;
        [DataMember]
        public string Organization
        {
            get { return _organization; }
            set { _organization = value; }
        }

        private string _vehicletype = string.Empty;
        [DataMember]
        public string VehicleType
        {
            get { return _vehicletype; }
            set { _vehicletype = value; }
        }

        DateTime? _createtime;
        ///<summary>
        ///创建或更新时间
        ///</summary>
        [DataMember]
        public DateTime? CreateTime
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

        DateTime? _sendtime;
        ///<summary>
        ///创建或更新时间
        ///</summary>
        [DataMember]
        public DateTime? SendTime
        {
            get
            {
                return _sendtime;
            }
            set
            {
                _sendtime = value;
            }
        }


        string _showState;
        [DataMember]
        public string ShowState
        {
            get
            {
                return _showState;
            }
            set
            {
                _showState = value;
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SpeedID)))
            {
                builder.AppendLine("SpeedID:" + SpeedID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PacketSeq)))
            {
                builder.AppendLine("PacketSeq:" + PacketSeq.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}


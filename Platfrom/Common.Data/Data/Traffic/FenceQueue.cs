using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///
    ///</summary>
    [DataContract]
    public class FenceQueue
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

        string _fenceid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string FenceID
        {
            get
            {
                return _fenceid;
            }
            set
            {
                _fenceid = value;
            }
        }

        string _clientid;
        ///<summary>
        ///
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

        string _name;
        ///<summary>
        ///名称
        ///</summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        int _fencetype;
        ///<summary>
        ///围栏类型
        ///1 监控点
        ///2 区域
        ///3 线
        ///4 其它
        ///</summary>
        [DataMember]
        public int FenceType
        {
            get
            {
                return _fencetype;
            }
            set
            {
                _fencetype = value;
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

        short _alerttype;
        ///<summary>
        ///告警类型
        ///1 入栏告警
        ///2 出栏告警
        ///3 出入栏告警
        ///</summary>
        [DataMember]
        public short AlertType
        {
            get
            {
                return _alerttype;
            }
            set
            {
                _alerttype = value;
            }
        }

        string _resultpacket;
        ///<summary>
        ///指令内容
        ///</summary>
        [DataMember]
        public string ResultPacket
        {
            get
            {
                return _resultpacket;
            }
            set
            {
                _resultpacket = value;
            }
        }

        string _pts;
        ///<summary>
        ///围栏坐标
        ///</summary>
        [DataMember]
        public string Pts
        {
            get
            {
                return _pts;
            }
            set
            {
                _pts = value;
            }
        }

        int _radius;
        ///<summary>
        ///半径
        ///</summary>
        [DataMember]
        public int Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
            }
        }

        string _circlecenter;
        ///<summary>
        ///监控点
        ///</summary>
        [DataMember]
        public string CircleCenter
        {
            get
            {
                return _circlecenter;
            }
            set
            {
                _circlecenter = value;
            }
        }

        DateTime _createtime;
        ///<summary>
        ///
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

        decimal _packetseq;
        ///<summary>
        ///数据包序号
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

        DateTime? _sendtime;
        ///<summary>
        ///指令发送时间
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

        int _status;
        ///<summary>
        ///状态
        //1等待
        //2成功
        //3失败 
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

        int _opertype;
        ///<summary>
        ///0 添加
        ///1 修改
        ///2 删除
        ///</summary>
        [DataMember]
        public int OperType
        {
            get
            {
                return _opertype;
            }
            set
            {
                _opertype = value;
            }
        }

        int _maxspeed;
        ///<summary>
        ///</summary>
        [DataMember]
        public int MaxSpeed
        {
            get
            {
                return _maxspeed;
            }
            set
            {
                _maxspeed = value;
            }
        }

        int _overspeedduration;
        ///<summary>
        ///</summary>
        [DataMember]
        public int OverSpeedDuration
        {
            get
            {
                return _overspeedduration;
            }
            set
            {
                _overspeedduration = value;
            }
        }

        int _pointcount;
        ///<summary>
        ///</summary>
        [DataMember]
        public int PointCount
        {
            get
            {
                return _pointcount;
            }
            set
            {
                _pointcount = value;
            }
        }

        string _starttime;
        ///<summary>
        ///</summary>
        [DataMember]
        public string StartTime
        {
            get
            {
                return _starttime;
            }
            set
            {
                _starttime = value;
            }
        }

        string _endtime;
        ///<summary>
        ///</summary>
        [DataMember]
        public string EndTime
        {
            get
            {
                return _endtime;
            }
            set
            {
                _endtime = value;
            }
        }

        string _regionproperty;
        ///<summary>
        ///</summary>
        [DataMember]
        public string RegionProperty
        {
            get
            {
                return _regionproperty;
            }
            set
            {
                _regionproperty = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceID)))
            {
                builder.AppendLine("FenceID:" + FenceID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceType)))
            {
                builder.AppendLine("FenceType:" + FenceType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlertType)))
            {
                builder.AppendLine("AlertType:" + AlertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ResultPacket)))
            {
                builder.AppendLine("ResultPacket:" + ResultPacket.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OverSpeedDuration)))
            {
                builder.AppendLine("OverSpeedDuration:" + OverSpeedDuration.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MaxSpeed)))
            {
                builder.AppendLine("MaxSpeed:" + MaxSpeed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Pts)))
            {
                builder.AppendLine("Pts:" + Pts.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Radius)))
            {
                builder.AppendLine("Radius:" + Radius.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CircleCenter)))
            {
                builder.AppendLine("CircleCenter:" + CircleCenter.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PacketSeq)))
            {
                builder.AppendLine("PacketSeq:" + PacketSeq.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperType)))
            {
                builder.AppendLine("OperType:" + OperType.ToString());
            }

            return builder.ToString();
        }

    }
}


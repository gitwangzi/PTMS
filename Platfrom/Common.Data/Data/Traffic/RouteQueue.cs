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
    public class RouteQueue
    {
        string _id;
        ///<summary>
        ///
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

        string _endtime;
        ///<summary>
        ///
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

        decimal _regionid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal RegionID
        {
            get
            {
                return _regionid;
            }
            set
            {
                _regionid = value;
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

        decimal _opertype;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal OperType
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

        decimal _status;
        ///<summary>
        ///状态
        //0 添加等待下发
        //1 添加下发中
        //2 添加下发失败 
        //3 修改等待下发
        //4 修改下发中
        //5 修改下发失败
        //6 删除等待下发
        //7 删除下发中
        //9 删除下发失败
        //10 下发成功
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

        string _starttime;
        ///<summary>
        ///
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

        string _pts;
        ///<summary>
        ///线路坐标
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

        decimal _width;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        string _routeproperty;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string RouteProperty
        {
            get
            {
                return _routeproperty;
            }
            set
            {
                _routeproperty = value;
            }
        }

        string _routesegmentproperty;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string RouteSegmentProperty
        {
            get
            {
                return _routesegmentproperty;
            }
            set
            {
                _routesegmentproperty = value;
            }
        }

        string _routeid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string RouteID
        {
            get
            {
                return _routeid;
            }
            set
            {
                _routeid = value;
            }
        }

        decimal _maxspeed;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal MaxSpeed
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

        string _name;
        ///<summary>
        ///
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

        decimal _overspeedduration;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal OverSpeedDuration
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

        decimal _pointcount;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal PointCount
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
            if (!string.IsNullOrEmpty(Convert.ToString(ResultPacket)))
            {
                builder.AppendLine("ResultPacket:" + ResultPacket.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PacketSeq)))
            {
                builder.AppendLine("PacketSeq:" + PacketSeq.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RegionID)))
            {
                builder.AppendLine("RegionID:" + RegionID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperType)))
            {
                builder.AppendLine("OperType:" + OperType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Pts)))
            {
                builder.AppendLine("Pts:" + Pts.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Width)))
            {
                builder.AppendLine("Width:" + Width.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteProperty)))
            {
                builder.AppendLine("RouteProperty:" + RouteProperty.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteSegmentProperty)))
            {
                builder.AppendLine("RouteSegmentProperty:" + RouteSegmentProperty.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteID)))
            {
                builder.AppendLine("RouteID:" + RouteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MaxSpeed)))
            {
                builder.AppendLine("MaxSpeed:" + MaxSpeed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OverSpeedDuration)))
            {
                builder.AppendLine("OverSpeedDuration:" + OverSpeedDuration.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PointCount)))
            {
                builder.AppendLine("PointCount:" + PointCount.ToString());
            }

            return builder.ToString();
        }

    }
}


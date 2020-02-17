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
    public class VehicleTrafficRoute
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

        string _uid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string UID
        {
            get
            {
                return _uid;
            }
            set
            {
                _uid = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string VehicleId
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

        int _turningcount;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int TurningCount
        {
            get
            {
                return _turningcount;
            }
            set
            {
                _turningcount = value;
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

        string _routesegmentproperty;
        ///<summary>
        ///主线路号
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

        int _routesegmentwidth;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int Width
        {
            get
            {
                return _routesegmentwidth;
            }
            set
            {
                _routesegmentwidth = value;
            }
        }

        int _routesegmentmaxspeed;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int MaxSpeed
        {
            get
            {
                return _routesegmentmaxspeed;
            }
            set
            {
                _routesegmentmaxspeed = value;
            }
        }

        string _routename;
        ///<summary>
        ///线路名称
        ///</summary>
        [DataMember]
        public string Name
        {
            get
            {
                return _routename;
            }
            set
            {
                _routename = value;
            }
        }

        int _routesegmentspeedduration;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public int OverSpeedDuration
        {
            get
            {
                return _routesegmentspeedduration;
            }
            set
            {
                _routesegmentspeedduration = value;
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

        bool _valid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public bool Valid
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

        string _creator;
        ///<summary>
        ///
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

        int _pointcount;
        ///<summary>
        ///
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


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UID)))
            {
                builder.AppendLine("ID:" + UID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TurningCount)))
            {
                builder.AppendLine("TurningCount:" + TurningCount.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteProperty)))
            {
                builder.AppendLine("RouteProperty:" + RouteProperty.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Pts)))
            {
                builder.AppendLine("Pts:" + Pts.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RouteSegmentProperty)))
            {
                builder.AppendLine("RouteSegmentProperty:" + RouteSegmentProperty.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Width)))
            {
                builder.AppendLine("RouteSegmentWidth:" + Width.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MaxSpeed)))
            {
                builder.AppendLine("RouteSegmentMaxSpeed:" + MaxSpeed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("RouteName:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OverSpeedDuration)))
            {
                builder.AppendLine("RouteSegmentSpeedDuration:" + OverSpeedDuration.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("Valid:" + Valid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }

            return builder.ToString();
        }

    }
}


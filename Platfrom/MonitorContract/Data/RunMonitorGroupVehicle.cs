using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace Gsafety.PTMS.Monitor.Contract.Data
{
    ///<summary>
    ///分组车辆
    ///</summary>
    [DataContract]
    public class RunMonitorGroupVehicle
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

        string _groupid;
        ///<summary>
        ///分组
        ///</summary>
        [DataMember]
        public string GroupId
        {
            get
            {
                return _groupid;
            }
            set
            {
                _groupid = value;
            }
        }

        string _vehicleid;
        ///<summary>
        ///车牌号
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

        decimal _tracedflag;
        ///<summary>
        ///追踪
        ///</summary>
        [DataMember]
        public decimal TracedFlag
        {
            get
            {
                return _tracedflag;
            }
            set
            {
                _tracedflag = value;
            }
        }

        decimal _vehicleindex;
        ///<summary>
        ///排序
        ///</summary>
        [DataMember]
        public decimal VehicleIndex
        {
            get
            {
                return _vehicleindex;
            }
            set
            {
                _vehicleindex = value;
            }
        }


        RunMonitorGroup monitorGroup;
        ///<summary>
        ///排序
        ///</summary>
        [DataMember]
        public RunMonitorGroup MonitorGroup
        {
            get
            {
                return monitorGroup;
            }
            set
            {
                monitorGroup = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GroupId)))
            {
                builder.AppendLine("GroupId:" + GroupId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TracedFlag)))
            {
                builder.AppendLine("TracedFlag:" + TracedFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleIndex)))
            {
                builder.AppendLine("VehicleIndex:" + VehicleIndex.ToString());
            }

            return builder.ToString();
        }

    }
}


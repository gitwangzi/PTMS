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
    public class MaintainRecord
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

        string _applicationid;
        ///<summary>
        ///预约ID
        ///</summary>
        [DataMember]
        public string ApplicationID
        {
            get
            {
                return _applicationid;
            }
            set
            {
                _applicationid = value;
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

        decimal _status;
        ///<summary>
        ///状态
        //0 未开始
        //1 维修中
        //2 已完成
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

        DateTime _starttime;
        ///<summary>
        ///开始时间
        ///</summary>
        [DataMember]
        public DateTime StartTime
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

        DateTime? _endtime;
        ///<summary>
        ///结束时间
        ///</summary>
        [DataMember]
        public DateTime? EndTime
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

        string _worker;
        ///<summary>
        ///修维人
        ///</summary>
        [DataMember]
        public string Worker
        {
            get
            {
                return _worker;
            }
            set
            {
                _worker = value;
            }
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

        private string _vehicleid;
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

        DateTime _scheduledate;
        ///<summary>
        ///预约时间
        ///</summary>
        [DataMember]
        public DateTime ScheduleDate
        {
            get
            {
                return _scheduledate;
            }
            set
            {
                _scheduledate = value;
            }
        }

        string _setupstation;
        ///<summary>
        ///安装点
        ///</summary>
        [DataMember]
        public string SetupStation
        {
            get
            {
                return _setupstation;
            }
            set
            {
                _setupstation = value;
            }
        }

        private string _showStatus;
        [DataMember]
        public string ShowStatus
        {
            get
            {
                return _showStatus;
            }
            set
            {
                _showStatus = value;
            }
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ApplicationID)))
            {
                builder.AppendLine("ApplicationID:" + ApplicationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Worker)))
            {
                builder.AppendLine("Worker:" + Worker.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }

            return builder.ToString();
        }

    }
}


using System;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
namespace Gsafety.PTMS.Common.Data
{
    ///<summary>
    ///维修申请
    ///</summary>
    [DataContract]
    public class MaintainApplication
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

        string _applicant;
        ///<summary>
        ///申请人
        ///</summary>
        [DataMember]
        public string Applicant
        {
            get
            {
                return _applicant;
            }
            set
            {
                _applicant = value;
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

        private string _problem;
         [DataMember]
        public string Problem
        {
            get
            {
                return _problem;
            }
            set
            {
                _problem = value;

            }
        }

        decimal _status;
        ///<summary>
        ///状态
        //0 已申请
        //1 已分配
        //2 已预约
        //3 维修中
        //4 维修完成
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

        string _worker;
        ///<summary>
        ///预约维修人
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

        DateTime? _scheduledate;
        ///<summary>
        ///预约时间
        ///</summary>
        [DataMember]
        public DateTime? ScheduleDate
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

        string _workerphone;
        ///<summary>
        ///维修人电话
        ///</summary>
        [DataMember]
        public string WorkerPhone
        {
            get
            {
                return _workerphone;
            }
            set
            {
                _workerphone = value;
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

        private string _contact;
        [DataMember]
        public string Contact
        {
            get
            {
                return _contact;
            }
            set
            {
                _contact = value;
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


        private Dictionary<string, string> _installStation = new Dictionary<string, string>();
         [DataMember]
        public Dictionary<string, string> ZInstallStation
        {
            get { return _installStation; }
            set
            {
                _installStation = value;

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
            if (!string.IsNullOrEmpty(Convert.ToString(Applicant)))
            {
                builder.AppendLine("Applicant:" + Applicant.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SetupStation)))
            {
                builder.AppendLine("SetupStation:" + SetupStation.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Worker)))
            {
                builder.AppendLine("Worker:" + Worker.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ScheduleDate)))
            {
                builder.AppendLine("ScheduleDate:" + ScheduleDate.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(WorkerPhone)))
            {
                builder.AppendLine("WorkerPhone:" + WorkerPhone.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}


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
    public class MaintainRecordUnfinished
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

        string _vehcileid;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public string VehcileID
        {
            get
            {
                return _vehcileid;
            }
            set
            {
                _vehcileid = value;
            }
        }

        string _applicant;
        ///<summary>
        ///
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

        string _contact;
        ///<summary>
        ///
        ///</summary>
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

        string _applicationid;
        ///<summary>
        ///
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

        decimal _applicationstatus;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public decimal ApplicationStatus
        {
            get
            {
                return _applicationstatus;
            }
            set
            {
                _applicationstatus = value;
            }
        }

        string _problem;
        ///<summary>
        ///
        ///</summary>
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

        DateTime? _scheduledate;
        ///<summary>
        ///
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

        string _setupstation;
        ///<summary>
        ///
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

        string _worker;
        ///<summary>
        ///
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

        DateTime ?_starttime;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        public DateTime? StartTime
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

        DateTime ?_endtime;
        ///<summary>
        ///
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

        string _note;
        ///<summary>
        ///
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

        decimal _status;
        ///<summary>
        ///
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
            if (!string.IsNullOrEmpty(Convert.ToString(VehcileID)))
            {
                builder.AppendLine("VehcileID:" + VehcileID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Applicant)))
            {
                builder.AppendLine("Applicant:" + Applicant.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contact)))
            {
                builder.AppendLine("Contact:" + Contact.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ApplicationID)))
            {
                builder.AppendLine("ApplicationID:" + ApplicationID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ApplicationStatus)))
            {
                builder.AppendLine("ApplicationStatus:" + ApplicationStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Problem)))
            {
                builder.AppendLine("Problem:" + Problem.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ScheduleDate)))
            {
                builder.AppendLine("ScheduleDate:" + ScheduleDate.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SetupStation)))
            {
                builder.AppendLine("SetupStation:" + SetupStation.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Worker)))
            {
                builder.AppendLine("Worker:" + Worker.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }

            return builder.ToString();
        }

    }
}


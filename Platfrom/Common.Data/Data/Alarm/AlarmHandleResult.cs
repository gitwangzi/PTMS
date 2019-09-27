using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class AlarmHandleResult
    {
        string alarmid = string.Empty;

        [DataMember]
        public string AlarmID
        {
            get { return alarmid; }
            set { alarmid = value; }
        }

        bool alarmflag = false;
        [DataMember]
        public bool AlarmFlag
        {
            get { return alarmflag; }
            set { alarmflag = value; }
        }

        DateTime handleTime;
        [DataMember]
        public DateTime HandleTime
        {
            get { return handleTime; }
            set { handleTime = value; }
        }

        bool _istransfer = false;
        [DataMember]
        public bool IsTransfer
        {
            get { return _istransfer; }
            set { _istransfer = value; }
        }
        string _note;
        [DataMember]
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        string _incidentaddress;
        [DataMember]
        public string IncidentAddress
        {
            get { return _incidentaddress; }
            set { _incidentaddress = value; }
        }

        string _incidentlevel;
        [DataMember]
        public string IncidentLevel
        {
            get { return _incidentlevel; }
            set { _incidentlevel = value; }
        }

        string _incidenttype;
        [DataMember]
        public string IncidentType
        {
            get { return _incidenttype; }
            set { _incidenttype = value; }
        }
    }
}

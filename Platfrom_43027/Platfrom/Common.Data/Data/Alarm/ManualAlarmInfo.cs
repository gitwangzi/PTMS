using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class ManualAlarmInfo
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        [DataMember]
        public string VehicleID { get; set; }

        [DataMember]
        public DateTime GPSTime { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public short Source { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public bool IsTransfer { get; set; }

        [DataMember]
        public short TransferMode { get; set; }

        [DataMember]
        public string Account { get; set; }
     
        [DataMember]
        public string IncidentAddress { get; set; }
     
        [DataMember]
        public int IncidentLevel { get; set; }
    
        [DataMember]
        public string IncidentType { get; set; }
    }
}

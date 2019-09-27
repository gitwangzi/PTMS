using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Contract.Data
{
    [DataContract]
    public class AlarmModel
    {
        [DataMember]
        public string VehicleSn { get; set; }

        [DataMember]
        public short? AlarmState { get; set; }

        [DataMember]
        public DateTime? AlarmTime { get; set; }

        [DataMember]
        public string LONGITUDE { get; set; }

        [DataMember]
        public string LATITUDE { get; set; }

        [DataMember]
        public string Speed { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Data
{
    [DataContract]
    public class VehicleModel
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string VehicleSn { get; set; }

        [DataMember]
        public string LONGITUDE { get; set; }

        [DataMember]
        public string LATITUDE { get; set; }

        [DataMember]
        public DateTime? GPS_Time { get; set; }
    }
}

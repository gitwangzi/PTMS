using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class GpsInfo
    {
        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public string Valid { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Height { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }


        [DataMember]
        public string GpsTime { get; set; }

        DateTime _gpstimevalue = DateTime.MinValue;
        [DataMember]
        public DateTime GpsTimeValue
        {
            get
            {
                return _gpstimevalue;
            }
            set
            {
                _gpstimevalue = value;
            }
        }

        [DataMember]
        public long AlarmFlag { get; set; }

        [DataMember]
        public long Status { get; set; }

        [DataMember]
        public string VehicleId { get; set; }
    }
}

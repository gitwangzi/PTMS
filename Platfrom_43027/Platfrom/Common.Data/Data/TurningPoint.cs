using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class TurningPoint
    {
        [DataMember]
        public int InflexionId { get; set; }

        [DataMember]
        public int RoadId { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public int? RoadWidth { get; set; }

        [DataMember]
        public List<int> RoadAttribute { get; set; }

        [DataMember]
        public int MaxRoute { get; set; }

        [DataMember]
        public int MinRoute { get; set; }

        [DataMember]
        public int MaxSpeed { get; set; }

        [DataMember]
        public int OverSpeedDuration { get; set; }
    }
}

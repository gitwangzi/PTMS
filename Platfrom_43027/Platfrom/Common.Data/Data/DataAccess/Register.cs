using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class Register
    {
        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public int SerialNo { get; set; }

        [DataMember]
        public string ManufactureId { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public string TerminalId { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string SIM { get; set; }
    }
}

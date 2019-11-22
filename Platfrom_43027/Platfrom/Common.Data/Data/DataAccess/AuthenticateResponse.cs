using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class AuthenticateResponse
    {
        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public int SerialNo { get; set; }

        [DataMember]
        public string RegisterNo { get; set; }

        [DataMember]
        public string SIM { get; set; }

        [DataMember]
        public int IsPassed { get; set; }

        [DataMember]
        public string VehicleId { get; set; }
    }
}

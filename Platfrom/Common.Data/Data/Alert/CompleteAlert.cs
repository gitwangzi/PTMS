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
    public class CompleteAlert
    {
        [DataMember]
        public string MdvrCoreId { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        [DataMember]
        public string VehicleID { get; set; }

        [DataMember]
        public string AlertID { get; set; }

        [DataMember]
        public List<string> Organizations { get; set; }
    }
}

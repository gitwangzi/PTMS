using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Contract.Data
{
    [DataContract]
    public class MDVR_Model
    {
        [DataMember]
        public string MDVR_SIM { get; set; }

        [DataMember]
        public string MDVR_CORE_SN { get; set; }

        [DataMember]
        public short? STATUS { get; set; }

        [DataMember]
        public string InstallPerson { get; set; }

        [DataMember]
        public DateTime? CREATE_TIME { get; set; }

    }
}

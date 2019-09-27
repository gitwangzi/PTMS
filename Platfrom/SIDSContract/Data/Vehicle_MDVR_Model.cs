using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Contract.Data
{
    [DataContract]
    public class Vehicle_MDVR_Model
    {
        [DataMember]
        public string VehicleSn { get; set; }

        [DataMember]
        public string Region_Name { get; set; }

        [DataMember]
        public string District_Code { get; set; }

        [DataMember]
        public string MDVR_Core_Sn { get; set; }
    }
}

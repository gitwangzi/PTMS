using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SIDS.Contract.Data
{
    [DataContract]
    public class PerVehicleModel
    {
        [DataMember]
        public string VehicleSn { get; set; }

        [DataMember]
        public string Brand_Model { get; set; }

        [DataMember]
        public string Start_Year { get; set; }

        [DataMember]
        public string Operation_Licence { get; set; }

        [DataMember]
        public string Owner { get; set; }

        [DataMember]
        public string Owner_Phone { get; set; }
    }
}

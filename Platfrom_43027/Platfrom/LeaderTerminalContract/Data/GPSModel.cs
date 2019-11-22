using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.Leader.Contract.Data
{
        [DataContract]
   public class GPSModel
    {
            [DataMember]
            public string Mdvr_Id { get ;set; }

            [DataMember]
            public string Vehcle_Id { get; set; }

            [DataMember]
            public string Longitude { get; set; }

            [DataMember]
            public string Latitude { get; set; }
    }
}

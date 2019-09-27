using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Leader.Contract.Data
{
      [DataContract]
  public  class VehicleModel
    {
        [DataMember]
        public string Mdvr_Id { get; set; }

        [DataMember]
        public string Vehcle_Id { get; set; }

        [DataMember]
        public DateTime GpsTime { get; set; }


    }
}

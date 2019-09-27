using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// Monitor Group Vehicle
    /// </summary>
    [DataContract]
    public class MonitorGroupVehicle
    {

        [DataMember]
        public string Group_ID;

        [DataMember]
        public string Vehicle_ID;

        [DataMember]
        public short? Traced_Flag;

        [DataMember]
        public int? Vehicle_Index;


        [DataMember]
        public string Vehicle_SN;
        [DataMember]
        public string MDVRID;

        [DataMember]
        public short? IsOnLine;
        [DataMember]
        public VehicleTypeEnum Type;
    }
}

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
    public class DeviceAlertStatistics
    {
        [DataMember]
        public string VehicleID { get; set; }

        [DataMember]
        public string OrganizatioName { get; set; }

        [DataMember]
        public short AlertType { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}

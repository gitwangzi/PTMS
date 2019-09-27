using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    /// <summary>
    /// IN and Out Fence Alert
    /// </summary>
    [DataContract]
    public class VehicleFenceAlert : DeviceAlert
    {
        /// <summary>
        /// FENCENAME
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// alertType
        /// </summary>
        [DataMember]
        public short alertType { get; set; }


        [DataMember]
        public DateTime? InFenceTime { get; set; }

        [DataMember]
        public DateTime? OutFenceTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(alertType)))
            {
                builder.AppendLine("alertType:" + alertType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InFenceTime)))
            {
                builder.AppendLine("InFenceTime:" + InFenceTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OutFenceTime)))
            {
                builder.AppendLine("OutFenceTime:" + OutFenceTime.ToString());
            }
            return builder.ToString();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    [DataContract]
    public class EmailAlertInfo
    {
        /// <summary>
        /// car no
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// over speend count
        /// </summary>
        [DataMember]
        public int OverSpeedCount { get; set; }

        /// <summary>
        /// the count in and out
        /// </summary>
        [DataMember]
        public int InOutFenceCount { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OverSpeedCount)))
            {
                builder.AppendLine("OverSpeedCount:" + OverSpeedCount.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InOutFenceCount)))
            {
                builder.AppendLine("InOutFenceCount:" + InOutFenceCount.ToString());
            }
            return builder.ToString();
        }

    }
}

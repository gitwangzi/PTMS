using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract.Data
{
    [DataContract]
    public class VehicleFenceFailedReason
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string CarNumber { get; set; }

        [DataMember]
        public string FenceId { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public DateTime SendTime { get; set; }

        [DataMember]
        public string FailedReason { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CarNumber)))
            {
                builder.AppendLine("CarNumber:" + CarNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FenceId)))
            {
                builder.AppendLine("FenceId:" + FenceId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FailedReason)))
            {
                builder.AppendLine("FailedReason:" + FailedReason.ToString());
            }
            return builder.ToString();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class SuiteInfoLog
    {
        ///<summary>
        ///Suite_ID
        ///</summary>
        [DataMember]
        public string Suite_ID { get; set; }
        ///<summary>
        ///Vehicle_ID
        ///</summary>
        [DataMember]
        public string Vehicle_ID { get; set; }
        ///<summary>
        ///CurrentStatus
        ///</summary>
        [DataMember]
        public string CurrentStatus { get; set; }
        ///<summary>
        ///ChangedStatus
        ///</summary>
        [DataMember]
        public string ChangedStatus { get; set; }
        ///<summary>
        ///OperatingTime
        ///</summary>
        [DataMember]
        public DateTime OperatingTime { get; set; }

        ///<summary>
        ///Operator
        ///</summary>
        [DataMember]
        public string Operator { get; set; }

        ///<summary>
        ///OperatingReason
        ///</summary>
        [DataMember]
        public string OperatingReason { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Suite_ID)))
            {
                builder.AppendLine("Suite_ID:" + Suite_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Vehicle_ID)))
            {
                builder.AppendLine("Vehicle_ID:" + Vehicle_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CurrentStatus)))
            {
                builder.AppendLine("CurrentStatus:" + CurrentStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChangedStatus)))
            {
                builder.AppendLine("ChangedStatus:" + ChangedStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperatingTime)))
            {
                builder.AppendLine("OperatingTime:" + OperatingTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Operator)))
            {
                builder.AppendLine("Operator:" + Operator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OperatingReason)))
            {
                builder.AppendLine("OperatingReason:" + OperatingReason.ToString());
            }
            return builder.ToString();
        }


    }
}

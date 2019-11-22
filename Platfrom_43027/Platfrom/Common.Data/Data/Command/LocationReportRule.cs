using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class LocationReportRule
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string ClientID { get; set; }
        [DataMember]
        public int ReportStrategy { get; set; }
        [DataMember]
        public int Length { get; set; }
        [DataMember]
        public int Interval { get; set; }
        [DataMember]
        public DateTime CreateTime { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public int Valid { get; set; }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsVisible { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ClientID)))
            {
                builder.AppendLine("ClientID:" + ClientID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ReportStrategy)))
            {
                builder.AppendLine("ReportStrategy:" + ReportStrategy.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Length)))
            {
                builder.AppendLine("Length:" + Length.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Valid)))
            {
                builder.AppendLine("ValID:" + Valid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Interval)))
            {
                builder.AppendLine("Interval:" + Interval.ToString());
            }
            return builder.ToString();
        }


    }
}

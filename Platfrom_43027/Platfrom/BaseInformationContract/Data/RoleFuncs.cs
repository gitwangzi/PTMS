using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    public class RoleFuncs
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public string FuncId { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RoleName)))
            {
                builder.AppendLine("RoleName:" + RoleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FuncId)))
            {
                builder.AppendLine("FuncId:" + FuncId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateDate)))
            {
                builder.AppendLine("CreateDate:" + CreateDate.ToString());
            }
            return builder.ToString();
        }

    }
}

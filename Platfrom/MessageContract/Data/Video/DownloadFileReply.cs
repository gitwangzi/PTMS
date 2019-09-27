using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data.Video
{
    [DataContract]
    [Serializable]
    public class DownloadFileReply
    {
        [DataMember]
        public string Mdvr_Id { get; set; }

        [DataMember]
        public string Uid { get; set; }

        [DataMember]
        public string ReplyResult { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Uid)))
            {
                builder.AppendLine("Uid:" + Uid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ReplyResult)))
            {
                builder.AppendLine("ReplyResult:" + ReplyResult.ToString());
            }
            return builder.ToString();
        }

    }
}

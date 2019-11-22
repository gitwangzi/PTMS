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
    public class QueryMdvrFileList
    {
        [DataMember]
        public string Mdvr_Id { get; set; }

        [DataMember]
        public List<int> Channel { get; set; }

        [DataMember]
        public int Video_Type { get; set; }

        [DataMember]
        public DateTime Start_Time { get; set; }

        [DataMember]
        public DateTime End_Time { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Video_Type)))
            {
                builder.AppendLine("Video_Type:" + Video_Type.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Start_Time)))
            {
                builder.AppendLine("Start_Time:" + Start_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(End_Time)))
            {
                builder.AppendLine("End_Time:" + End_Time.ToString());
            }
            return builder.ToString();
        }

    }
}

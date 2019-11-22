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
    public class QueryMdvrFileList
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string Mdvr_Id { get; set; }

        [DataMember]
        public List<int> Channel { get; set; }

        [DataMember]
        public DateTime Start_Time { get; set; }

        [DataMember]
        public DateTime End_Time { get; set; }

        [DataMember]
        public int StreamType { get; set; }

        /// <summary>
        /// 0.普通文件；1.报警文件；2.所有文件
        /// </summary>
        [DataMember]
        public int FileType { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserID)))
            {
                builder.AppendLine("UserID:" + UserID);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
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

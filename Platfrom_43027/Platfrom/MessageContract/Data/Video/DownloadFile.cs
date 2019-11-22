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
    public class DownloadFile
    {
        [DataMember]
        public string Mdvr_Id { get; set; }

        [DataMember]
        public int DownloadType { get; set; }

        [DataMember]
        public int? BeginTimeBackforward { get; set; }

        [DataMember]
        public int? EndTimeBackforward { get; set; }

        [DataMember]
        public int? BeginFileBackforward { get; set; }

        [DataMember]
        public int? EndFileBackforward { get; set; }

        [DataMember]
        public string Uid { get; set; }

        [DataMember]
        public string File_Id { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public string Port { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DownloadType)))
            {
                builder.AppendLine("DownloadType:" + DownloadType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BeginTimeBackforward)))
            {
                builder.AppendLine("BeginTimeBackforward:" + BeginTimeBackforward.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTimeBackforward)))
            {
                builder.AppendLine("EndTimeBackforward:" + EndTimeBackforward.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(BeginFileBackforward)))
            {
                builder.AppendLine("BeginFileBackforward:" + BeginFileBackforward.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndFileBackforward)))
            {
                builder.AppendLine("EndFileBackforward:" + EndFileBackforward.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Uid)))
            {
                builder.AppendLine("Uid:" + Uid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(File_Id)))
            {
                builder.AppendLine("File_Id:" + File_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Ip)))
            {
                builder.AppendLine("Ip:" + Ip.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Port)))
            {
                builder.AppendLine("Port:" + Port.ToString());
            }
            return builder.ToString();
        }

    }
}

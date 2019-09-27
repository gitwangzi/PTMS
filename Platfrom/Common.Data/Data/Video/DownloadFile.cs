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
    public class DownloadFile
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        [DataMember]
        public string UUID { get; set; }

        [DataMember]
        public string MdvrCoreSn { get; set; }

        [DataMember]
        public string ChannelID { get; set; }

        [DataMember]
        public string FileId { get; set; }

        [DataMember]
        public int OffSetFlag { get; set; }

        [DataMember]
        public DateTime StartTime { get; set; }

        [DataMember]
        public DateTime EndTime { get; set; }

        [DataMember]
        public int OffSet { get; set; }

        [DataMember]
        public int FileType { get; set; }

        [DataMember]
        public decimal FileSize { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ChannelID)))
            {
                builder.AppendLine("StreamName:" + ChannelID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FileId)))
            {
                builder.AppendLine("FileId:" + FileId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OffSetFlag)))
            {
                builder.AppendLine("OffSetFlag:" + OffSetFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OffSet)))
            {
                builder.AppendLine("OffSet:" + OffSet.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("MdvrCoreSn:" + MdvrCoreSn.ToString());
            }

            return builder.ToString();
        }

    }
}

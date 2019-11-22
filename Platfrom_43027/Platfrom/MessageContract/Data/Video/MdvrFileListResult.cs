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
    public class MdvrFileListResult
    {
        /// <summary>
        /// 文件唯一编号（由捷诺产生，以后根据该编号进行文件下载）
        /// </summary>
        [DataMember]
        public string Mdvr_File_Id { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public string Mdvr_File_Size { get; set; }
        /// <summary>
        /// 是否已经下载到服务器
        /// </summary>
        [DataMember]
        public bool Download_Flag { get; set; }
        /// <summary>
        /// 文件对应视频开始时间
        /// </summary>
        [DataMember]
        public DateTime Start_Time { get; set; }
        /// <summary>
        /// 文件对应视频结束时间
        /// </summary>
        [DataMember]
        public DateTime End_Time { get; set; }
        /// <summary>
        /// 通道号
        /// </summary>
        [DataMember]
        public int Channel { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_File_Id)))
            {
                builder.AppendLine("Mdvr_File_Id:" + Mdvr_File_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_File_Size)))
            {
                builder.AppendLine("Mdvr_File_Size:" + Mdvr_File_Size.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Download_Flag)))
            {
                builder.AppendLine("Download_Flag:" + Download_Flag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Start_Time)))
            {
                builder.AppendLine("Start_Time:" + Start_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(End_Time)))
            {
                builder.AppendLine("End_Time:" + End_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            return builder.ToString();
        }

    }
}

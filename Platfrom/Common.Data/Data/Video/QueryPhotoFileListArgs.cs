using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.Common.Data.Data.Video
{
    public class QueryPhotoFileListArgs
    {
        /// <summary>
        /// 设备芯片号
        /// </summary>
        [DataMember]
        public string MdvrCoreSn { get; set; }

        /// <summary>
        /// 1:第一路视频，2:第二路视频,99:所有通道；
        /// </summary>
        [DataMember]
        public int ChannelID { get; set; }

        /// <summary>
        /// 0:MDVR
        /// </summary>
        [DataMember]
        public int DeviceType { get; set; }

        /// <summary>
        /// 0.未标记；1.已标记；2.所有文件
        /// </summary>
        [DataMember]
        public PhotoStatusEnum Status { get; set; }

        /// <summary>
        /// 标注关键字
        /// </summary>
        [DataMember]
        public string Note { get; set; }

        /// <summary>
        /// 查询起始时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>
        [DataMember]
        public DateTime Start_Time { get; set; }

        /// <summary>
        /// 下载的截止时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>        
        [DataMember]
        public DateTime End_Time { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        [DataMember]
        public int PageNum { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int OrderBy { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreSn)))
            {
                builder.AppendLine("Mdvr_Id:" + MdvrCoreSn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ChannelID)))
            {
                builder.AppendLine("ChannelID:" + ChannelID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceType)))
            {
                builder.AppendLine("DeviceType:" + DeviceType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Status)))
            {
                builder.AppendLine("Status:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Start_Time)))
            {
                builder.AppendLine("Start_Time:" + Start_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(End_Time)))
            {
                builder.AppendLine("End_Time:" + End_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PageSize)))
            {
                builder.AppendLine("PageSize:" + PageSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PageNum)))
            {
                builder.AppendLine("PageNum:" + PageNum.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrderBy)))
            {
                builder.AppendLine("OrderBy:" + OrderBy.ToString());
            }
            return builder.ToString();
        }
    }
}

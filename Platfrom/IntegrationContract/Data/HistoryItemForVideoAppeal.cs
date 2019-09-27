using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract.Data
{
    [DataContract]
    public class HistoryItemForVideoAppeal
    {
        /// <summary>
        /// 报警时间
        /// </summary>
        [DataMember]
        public DateTime AppealTime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        [DataMember]
        public string Device { get; set; }

        /// <summary>
        /// 报警人
        /// </summary>
        [DataMember]
        public string Reportor { get; set; }

        /// <summary>
        /// 接警人
        /// </summary>
        [DataMember]
        public string Dealer { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// 视频类型
        /// </summary>
        [DataMember]
        public string VideoType { get; set; }

        /// <summary>
        /// mdvr
        /// </summary>
        [DataMember]
        public string MDVR { get; set; }

        /// <summary>
        /// 警情
        /// </summary>
        [DataMember]
        public string IsTrueAlarm { get; set; }

        /// <summary>
        /// 通道
        /// </summary>
        [DataMember]
        public int Channel { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal Latitude { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public decimal Longitude { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(AppealTime)))
            {
                builder.AppendLine("AppealTime:" + AppealTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mobile)))
            {
                builder.AppendLine("Mobile:" + Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Device)))
            {
                builder.AppendLine("Device:" + Device.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Reportor)))
            {
                builder.AppendLine("Reportor:" + Reportor.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Dealer)))
            {
                builder.AppendLine("Dealer:" + Dealer.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FilePath)))
            {
                builder.AppendLine("FilePath:" + FilePath.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoType)))
            {
                builder.AppendLine("VideoType:" + VideoType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR)))
            {
                builder.AppendLine("MDVR:" + MDVR.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsTrueAlarm)))
            {
                builder.AppendLine("IsTrueAlarm:" + IsTrueAlarm.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Address)))
            {
                builder.AppendLine("Address:" + Address.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            return builder.ToString();
        }
    }
}

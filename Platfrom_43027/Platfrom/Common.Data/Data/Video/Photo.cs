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
    public class Photo
    {
        /// <summary>
        /// 数据库主键
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        [DataMember]
        public int DeviceType { get; set; }

        [DataMember]
        public string DeviceSn { get; set; }
   
        [DataMember]
        public int ChannelID { get; set; }

        [DataMember]
        public string MiniImg_Url { get; set; }

        [DataMember]
        public string Img_Url { get; set; }

        [DataMember]
        public DateTime Create_Time { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public decimal Img_Size { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool IsChecked { get; set; }

        [DataMember]
        public bool IsVisibility { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}

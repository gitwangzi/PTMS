using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class TakePictureResponseJson
    {      
        [DataMember]
        public string UID { get; set; }

        /// <summary>
        /// 0、1、2、3分别表示通道1、2、3、4
        /// </summary>
        [DataMember]
        public int Channel { get; set; }

        [DataMember]
        public int SerialNo { get; set; }

        [DataMember]
        public string Time { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        /// <summary>
        /// 0:JPEG,1:TIF
        /// </summary>
        [DataMember]
        public int Format { get; set; }

        /// <summary>
        /// 0.平台下发指令；1.定时动作；2.抢劫报警触发；3.碰撞侧翻报警触发
        /// </summary>
        [DataMember]
        public int Event { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class GenenalResponse
    {
        [DataMember]
        public string UID { get; set; }//平台ID

        [DataMember]
        public int SerialNo { get; set; }//流水号

        [DataMember]
        public int ResponseSerialNo { get; set; }//应答流水号

        [DataMember]
        public string MessageID { get; set; }//消息ID

        [DataMember]
        public int Result { get; set; }//结果  0：成功/确认；1：失败；2：消息有误；3：不支持
    }
}

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
    public class AlarmNoteInfo
    {
        [DataMember]
        public string ID { get; set; }//ID

        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string Note { get; set; }//备注内容
     

    }
}

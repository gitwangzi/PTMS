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
    public class AlarmEmailInfo
    {
        [DataMember]
        public string ID { get; set; }//ID

        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public int EmailType { get; set; }  //0为报警，1为告警
     

    }
}

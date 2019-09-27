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
    public class AlarmTypeInfo
    {
       

        [DataMember]
        public string incidentTypeId { get; set; }

        [DataMember]
        public string typeCode { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string parentId { get; set; }

        [DataMember]
        public string rootId { get; set; }

        [DataMember]
        public string incTypeImg { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string hasSubType { get; set; }

    }
}

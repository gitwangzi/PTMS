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
    public class OnOffline
    {
        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public DateTime? OnOffLineTime { get; set; }

        [DataMember]
        public int IsOnline { get; set; }
    }
}

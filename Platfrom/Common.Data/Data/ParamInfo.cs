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
    public class ParamInfo
    {
        [DataMember]
        public uint ParaId { get; set; }

        [DataMember]
        public int ParaLen { get; set; }

        [DataMember]
        public string ParaValue { get; set; }
    }
}

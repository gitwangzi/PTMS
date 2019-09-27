using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class QueryPartParam
    {
        [DataMember]
        public string UID { get; set; }//GUID

        [DataMember]
        public int SerialNo { get; set; }//流水号

        [DataMember]
        public int ParamCount { get; set; }//参数总数

        [DataMember]
        public List<string> ParamList { get; set; }//参数项列表
    }
}

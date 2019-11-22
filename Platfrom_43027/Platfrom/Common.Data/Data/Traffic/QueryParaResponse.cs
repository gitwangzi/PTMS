using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class QueryParaResponse
    {
        [DataMember]
        public string UID { get; set; }//平台id

        [DataMember]
        public int SerialNo { get; set; }//流水号

        [DataMember]
        public int ParamCount { get; set; }//应答参数个数

        private List<ParamInfo> paramList;//参数项列表

        [DataMember]
        public List<ParamInfo> ParamList
        {
            get { return paramList; }
            set
            {
                paramList = value;
                if (paramList != null && paramList.Count > 0)
                    ParamCount = paramList.Count;
            }
        }
    }
}

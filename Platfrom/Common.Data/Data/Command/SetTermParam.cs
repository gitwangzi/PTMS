using Gsafety.Common.Util;
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
    public class SetTermParam
    {
        [DataMember]
        public string UID { get; set; }//平台ID

        //[DataMember]
        public int SerialNo = SerialNoHelper.Create();//流水号

        //[DataMember]
        public int ParamCount { get; set; }//参数总数

        private List<ParamInfo> paramList;//参数项列表

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

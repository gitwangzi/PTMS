using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class DelRouteRegion
    {

        [DataMember]
        public string UID { get; set; }//GUID

        [DataMember]
        public int SerialNo { get; set; }//流水号

        [DataMember]
        public int RouteCount { get; set; }//区域总数

        [DataMember]
        public List<int> RouteList { get; set; }//区域内容
    }
}

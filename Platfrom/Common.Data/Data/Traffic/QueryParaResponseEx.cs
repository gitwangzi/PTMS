using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class QueryParaResponseEx : QueryParaResponse
    {
        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string SuiteInfoID { get; set; }//安全套件信息GUID

        [DataMember]
        public short? SuiteStatus { get; set; }//套件状态

        [DataMember]
        public string SuiteID { get; set; }//套件号

        [DataMember]
        public string VehicleId { get; set; }//车牌号

        [DataMember]
        public int VehicleType { get; set; }//车辆类型

        [DataMember]
        public string ClientId { get; set; }//云账号客户ID
    }
}

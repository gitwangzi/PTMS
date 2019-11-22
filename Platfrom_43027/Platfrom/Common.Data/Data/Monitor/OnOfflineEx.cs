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
    public class OnOfflineEx : OnOffline
    {
        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string SuiteInfoID { get; set; }//安全套件信息GUID

        [DataMember]
        public string MdvrCoreId { get; set; }//芯片号

        [DataMember]
        public string ClientId { get; set; }//云账号客户ID

        [DataMember]
        public string OrganizationID { get; set; }

        [DataMember]
        public int SourceMode { get; set; }
    }
}

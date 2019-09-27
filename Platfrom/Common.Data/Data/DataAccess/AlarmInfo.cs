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
    public class AlarmInfo
    {
        [DataMember]
        public string UID { get; set; }//平台ID

        [DataMember]
        public int SerialNo { get; set; }//流水号

        [DataMember]
        public GpsInfo GpsInfo { get; set; }//GPS信息

        [DataMember]
        public string AdditionalInfo { get; set; }//附加信息
    }
}

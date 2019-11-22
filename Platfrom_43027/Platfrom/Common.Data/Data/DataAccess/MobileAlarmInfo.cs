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
    public class MobileAlarmInfo : AlarmInfo
    {
        [DataMember]
        public string Keyword { get; set; }//关键字

        [DataMember]
        public string AlarmContent { get; set; }//关键字
    }
}

using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    public class SetAlarmParaResponseJson
    {
        public string UID { get; set; }//平台ID

        public int SerialNo = SerialNoHelper.Create();//流水号

        public int Result { get; set; }
    }
}

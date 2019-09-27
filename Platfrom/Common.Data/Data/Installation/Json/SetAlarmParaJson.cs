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
    public class SetAlarmParaJson
    {
        public string UID { get; set; }//平台ID

        public int SerialNo = SerialNoHelper.Create();//流水号

        public List<int> ChannelList { get; set; }

        public int AlarmBeforeTime { get; set; }

        public int AlarmEndTime { get; set; }

        /// <summary>
        /// 1 GPS信息，2 报警日志信息，3 ACC信息， 4 设备状态信息，5 CAN数据信息，6 拨号日志信息， 7 GDS信息
        /// </summary>
        public int RelatedData { get; set; }
    }
}

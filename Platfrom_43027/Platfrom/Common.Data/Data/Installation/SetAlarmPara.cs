using Gsafety.PTMS.Common.Data.Enum;
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
    public class SetAlarmPara
    {
        [DataMember]
        public string CommandID { get; set; }

        [DataMember]
        public string InstallationDetailID { get; set; }

        [DataMember]
        public CommandStateEnum SuccessFlag { get; set; }

        [DataMember]
        public string MDVRID { get; set; }

        [DataMember]
        public List<int> ChannelList { get; set; }

        [DataMember]
        public int AlarmBeforeTime { get; set; }

        [DataMember]
        public int AlarmEndTime { get; set; }

        /// <summary>
        /// 1 GPS信息，2 报警日志信息，3 ACC信息， 4 设备状态信息，5 CAN数据信息，6 拨号日志信息， 7 GDS信息
        /// </summary>
        [DataMember]
        public int RelatedData { get; set; }
    }
}

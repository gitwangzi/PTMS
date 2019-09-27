using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Alert.Contract.Data
{
    /// <summary>
    /// 设备告警信息
    /// </summary>
    [DataContract]
    public class AlertDevice
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }
        /// <summary>
        /// 安全套件号
        /// </summary>
        [DataMember]
        public string SuiteID { get; set; }
        /// <summary>
        /// 告警时间
        /// </summary>
        [DataMember]
        public DateTime AlertTime { get; set; }
        /// <summary>
        /// 告警类型
        /// </summary>
        [DataMember]
        public short AlertType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [DataMember]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [DataMember]
        public DateTime EndTime { get; set; }
    }
}

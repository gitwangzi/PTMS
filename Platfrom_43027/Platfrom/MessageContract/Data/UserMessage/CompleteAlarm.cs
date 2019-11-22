using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Complete Alarm
    /// </summary>
    [Serializable]
    [DataContract]
    public class CompleteAlarm
    {
        bool _IsRealAlarm = false;
        /// <summary>
        /// MDVR Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Alarm Time
        /// </summary>
        [DataMember]
        public DateTime AlarmTime { get; set; }

        /// <summary>
        /// Complete Time
        /// </summary>
        [DataMember]
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// Is Real Alarm
        /// </summary>
        [DataMember]
        public bool IsRealAlarm
        {
            get { return _IsRealAlarm; }
            set { _IsRealAlarm = value; }
        }
    }
}

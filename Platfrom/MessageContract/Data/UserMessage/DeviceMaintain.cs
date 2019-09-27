using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Device Change to Maintain
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeviceMaintain
    {
        /// <summary>
        /// MDVR Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Maintain Time
        /// </summary>
        [DataMember]
        public DateTime MaintainTime { get; set; }
    }
}

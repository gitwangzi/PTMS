using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [Serializable]
    [DataContract]
    public enum AreaType
    {
        /// <summary>
        /// null
        /// </summary>
        [EnumMember]
        NoType = 0x00,
        /// <summary>
        /// ElectronicFence Region
        /// </summary>
        [EnumMember]
        ElectronicFence = 0x01,
        /// <summary>
        /// MonitoringPoint
        /// </summary>
        [EnumMember]
        MonitoringPoint = 0x02,
    }
}

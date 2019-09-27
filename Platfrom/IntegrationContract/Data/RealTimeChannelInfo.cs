using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract.Data
{
    [DataContract]
    public class RealTimeChannelInfo
    {
        /// <summary>
        /// vehicle sn
        /// </summary>
        [DataMember]
        public string VehicleSN { get; set; }
        /// <summary>
        /// mdvr id
        /// </summary>
        [DataMember]
        public string MdvrID { get; set; }

        /// <summary>
        /// chnannel id
        /// </summary>
        [DataMember]
        public string ChannelID { get; set; }

        [DataMember]
        public bool OneKey15Enabled { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// The Alert Of Completion
    /// </summary>
    [Serializable]
    [DataContract]
    public class CompleteAlert
    {
        /// <summary>
        /// MDVR Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Alert Time
        /// </summary>
        [DataMember]
        public DateTime AlertTime { get; set; }

        /// <summary>
        /// Complete Time
        /// </summary>
        [DataMember]
        public DateTime CompleteTime { get; set; }

        /// <summary>
        /// Alert Type
        /// </summary>
        [DataMember]
        public int AlertType { get; set; }
    }
}

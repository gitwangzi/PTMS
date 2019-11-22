using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    /// <summary>
    /// Device Install Message
    /// </summary>
    [Serializable]
    [DataContract]
    public class DeviceInstall
    {
        /// <summary>
        /// MDVR Core ID
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Install Complete Time
        /// </summary>
        [DataMember]
        public DateTime InstallCompleteTime { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallCompleteTime)))
            {
                builder.AppendLine("InstallCompleteTime:" + InstallCompleteTime.ToString());
            }
            return builder.ToString();
        }

    }
}

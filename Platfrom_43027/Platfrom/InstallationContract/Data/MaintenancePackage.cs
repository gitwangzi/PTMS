using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    [DataContract]
    public class MaintenancePackage
    {
        [DataMember]
        public string SuiteId { get; set; }

        [DataMember]
        public string SuiteKey { get; set; }

        [DataMember]
        public string InstallID { get; set; }

        [DataMember]
        public short DeviceStatus { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(SuiteKey)))
            {
                builder.AppendLine("SuiteKey:" + SuiteKey.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(InstallID)))
            {
                builder.AppendLine("InstallID:" + InstallID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceStatus)))
            {
                builder.AppendLine("DeviceStatus:" + DeviceStatus.ToString());
            }


            return builder.ToString();
        }
    }
}

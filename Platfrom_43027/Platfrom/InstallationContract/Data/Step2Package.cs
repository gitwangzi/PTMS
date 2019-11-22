using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    [DataContract]
    public class Step2Package
    {
        [DataMember]
        public string SuiteId { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string MDVR_CORE_SN { get; set; }

        [DataMember]
        public short SuiteStatus { get; set; }

        [DataMember]
        public DateTime SuiteSwitchTime { get; set; }

        [DataMember]
        public string AbnormalCause { get; set; }

        [DataMember]
        public short OnlineFlag { get; set; }

        [DataMember]
        public string InstallID { get; set; }

        [DataMember]
        public string SuiteKey { get; set; }

        [DataMember]
        public short SelfInspectCheck { get; set; }

        [DataMember]
        public short AlarmCheck { get; set; }

        [DataMember]
        public short VideoCheck { get; set; }

        [DataMember]
        public short IsSuccess { get; set; }

        [DataMember]
        public short GpsCheck { get; set; }

        [DataMember]
        public short VideoQualityCheck { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        [DataMember]
        public string Organization { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_CORE_SN)))
            {
                builder.AppendLine("MDVR_CORE_SN:" + MDVR_CORE_SN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteStatus)))
            {
                builder.AppendLine("SuiteStatus:" + SuiteStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteSwitchTime)))
            {
                builder.AppendLine("SuiteSwitchTime:" + SuiteSwitchTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalCause)))
            {
                builder.AppendLine("AbnormalCause:" + AbnormalCause.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OnlineFlag)))
            {
                builder.AppendLine("OnlineFlag:" + OnlineFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallID)))
            {
                builder.AppendLine("InstallID:" + InstallID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteKey)))
            {
                builder.AppendLine("SuiteKey:" + SuiteKey.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SelfInspectCheck)))
            {
                builder.AppendLine("SelfInspectCheck:" + SelfInspectCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmCheck)))
            {
                builder.AppendLine("AlarmCheck:" + AlarmCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoCheck)))
            {
                builder.AppendLine("VideoCheck:" + VideoCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsSuccess)))
            {
                builder.AppendLine("IsSuccess:" + IsSuccess.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsCheck)))
            {
                builder.AppendLine("GpsCheck:" + GpsCheck.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VideoQualityCheck)))
            {
                builder.AppendLine("VideoQualityCheck:" + VideoQualityCheck.ToString());
            }

            return builder.ToString();
        }
    }
}

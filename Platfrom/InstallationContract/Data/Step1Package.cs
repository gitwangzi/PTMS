using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    [DataContract]
    public class Step1Package
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string InstallationStationId { get; set; }

        [DataMember]
        public string InstallationStaff { get; set; }

        [DataMember]
        public string RecordStaff { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }

        [DataMember]
        public int Step { get; set; }

        [DataMember]
        public string Note { get; set; }

        [DataMember]
        public string ClientID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }

            if (!string.IsNullOrEmpty(Convert.ToString(InstallationStationId)))
            {
                builder.AppendLine("InstallationStationId:" + InstallationStationId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallationStaff)))
            {
                builder.AppendLine("InstallationStaff:" + InstallationStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RecordStaff)))
            {
                builder.AppendLine("RecordStaff:" + RecordStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Step)))
            {
                builder.AppendLine("Step:" + Step.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
           

            return builder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Common.Data.Data.Video
{
    [DataContract]
    [Serializable]
    public class TakePictureArgs
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string Mdvr_Core_Sn { get; set; }

        [DataMember]
        public List<int> Channel { get; set; }

        [DataMember]
        public int Cmd { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public int Resolution { get; set; }

        [DataMember]
        public int Quality { get; set; }

        [DataMember]
        public int Brightness { get; set; }

        [DataMember]
        public int Contrast { get; set; }

        [DataMember]
        public int Saturation { get; set; }

        [DataMember]
        public int Color { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(UserID)))
            {
                builder.AppendLine("UserID:" + UserID);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Core_Sn)))
            {
                builder.AppendLine("Mdvr_Core_Sn:" + Mdvr_Core_Sn.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel)))
            {
                builder.AppendLine("Channel:" + Channel.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Cmd)))
            {
                builder.AppendLine("Cmd:" + Cmd.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Interval)))
            {
                builder.AppendLine("Interval:" + Interval.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Resolution)))
            {
                builder.AppendLine("Resolution:" + Resolution);
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Quality)))
            {
                builder.AppendLine("Quality:" + Quality.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Brightness)))
            {
                builder.AppendLine("Brightness:" + Brightness.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Contrast)))
            {
                builder.AppendLine("Contrast:" + Contrast.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Saturation)))
            {
                builder.AppendLine("Saturation:" + Saturation.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Color)))
            {
                builder.AppendLine("Color:" + Color.ToString());
            }
            return builder.ToString();
        }
    }
}

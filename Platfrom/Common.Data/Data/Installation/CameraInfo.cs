using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gs.PTMS.Common.Data.Enum;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class CameraInfo
    {
        [DataMember]
        public CameraInstallLocationEnum InstallLocation
        {
            get;
            set;
        }

        [DataMember]
        public string ChannelID { get; set; }

        [DataMember]
        public string SuitPartID { get; set; }

        [DataMember]
        public string SuitPartSn { get; set; }
    }
}

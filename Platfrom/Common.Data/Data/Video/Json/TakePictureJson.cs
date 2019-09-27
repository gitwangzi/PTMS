using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Util;

namespace Gsafety.PTMS.Common.Data
{
    public class TakePictureJson
    {
        [DataMember]
        public string UID { get; set; }

        public int SerialNo = SerialNoHelper.Create();

        [DataMember]
        public int Channel { get; set; }

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
    }
}

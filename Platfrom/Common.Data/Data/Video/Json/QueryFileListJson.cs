using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class QueryFileListJson
    {
        [DataMember]
        public int StreamType { get; set; }

        [DataMember]
        public int FileType { get; set; }

        [DataMember]
        public List<int> Channel { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public string UID { get; set; }

        public int SerialNo = SerialNoHelper.Create();
    }
}

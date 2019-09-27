using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class QueryFileListResponseJson
    {
        [DataMember]
        public List<FileItem> FileItems { get; set; }

        [DataMember]
        public string UID { get; set; }

        [DataMember]
        public int SerialNo { get; set; }

        public class FileItem
        {
            [DataMember]
            public string RecordStartTime { get; set; }

            [DataMember]
            public string RecordEndTime { get; set; }

            [DataMember]
            public int RecordChannel { get; set; }

            [DataMember]
            public string RecordId { get; set; }

            [DataMember]
            public double RecordSize { get; set; }

            [DataMember]
            public int FileType { get; set; }
        }
    }
}

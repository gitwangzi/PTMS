using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    public class DownloadFileJson
    {
        public string StreamName { get; set; }

        public string RecordId { get; set; }

        public int OffSetFlag { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int OffSet { get; set; }

        public string UID { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(StreamName)))
            {
                builder.AppendLine("StreamName:" + StreamName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RecordId)))
            {
                builder.AppendLine("RecordId:" + RecordId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OffSetFlag)))
            {
                builder.AppendLine("OffSetFlag:" + OffSetFlag.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StartTime)))
            {
                builder.AppendLine("StartTime:" + StartTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(EndTime)))
            {
                builder.AppendLine("EndTime:" + EndTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OffSet)))
            {
                builder.AppendLine("OffSet:" + OffSet.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UID)))
            {
                builder.AppendLine("Uid:" + UID.ToString());
            }

            return builder.ToString();
        }

    }
}

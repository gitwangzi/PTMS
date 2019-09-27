using Gsafety.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    public class MDVRMessage
    {
        public string UID { get; set; }
        public int SerialNo = SerialNoHelper.Create();
        public List<int> TextFlag { get; set; }
        public string TextContent { get; set; }
    }
}

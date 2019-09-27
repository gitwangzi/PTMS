using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data.Data.DataAccess
{
    [Serializable]
    public class AppMessage
    {
        public string UID { get; set; }
        public string MessageId { get; set; }
        public string MessgeType { get; set; }
        public string MessageTitle { get; set; }
    }
}

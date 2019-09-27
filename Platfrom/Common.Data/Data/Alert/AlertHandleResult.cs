using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class AlertHandleResult
    {
        string alertid = string.Empty;

        [DataMember]
        public string AlertID
        {
            get { return alertid; }
            set { alertid = value; }
        }

        DateTime handleTime;
        [DataMember]
        public DateTime HandleTime
        {
            get { return handleTime; }
            set { handleTime = value; }
        }

        string content;
        [DataMember]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}

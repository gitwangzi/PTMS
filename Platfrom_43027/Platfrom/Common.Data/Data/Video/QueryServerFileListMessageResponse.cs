using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [Serializable]
    [DataContract]
    public class QueryServerFileListMessageResponse
    {
        public string UserID { get; set; }

        [DataMember]
        public List<QueryServerFileListMessage> QueryServerFileListMessages { get; set; }
    }
}

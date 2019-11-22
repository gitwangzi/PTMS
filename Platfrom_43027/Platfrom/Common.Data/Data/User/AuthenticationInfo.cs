using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    [Serializable]
    public class AuthenticationInfo
    {
        [DataMember]
        public bool Code { get; set; }

        [DataMember]
        public string UserToken { get; set; }
    }
}

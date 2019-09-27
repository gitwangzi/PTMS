using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [KnownType(typeof(GUser))]
    [KnownType(typeof(Role))]
    [DataContract]
    public class AccountInfo
    {
        [DataMember]
        public GUser User { get; set; }

        [DataMember]
        public Role Role { get; set; }

        [DataMember]
        public bool Allowed { get; set; }

        [DataMember]
        public short TransferMode { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}

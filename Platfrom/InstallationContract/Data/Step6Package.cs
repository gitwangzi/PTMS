using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    [DataContract]
    public class Step6Package
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public DateTime FinishTime { get; set; }
        [DataMember]
        public short CheckStep { get; set; }
        [DataMember]
        public string MdvrCoreId { get; set; }
        [DataMember]
        public short Status { get; set; }
    }
}

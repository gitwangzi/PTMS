using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    [DataContract]
    public class DeletePackage
    {
        [DataMember]
        public string SuiteKey { get; set; }

        [DataMember]
        public string InstallID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
   public class ChangeGroup
    {
        [DataMember]
        public List<string> GroupId;

        [DataMember] 
        public string CreatUser;
    }
}

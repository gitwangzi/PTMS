using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    [Serializable]
    public enum ElectricFenceOperType
    {
        [EnumMember]
        Add=1,
        [EnumMember]
        Modify = 2,
        [EnumMember]
        Delete = 3,
    }
}

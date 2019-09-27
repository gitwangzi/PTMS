using Gsafety.PTMS.Bases.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data.Enum
{
    [DataContract]
    [Serializable]
    public enum PhotoStatusEnum
    {
        [EnumMember]
        [EnumAttribute(ResourceName = "NoMark")]
        NoMark = 0,
        [EnumMember]
        [EnumAttribute(ResourceName = "Marked")]
        Marked = 1,
        [EnumMember]
        [EnumAttribute(ResourceName = "All")]
        All = 2,
    }
}

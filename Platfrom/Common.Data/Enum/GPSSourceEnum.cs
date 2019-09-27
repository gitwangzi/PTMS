using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Enum
{
    [DataContract]
    public enum GPSSourceEnum
    {
        [EnumMember]
        Suite = 0,
        [EnumMember]
        GPS,
        [EnumMember]
        Mobile,
        [EnumMember]
        UnKnown = 10,
    }
}

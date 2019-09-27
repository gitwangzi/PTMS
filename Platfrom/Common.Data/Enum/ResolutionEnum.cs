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
    public enum ResolutionEnum
    {
        [EnumMember]
        R_one = 320 * 240,
        [EnumMember]
        R_two = 640 * 480,
        [EnumMember]
        R_Three = 800 * 600,
        [EnumMember]
        R_Four = 1024 * 768,
        [EnumMember]
        R_Five = 176 * 144,
        [EnumMember]
        R_Six = 353 * 288,
        [EnumMember]
        R_Seven = 704 * 288,
        [EnumMember]
        R_Eight = 704 * 576
    }
}

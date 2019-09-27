using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
    [Serializable]
    [DataContract]
    public enum TemperatureSettingType
    {
        [EnumMember]
        DisEnable,
        [EnumMember]
        Enable,
        [EnumMember]
        EnableAndNoSettingValue
    }
}

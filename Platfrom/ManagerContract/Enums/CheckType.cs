using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Gsafety.PTMS.Manager.Repository
{
    [Serializable]
    [DataContract]
    public enum CheckType
    {
        [EnumMember]
        GpsSetting,
        [EnumMember]
        Temperature,
        [EnumMember]
        AbnormalDoor,
        [EnumMember]
        AlarmSetting

    }
}

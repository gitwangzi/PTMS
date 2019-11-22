using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data.CommandManage
{
    [Serializable]
    [DataContract]
    public enum SendInfomationType
    {
        [EnumMember]
        Car,
        [EnumMember]
        Alert
    }
    [Serializable]
    [DataContract]
    public enum DisplayPositionType
    {
        [EnumMember]
        DriverScreen,
        [EnumMember]
        CarFrontLED,
        [EnumMember]
        CarInnerLED,
        [EnumMember]
        CarInnerScreen,
        [EnumMember]
        CarOuterPlate,
        [EnumMember]
        CarBroadside,
        [EnumMember]
        CarAfterbody,
    }

}

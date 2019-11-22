using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Gsafety.PTMS.Bases.Enums;
namespace Gsafety.PTMS.Manager.Models
{
    public enum TemperatureSettingType
    {
        [Enum(ResourceName = "MANAGE_DisEnable_Alert")]
        DisEnable,
        [Enum(ResourceName = "MANAGE_Enable_Alert")]
        Enable,
        //[Enum(ResourceName e= "MANAGE_EnableNotSetting")]
        //EnableAndNoSettingValue
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Gsafety.PTMS.Bases.Enums;
namespace Gsafety.PTMS.Manager.Models
{
    public enum TemperatureType
    {
        [Enum(ResourceName = "MANAGE_TemperatureType_Suite")]
        Suite,
        [Enum(ResourceName = "MANAGE_TemperatureType_Engine")]
        Engine,
        [Enum(ResourceName = "MANAGE_TemperatureType_Coache")]
        Coache
    }
}

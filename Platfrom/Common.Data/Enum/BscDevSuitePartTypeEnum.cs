using Gsafety.PTMS.Bases.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Enum
{
    /// <summary>
    /// 套件配件类型枚举
    /// </summary>
    public enum BscDevSuitePartTypeEnum
    {
        [Description("AlarmButton")]
        [EnumAttribute(ResourceName = "AlarmButton")]
        AlarmButton = 0,

        [Description("Camera")]
        [EnumAttribute(ResourceName = "Camera")]
        Camera,

        [Description("Screen")]
        [EnumAttribute(ResourceName = "Screen")]
        Screen
    }
}

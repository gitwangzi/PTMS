using Gsafety.PTMS.Bases.Enums;
using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Enums
{
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

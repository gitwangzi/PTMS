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

namespace Gsafety.PTMS.Bases.Enums
{
    public enum ProtocolTypeEnum
        : short
    {
        [Description("Stander808")]
        [EnumAttribute(ResourceName = "Stander808")]
        Stander808 = 0,

        [Description("N9m")]
        [EnumAttribute(ResourceName = "N9m")]
        N9m = 1,

        [Description("Other")]
        [EnumAttribute(ResourceName = "Other")]
        Other = 99
    }
}

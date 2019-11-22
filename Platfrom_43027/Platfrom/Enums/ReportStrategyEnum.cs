using System;
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
    public enum ReportStrategyEnum
    {
        [Enum(ResourceName = "ReportStrategyEnum_ByInterval")]
        ByInterval = 0,
        [Enum(ResourceName = "ReportStrategyEnum_ByLength")]
        ByLength = 1,
        [Enum(ResourceName = "ReportStrategyEnum_ByIntervalAndLength")]
        ByIntervalAndLength = 2
    }
}

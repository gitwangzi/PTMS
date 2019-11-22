using Gsafety.PTMS.Monitor;
using Jounce.Core.ViewModel;
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
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using System.Collections.Generic;

namespace Gsafety.Ant.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.AddMonitorGroupVm)]
    public class AddMonitorGroupViewModel
    {
        protected static List<RunMonitorGroup> monitorGroupList = new List<RunMonitorGroup>();
    }
}

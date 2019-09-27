﻿using Jounce.Core.ViewModel;
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

namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.DeviceAlertByMonthVm)]
    public class DeviceAlertByMonthViewModel : DeviceAlertBaseViewModel
    {
        public DeviceAlertByMonthViewModel()
        {
            GroupMode = 2;
        }
    }
}

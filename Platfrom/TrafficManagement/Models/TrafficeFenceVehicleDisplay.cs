﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Traffic.Models
{
    public class TrafficeFenceVehicleDisplay
    {
        bool _show;

        public bool Show
        {
            get { return _show; }
            set { _show = value; }
        }
    }
}

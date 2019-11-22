using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Command =  Gsafety.PTMS.ServiceReference.CommandManageService;
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

namespace Gsafety.PTMS.Traffic.Models
{
    public class SelectedFenceChange
    {
        TrafficFence _selectedfence;

        public TrafficFence SelectedFence
        {
            get { return _selectedfence; }
            set { _selectedfence = value; }
        }
    }

    public class SelectedRouteChange
    {
        TrafficRoute _selectroute;
        public TrafficRoute SelectedRoute
        {
            get { return _selectroute; }
            set { _selectroute = value; }
        }
    }

    public class SelectedSpeedLimitChange
    {
        Command.SpeedLimit _selectSpeedLimit;
        public Command.SpeedLimit SelectedSpeedLimit
        {
            get { return _selectSpeedLimit; }
            set { _selectSpeedLimit = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Share.MessageManage
{
    public partial class MessageCenterClient
    {
        List<string> _vehicles = new List<string>();

        public void MonitorVehicle(List<string> vehicles)
        {
            ObservableCollection<string> sendvehicles = new ObservableCollection<string>();
            foreach (var item in vehicles)
            {
                if (!_vehicles.Contains(item))
                {
                    _vehicles.Add(item);
                    sendvehicles.Add(item);
                }
            }

            if (sendvehicles.Count > 0)
            {
                _messageClient.MonitorVehicleAsync(_sessionid, sendvehicles);
            }
        }

        public void UnMonitorVehicle(List<string> vehicles)
        {
            ObservableCollection<string> sendvehicles = new ObservableCollection<string>();
            foreach (var item in vehicles)
            {
                if (_vehicles.Contains(item))
                {
                    _vehicles.Remove(item);
                    sendvehicles.Add(item);
                }
            }

            if (sendvehicles.Count > 0)
            {
                _messageClient.UnMonitorVehicleAsync(_sessionid, sendvehicles);
            }
        }
    }
}

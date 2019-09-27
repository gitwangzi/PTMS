using Gsafety.PTMS.ServiceReference.OrderClientService;
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

namespace SuperPowerManagement.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderClientInfo : OrderClient
    {
        public int Order { get; set; }
        public string Organization { get; set; }
        public string UserName { get; set; }
        public string DeviceCount { get; set; }
        public string ActualUser { get; set; }
    }
}

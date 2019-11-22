using Gsafety.PTMS.ServiceReference.VehicleService;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.ReportManager
{
    public class VehicleTypeManager
    {
        static List<VehicleType> vehicletypes = null;

        public static List<VehicleType> VehicleTypes
        {
            get
            {
                return vehicletypes;
            }
            set
            {
                vehicletypes = value;
            }
        }
    }
}

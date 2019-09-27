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
using Gsafety.PTMS.ServiceReference.RunVehicleLocationService;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
using BaseLib.ViewModels;
using GisManagement.Models;
using BaseLib.Model;

namespace GisManagement.ViewModels
{
    public partial class GisViewModel
    {
        private PagedServerCollection<RunVehicleLocation> vehicleInfoList;
        public PagedServerCollection<RunVehicleLocation> VehicleInfoList
        {
            get { return vehicleInfoList; }
            set
            {
                vehicleInfoList = value;
                RaisePropertyChanged(() => VehicleInfoList);
            }
        }
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8886f443-2ea7-4471-bb84-bf8a3794bb98      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.ViewModels
/////    Project Description:    
/////             Class Name: AlarmDetailPageVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 9:28:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 9:28:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using Jounce.Core.ViewModel;
using Jounce.Core.Event;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Monitor;

namespace Gsafety.PTMS.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.VehicleDetailViewModel)]
    public class VehicleDetailViewModel : BaseViewModel
    {
        public Vehicle VehicleDetailInfo { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Company { get; set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            if (viewParameters.ContainsKey("Vehicle"))
            {
                VehicleDetailInfo = viewParameters["Vehicle"] as Vehicle;
                RaisePropertyChanged(() => this.VehicleDetailInfo);
            }
        }

        public VehicleDetailViewModel()
        {
        }
    }
}

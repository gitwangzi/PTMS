using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b328c01c-8273-4080-ab51-330230b5ad27      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert
/////    Project Description:    
/////             Class Name: AlertBingding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:23:43 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:23:43 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Alert
{
    public class AlertBinding
    {
        [Export]
        public ViewModelRoute VehicleAlertBinding
        {
            get { return ViewModelRoute.Create(AlertName.VehicleAlertViewModel, AlertName.VehicleAlertView); }
        }

        [Export]
        public ViewModelRoute BindingAlertDetail
        {
            get { return ViewModelRoute.Create(AlertName.AlertDetailPageViewModel, AlertName.AlertDetailPage); }
        }
    }
}

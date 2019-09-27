/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 724586fe-ee3e-4e05-ac16-6dec1e45b041      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Models
/////    Project Description:    
/////             Class Name: VehicleAlertModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 18:00:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 18:00:22
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

namespace Gsafety.PTMS.Alert.Models
{
    public class VehicleAlertModel
    {
        public string ID { get; set; }
        public string CarNumber { get; set; }
        public short? AlertType { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}

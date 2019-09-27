/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b31e4b59-c979-4734-8968-632a5feda826      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: RequestVehicleAlarmArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/7 17:46:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/7 17:46:43
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

namespace Gsafety.Common.CommMessage
{
    public class RequestVehicleAlarmArgs
    {
        /// <summary>
        /// alarm ID
        /// </summary>
        public string ID { get; set; }
        
        public string VehicleID { get; set; }
        /// <summary>
        /// operator type
        /// Op=0，location alarm point
        /// Op=1，request current location
        /// Op=2,  follow location 
        /// </summary>
        public int Op { get; set; }
        /// <summary>
        /// display layer
        /// IsDisposed=0，unhandler alarm
        /// IsDisposed=1，handler alarm
        /// </summary>
        public int IsDispsed { get; set; }
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2a9dce28-c578-488b-8847-af167e41a820      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: DrawRoutEventArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/9 10:05:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/9 10:05:22
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
using Gsafety.PTMS.ServiceReference.TrafficManageService;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// notice GIS windows start draw route，insert into spatial database when draw completed
    /// </summary>
    public class DrawRoutEventArgs
    {
        public TrafficRoute Route;
    }
}

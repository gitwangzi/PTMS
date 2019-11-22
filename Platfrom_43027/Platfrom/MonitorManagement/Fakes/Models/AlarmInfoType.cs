/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 05248f63-485d-42ae-aab8-537411ef386d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Models
/////    Project Description:    
/////             Class Name: AlarmInfoType
/////          Class Version: v1.0.0.0
/////            Create Time: 9/15/2013 10:44:44 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/15/2013 10:44:44 AM
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

namespace Gsafety.PTMS.Monitor.Models
{
    public enum AlarmInfoType
    {
        BasicInfo = 0x1,
        HandedInfo = 0x2,
        EC911Info = 0x3,
    }
}

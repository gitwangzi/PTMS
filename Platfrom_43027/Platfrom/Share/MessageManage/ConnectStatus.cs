/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cce65b1f-c5c6-4a15-a40a-56793edbcede      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Share.MessageManage
/////    Project Description:    
/////             Class Name: ConnectStatus
/////          Class Version: v1.0.0.0
/////            Create Time: 11/26/2013 9:59:24 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/26/2013 9:59:24 AM
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

namespace Gsafety.Ant.Share
{
    public enum MessageServiceStatus
    {
        RequestConnect = 0x01,
        DisConnected = 0x02,
        Connected = 0x03,
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 01b82aec-9b8b-4a5f-8b57-0790fa3b21fe      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: DisplayCurrentPositionArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/16 10:15:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/16 10:15:07
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
    public class DisplayCurrentPositionArgs
    {
        public string CarNo { get; set; }
        public string Prov { get; set; }
        //the layer type for operation（miVEGpsData = 1, miVEOneKeyAlarm = 2, miVEHisData = 3, miVEDisposedOneKeyAlarm = 4, miVEAlert = 5, miVEAlertDisposed = 6, miVETraffic=7）
        public int VE;
    }
}

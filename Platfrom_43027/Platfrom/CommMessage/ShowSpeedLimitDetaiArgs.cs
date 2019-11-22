using Gsafety.PTMS.ServiceReference.CommandManageService;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 229ccdd6-0fd4-4e32-9b06-8f22f1afdc30      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ShowSpeedLimitDetaiArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/31 9:45:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/31 9:45:26
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
    /// <summary>
    /// display limit speed rule xiangxi information
    /// </summary>
    public class ShowSpeedLimitDetaiArgs
    {
        public SpeedLimit selectSpeedLimit { get; set; }
    } 
}

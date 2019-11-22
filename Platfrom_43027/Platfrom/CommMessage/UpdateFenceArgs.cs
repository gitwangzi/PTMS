using Gsafety.PTMS.ServiceReference.TrafficManageService;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 25bd1a2e-ff3c-40c5-aedb-5b81b7a077fe      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: UpdateFence
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 17:07:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 17:07:38
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
    public class UpdateFenceArgs
    {
        /// <summary>
        /// fence infomation
        /// </summary>
        public TrafficFence eleFence{get;set;}

    }
}

using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6973aa2e-fd47-4f15-a856-82f00e73572a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: DrawFenceEventArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/9 10:02:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/9 10:02:52
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
    /// draw fence information args
    /// </summary>
    public class DrawFenceEventArgs
    {

        public TrafficDrawType nType;
        /// <summary>
        /// radius
        /// </summary>
        public double dDist;
        /// <summary>
        /// fence information
        /// </summary>
        public TrafficFence eleFence;
    }
}

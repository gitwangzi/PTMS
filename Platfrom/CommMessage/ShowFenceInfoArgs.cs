using Gsafety.PTMS.ServiceReference.TrafficManageService;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2d5b2334-9228-4960-b25c-ed1a0f99996a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ShowFenceInfoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/9 10:03:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/9 10:03:35
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
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.CommMessage
{
    /// <summary>
    /// display fence message args
    /// </summary>
    public class ShowFenceInfoArgs
    {
        /// <summary>
        /// choose fence
        /// </summary>
        public TrafficFence selectEleFence { get; set; }

        /// <summary>
        /// 选择的线路
        /// </summary>
        public TrafficRoute selectRoute { get; set; }
        /// <summary>
        /// type：PolygonFence monitor point
        /// </summary>
        public TrafficFeature Featuetype { get; set; }
    }
}

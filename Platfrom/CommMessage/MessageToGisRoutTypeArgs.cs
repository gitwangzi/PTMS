/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 60a822ca-9265-4b81-9103-ef709f0cdaa8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: MessageToGisTrfficFeatureTypeArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/25 13:38:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/25 13:38:18
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
    /// notice gis the current operator type of route 
    /// </summary>
    public class MessageToGisTrfficFeatureTypeArgs
    {
        /// <summary>
        /// traffice management module
        /// </summary>
        public TrafficFeature FeatureType { get; set; }
        /// <summary>
        ///  if it is route must remind specific route 
        /// </summary>
        public enumRouteType RoutType { get; set; }
    }
}

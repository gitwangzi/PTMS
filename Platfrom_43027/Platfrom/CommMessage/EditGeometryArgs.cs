using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e310075c-f0ce-44dd-b9a0-baed21cac3e3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: EditGeometryArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/22 11:31:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/22 11:31:03
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
    /// edit key element information
    /// </summary>
    public class EditGeometryArgs
    {
        /// <summary>
        /// edit layer type
        /// </summary>
        public TrafficFeature nType { get; set; }
        /// <summary>
        /// choose object
        /// </summary>
        public TrafficFence selectFence { get; set; }
        public TrafficRoute selectRoute { get; set; }
    }
}

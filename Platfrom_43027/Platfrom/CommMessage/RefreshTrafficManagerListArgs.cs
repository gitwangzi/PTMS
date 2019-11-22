using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4c6802ec-2658-4c93-8daa-9c74ca8a0d1f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: RefreshTrafficManagerList
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/29 17:10:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/29 17:10:08
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
    /// notice traffice manamgent windows refresh date
    /// </summary>
    public class RefreshTrafficManagerListArgs
    {
        /// <summary>
        /// type
        /// </summary>
        public TrafficFeature nType { get; set; }
        /// <summary>
        /// whether requery
        /// </summary>
        public bool bReQuery { get; set; }
        /// <summary>
        /// the object after query 
        /// </summary>
        public object UpdateItemInfo { get; set; }
    }
}

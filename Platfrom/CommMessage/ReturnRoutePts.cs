/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dcdc3c42-a26f-4325-a080-ed01a9c85972      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ReturnRoutePts
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 14:17:28
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 14:17:28
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
    /// back route pointlist information
    /// </summary>
    public class ReturnRoutePts
    {
        /// <summary>
        /// pointlist
        /// </summary>
        public string routPTS { get; set; }
        /// <summary>
        /// type
        /// </summary>
        public enumRouteType nType { get; set; }
    }
}

using Gsafety.PTMS.Bases.Enums;
using Gsafety.Common.CommMessage.Controls;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1f0086e9-d887-4768-9bae-86c22293c9af      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: MarkTrafficGraphicByCarID
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/26 17:40:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/26 17:40:27
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
    /// notice gis display traffice element;fence and route
    /// </summary>
    public class MarkTrafficGraphicByCarID
    {
        /// <summary>
        /// type
        /// </summary>
        public TrafficFeature nType { get; set; }
        /// <summary>
        /// whether display true:display false : undisplay
        /// </summary>
        public bool bShow { get; set; }
        /// <summary>
        /// car num
        /// </summary>
        public string carID { get; set; }
        /// <summary>
        /// whether locate
        /// </summary>
        public bool bLocate { get; set; }
        /// <summary>
        /// draw sign style
        /// </summary>
        public SymbolParams MarkSymbolParm{get;set;}

    }
}

using Gsafety.PTMS.Bases.Enums;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c4e9e42d-77b4-47f6-ba02-6d2a39f3eb4d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: ClearTrafficMaps
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 17:56:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 17:56:46
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
    /// clear traffice control GIS map, clear MyGraphicLayer layer， 
    /// and display according to the traffice type
    /// </summary>
    public class ClearTrafficMaps
    {
        //0fence 1route 2station
        public TrafficFeature nType { get; set; }
    }
}

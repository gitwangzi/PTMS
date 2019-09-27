/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2ba85cd6-8305-4ac8-bdaa-1ef8829f845e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: ToVehicleView
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/1/13 15:05:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/1/13 15:05:52
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.View;
using Jounce.Regions.Core;

namespace Gsafety.PTMS.Traffic.Views
{
    [ExportAsView(TrafficName.ToVehicleView)]
    [ExportViewToRegion(TrafficName.ToVehicleView, TrafficName.TrafficContainer)]
    public partial class ToVehicleView : UserControl
    {
        public ToVehicleView()
        {
            InitializeComponent();
        }
    }
}

using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3560e1fa-2b5b-486f-85d4-f435fd4ad864      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: VehicleRuleDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/9 10:40:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/9 10:40:08
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

namespace Gsafety.PTMS.Traffic.Views
{
    [ExportAsView(TrafficName.RuleToVehicleDetailView)]
    [ExportViewToRegion(TrafficName.RuleToVehicleDetailView, TrafficName.TrafficContainer)]
    public partial class RuleToVehicleDetailView : UserControl
    {
        public RuleToVehicleDetailView()
        {
            InitializeComponent();
        }
    }
}

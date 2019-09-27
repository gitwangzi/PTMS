/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 426dd094-41cb-4b51-91f0-947017e5c956      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: RuleToVehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/3 11:23:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/3 11:23:16
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
    [ExportAsView(TrafficName.RuleToVehicleView, Category = TrafficName.CategoryName,
    MenuName = TrafficName.SpeedMenuName, MenuTitle = "TRAFFICE_Rule_Vehicle",
    ToolTip = "Click to view some text.", Url = "/RuleToVehicleView", Order = 1)]
    [ExportViewToRegion(TrafficName.RuleToVehicleView, TrafficName.TrafficContainer)]
    public partial class RuleToVehicleView : UserControl
    {
        public RuleToVehicleView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ruletovehicleDataGrid);
        }
    }
}

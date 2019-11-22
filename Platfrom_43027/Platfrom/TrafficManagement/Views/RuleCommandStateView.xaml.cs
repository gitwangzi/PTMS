/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: ee66b6b2-a45b-4f2d-81f9-43032582f5d6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: RuleCommandState
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/3 14:28:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/3 14:28:11
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
    [ExportAsView(TrafficName.RuleCommandStateView, Category = TrafficName.CategoryName,
     MenuName = TrafficName.SpeedMenuName, MenuTitle = "TRAFFIC_SpeedRule_Failed",
    ToolTip = "Click to view some text.", Url = "/RuleCommandStateView", Order = 2)]
    [ExportViewToRegion(TrafficName.RuleCommandStateView, TrafficName.TrafficContainer)]
    public partial class RuleCommandStateView : UserControl
    {
        public RuleCommandStateView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(rulecommandtovehicleDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}

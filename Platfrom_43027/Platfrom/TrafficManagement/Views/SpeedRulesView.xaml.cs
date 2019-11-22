/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 868c836b-9c18-4fb5-ad93-5f4cb3bc11b0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: SpeedRules
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/27 17:09:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/27 17:09:05
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
    [ExportAsView(TrafficName.SpeedRulesView, Category = TrafficName.CategoryName,
    MenuName = TrafficName.SpeedMenuName, MenuTitle = "TRAFFIC_SpeedLimit",
    ToolTip = "Click to view some text.", Url = "/SpeedRulesView", Order = 0)]
    [ExportViewToRegion(TrafficName.SpeedRulesView, TrafficName.TrafficContainer)]
    public partial class SpeedRulesView : UserControl
    {
        public SpeedRulesView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SpeedlimitDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }


        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}

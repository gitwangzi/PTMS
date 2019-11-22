/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 46b0d8d0-88d0-4606-89ee-f18810cd01bf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: SpeedRuleDetailView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/4 16:14:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/4 16:14:52
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
    [ExportAsView(TrafficName.SpeedRuleDetailView)]
    [ExportViewToRegion(TrafficName.SpeedRuleDetailView, TrafficName.TrafficContainer)]
    public partial class SpeedRuleDetailView : UserControl
    {
        public SpeedRuleDetailView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}

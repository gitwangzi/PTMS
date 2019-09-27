/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1551fa89-0dde-4ee6-8565-0457268af664      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: TrafficUserAddView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 15:18:50
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 15:18:50
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
namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.TrafficUserAddView)]
    [ExportViewToRegion(ManagerName.TrafficUserAddView, ManagerName.ManagerContainer)]
    public partial class TrafficUserAddView : UserControl
    {
        public TrafficUserAddView()
        {
            InitializeComponent();
        }
    }
}

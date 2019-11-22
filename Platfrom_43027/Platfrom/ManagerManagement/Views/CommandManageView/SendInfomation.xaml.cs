using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0fc9dea7-eb30-4af6-99ab-f7677f166d9e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: TemperatureAddRule
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/30 15:08:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/30 15:08:11
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

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.SendInfomationView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.CommandManageMenuName, MenuTitle = "SendInfomation",
        ToolTip = "Click to view some text.", Url = "/SendInfomationView", Order = 3)]
    [ExportViewToRegion(ManagerName.SendInfomationView, ManagerName.ManagerContainer)]
    public partial class SendInfomationView : UserControl
    {
        public SendInfomationView()
        {
            InitializeComponent();
        }
    }
}

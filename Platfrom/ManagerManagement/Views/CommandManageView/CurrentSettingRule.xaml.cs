/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: bbbded90-fa57-4218-9d0e-a058ddbc3431      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: CurrentSettingRule
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/7 16:40:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/7 16:40:37
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
using System.ComponentModel;

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.CurrentSettingRuleView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.CommandManageMenuName, MenuTitle = "CURRENT_SettingGule",
        ToolTip = "Click to view some text.", Url = "/CurrentSettingRule", Order = 5)]
    [ExportViewToRegion(ManagerName.CurrentSettingRuleView, ManagerName.ManagerContainer)]
    public partial class CurrentSettingRule : UserControl
    {
        public CurrentSettingRule()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(CurrentSettingRuleDataGrid);
        }
    }
}

/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: d2b525f9-8e00-40c5-be7b-fdeebfdca70d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.CommandManageView
/////    Project Description:    
/////             Class Name: RuleSettingLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/12 14:05:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/12 14:05:04
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

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    [ExportAsView(ManagerName.RuleSettingLogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGE_Rule_Setting_Log",
        ToolTip = "Click to view some text.", Url = "/RuleSettingLogView", Order = 11)]
    [ExportViewToRegion(ManagerName.RuleSettingLogView, ManagerName.ManagerContainer)]
    public partial class RuleSettingLogView : UserControl
    {
        public RuleSettingLogView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(RuleSettingLogDataGrid);
        }
    }
}

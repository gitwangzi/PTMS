
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 340a5d5e-3cb9-40c1-aff4-2c05d4803cb8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: AlertTypeColorSet
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 19:03:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 19:03:34
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
     [ExportAsView(ManagerName.AlertTypeColorSet, Category = ManagerName.CategoryName,
        MenuName = ManagerName.SettingManageMenuName, MenuTitle = "MANAGER_AlertTypeColorSetting",
        ToolTip = "Click to view some text.", Url = "/AlertTypeColorSet", Order = 3)]
     [ExportViewToRegion(ManagerName.AlertTypeColorSet, ManagerName.ManagerContainer)]
    public partial class AlertTypeColorSet : UserControl
    {
        public AlertTypeColorSet()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UserListDataGrid);
        }

    }
}

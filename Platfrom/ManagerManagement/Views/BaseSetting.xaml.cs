/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 92bf8f50-197d-41ff-8b77-cacc47cecdc5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: BaseSetting
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 15:56:43
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 15:56:43
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
    [ExportAsView(ManagerName.BaseSetting, Category = ManagerName.CategoryName,
       MenuName = ManagerName.SettingManageMenuName, MenuTitle = "MANAGER_BasicinfoSetting",
       ToolTip = "Click to view some text.", Url = "/BaseSetting", Order = 1)]
    [ExportViewToRegion(ManagerName.BaseSetting, ManagerName.ManagerContainer)]
    public partial class BaseSetting : UserControl
    {
        public BaseSetting()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UserListDataGrid);
        }
    }
}

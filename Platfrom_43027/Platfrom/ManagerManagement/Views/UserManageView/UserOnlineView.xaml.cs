/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 235fa621-f826-40a9-b8d7-978c73b167c3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views
/////    Project Description:    
/////             Class Name: UserListView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/6 15:16:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/6 15:16:09
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Manager.Views.LogManageView;
using Jounce.Core.View;
using Jounce.Regions.Core;
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

namespace Gsafety.PTMS.Manager.Views.UserManageView
{
    //  [ExportAsView(ManagerName.UserOnlineView, Category = ManagerName.CategoryName,
    //MenuName = ManagerName.UserMangeMenuName, MenuTitle = "MANAGER_UserOnline",
    //ToolTip = "Click to view some text.", Url = "/UserOnlineView", Order = 7)]
    //  [ExportViewToRegion(ManagerName.UserOnlineView, ManagerName.ManagerContainer)]
    public partial class UserOnlineView : UserControl
    {
        public UserOnlineView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(LogDataGrid);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SLExtend.ExportExcel(LogDataGrid);
        }
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0e27053a-0070-4d6e-911d-cb63730c5b08      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: LoginLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:24:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:24:04
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    //[ExportAsView(ManagerName.LoginLogView, Category = ManagerName.CategoryName,
    //    MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_LoginLog",
    //    ToolTip = "Click to view some text.", Url = "/LoginLogView", Order = 5)]
    //[ExportViewToRegion(ManagerName.LoginLogView, ManagerName.ManagerContainer)]
    public partial class LoginLogView : UserControl
    {
        public LoginLogView()
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

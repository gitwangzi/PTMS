/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 975c3a1f-2202-482d-a519-941e6b663a88      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: DeviceAlertLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:23:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:23:21
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
    [ExportAsView(ManagerName.DeviceAlertLogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_DeviceAlertLog",
        ToolTip = "Click to view some text.", Url = "/DeviceAlertLogView", Order = 3)]
    [ExportViewToRegion(ManagerName.DeviceAlertLogView, ManagerName.ManagerContainer)]
    public partial class DeviceAlertLogView : UserControl
    {
        public DeviceAlertLogView()
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

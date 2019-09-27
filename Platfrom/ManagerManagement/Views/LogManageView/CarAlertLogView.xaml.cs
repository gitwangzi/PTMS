/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3a89c780-be03-40b5-8e52-d2347df335e3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: CarAlertLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 15:33:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 15:33:10
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    [ExportAsView(ManagerName.CarAlertLogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_CarAlertLog",
        ToolTip = "Click to view some text.", Url = "/CarAlertLogView", Order = 2)]
    [ExportViewToRegion(ManagerName.CarAlertLogView, ManagerName.ManagerContainer)]
    public partial class CarAlertLogView : UserControl
    {
        public CarAlertLogView()
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

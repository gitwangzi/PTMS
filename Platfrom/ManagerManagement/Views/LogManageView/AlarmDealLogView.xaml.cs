/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 33110b5a-7309-4eb8-91cd-c8c5e56131d9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: AlarmDealLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:20:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:20:58
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    [ExportAsView(ManagerName.AlarmDealLogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_AlarmDealLog",
        ToolTip = "Click to view some text.", Url = "/AlarmDealLogView", Order = 1)]
    [ExportViewToRegion(ManagerName.AlarmDealLogView, ManagerName.ManagerContainer)]
    public partial class AlarmDealLogView : UserControl
    {
        public AlarmDealLogView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(LogDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SLExtend.ExportExcel(LogDataGrid);
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

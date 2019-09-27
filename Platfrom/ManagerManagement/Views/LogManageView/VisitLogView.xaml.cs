
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b17e2f9a-ac9c-4566-b2b9-aa4647ead4fd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: VisitLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:26:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:26:35
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
    [ExportAsView(ManagerName.VisitLogView, Category = ManagerName.CategoryName,
      MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_VisitLog",
      ToolTip = "Click to view some text.", Url = "/VisitLogView", Order = 9)]
    [ExportViewToRegion(ManagerName.VisitLogView, ManagerName.ManagerContainer)]
    public partial class VisitLogView : UserControl
    {
        public VisitLogView()
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

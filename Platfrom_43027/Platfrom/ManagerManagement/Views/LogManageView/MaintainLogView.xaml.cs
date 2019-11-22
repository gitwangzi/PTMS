using Jounce.Core.View;
using Jounce.Regions.Core;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c16aebfe-b681-492e-8b24-efea62bad2b7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: MaintainLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:24:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:24:25
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

namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    //[ExportAsView(ManagerName.MaintainLogView, Category = ManagerName.CategoryName,
    //    MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_MaintainLog",
    //    ToolTip = "Click to view some text.", Url = "/MaintainLogView", Order = 6)]
    //[ExportViewToRegion(ManagerName.MaintainLogView, ManagerName.ManagerContainer)]
    public partial class MaintainLogView : UserControl
    {
        public MaintainLogView()
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

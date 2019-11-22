/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 28bb29ec-bb09-4522-be26-60324f98b0c1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: SuiteStatusChangeLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/20 13:46:32
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/20 13:46:32
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
using Gsafety.PTMS.Manager;
using Jounce.Regions.Core;


namespace Gsafety.PTMS.Manager.Views.LogManageView
{
    [ExportAsView(ManagerName.SuiteStatusChangeLogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "StatusChangeLog",
        ToolTip = "Click to view some text.", Url = "/SuiteStatusChangeLogView", Order = 10)]
    [ExportViewToRegion(ManagerName.SuiteStatusChangeLogView, ManagerName.ManagerContainer)]
    public partial class SuiteStatusChangeLogView : UserControl
    {
        public SuiteStatusChangeLogView()
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

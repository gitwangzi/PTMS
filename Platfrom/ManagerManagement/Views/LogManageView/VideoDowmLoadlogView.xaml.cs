/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ef102161-2018-428b-863c-ad1a30a50136      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: VideoDowmLoadlogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:25:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:25:19
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
    [ExportAsView(ManagerName.VideoDowmLoadlogView, Category = ManagerName.CategoryName,
        MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_VideoDowmLoadlog",
        ToolTip = "Click to view some text.", Url = "/VideoDowmLoadlogView", Order = 7)]
    [ExportViewToRegion(ManagerName.VideoDowmLoadlogView, ManagerName.ManagerContainer)]
    public partial class VideoDowmLoadlogView : UserControl
    {
        public VideoDowmLoadlogView()
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

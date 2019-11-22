/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 291d3832-9edb-4141-9e35-e06a266ca3a9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: VideoLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:25:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:25:48
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
    [ExportAsView(ManagerName.VideoLogView, Category = ManagerName.CategoryName,
      MenuName = ManagerName.LogManageMenuName, MenuTitle = "MANAGER_VideoLog",
      ToolTip = "Click to view some text.", Url = "/VideoLogView", Order = 8)]
    [ExportViewToRegion(ManagerName.VideoLogView, ManagerName.ManagerContainer)]
    public partial class VideoLogView : UserControl
    {
        public VideoLogView()
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

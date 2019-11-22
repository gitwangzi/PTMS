/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 79ae7535-20df-4d29-96d5-fe1bded1cbac      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: InstallLogView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 14:23:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 14:23:44
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

    public partial class InstallLogView : UserControl
    {
        public InstallLogView()
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

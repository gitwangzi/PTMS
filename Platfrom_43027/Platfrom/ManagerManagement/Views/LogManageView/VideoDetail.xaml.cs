/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1e27053a-0070-4d6e-911d-cb63730c5b08      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Views.LogManageView
/////    Project Description:    
/////             Class Name: SLExtend
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/13 12:24:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/13 12:24:04
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

namespace Gsafety.PTMS.Manager.Views
{
    [ExportAsView(ManagerName.VideoDetailV)]
    [ExportViewToRegion(ManagerName.VideoDetailV, ManagerName.ManagerContainer)]
    public partial class VideoDetail : UserControl
    {
        public VideoDetail()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

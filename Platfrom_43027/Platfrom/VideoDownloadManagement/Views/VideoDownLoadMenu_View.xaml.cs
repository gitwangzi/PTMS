/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 10bbfd30-b24d-48c7-9f19-6955d12e8c2b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoDownloadManagement.Views
/////    Project Description:    
/////             Class Name: VideoDownLoadMenu_View
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-17 17:10:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-17 17:10:13
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
using Gsafety.PTMS.VideoDownloadManagement.ViewModels;
using Jounce.Core.View;

namespace Gsafety.PTMS.VideoDownloadManagement.Views
{
    public partial class VideoDownLoadMenu_View : UserControl
    {
        public VideoDownLoadMenu_View()
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

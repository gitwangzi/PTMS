/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6353f54d-b31d-43ec-9059-95576954a38c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoDownloadManagement
/////    Project Description:    
/////             Class Name: VideoDownloadBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-16 09:27:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-16 09:27:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.VideoDownloadManagement.ViewModels;
using Gsafety.PTMS.VideoDownloadManagement.Views;
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.VideoDownloadManagement
{
    public class VideoDownloadBinding
    {
        [Export]
        public ViewModelRoute BindingMenu
        {
            get
            {
                return ViewModelRoute.Create(VideoDownloadName.VideoDownLoadMenuVm, VideoDownloadName.VideoDownLoadMenuView);
            }
        }

        [Export]
        public ViewModelRoute BindingMainPage
        {
            get
            {
                return ViewModelRoute.Create(VideoDownloadName.VideoDownLoadMainVm, VideoDownloadName.VideoDownLoadMainView);
            }
        }

        [Export]
        public ViewModelRoute BindingVideoDownload
        {
            get
            {
                return ViewModelRoute.Create(VideoDownloadName.VideoDownLoadVm, VideoDownloadName.VideoDownLoadV);
            }
        }
    }
}

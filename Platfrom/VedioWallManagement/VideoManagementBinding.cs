/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3bbcbc6c-cd8a-4f24-94f4-a5af8ea5d37f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VedioWall
/////    Project Description:    
/////             Class Name: VedioWallBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 9/11/2013 4:40:39 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/11/2013 4:40:39 PM
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
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.VedioWall
{
    public class VideoManagementBinding
    {
        [Export]
        public ViewModelRoute BindingMainPage
        {
            get
            {
                return ViewModelRoute.Create(VideoManagementName.VedioWallMainPageVm, VideoManagementName.VedioWallMainPageV);
            }
        }

        [Export]
        public ViewModelRoute BindingMenu
        {
            get { return ViewModelRoute.Create(VideoManagementName.VedioWallMenuVm, VideoManagementName.VedioWallMenuV); }
        }
    }
}

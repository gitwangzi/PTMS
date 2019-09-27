//#define debug
//#define test
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2197ddfa-71e0-4394-9395-26a3b3fcd005      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoDownloadManagement.ViewModels
/////    Project Description:    
/////             Class Name: VideoDownloadVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-16 09:55:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-16 09:55:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Jounce.Framework.Command;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using System.Collections.ObjectModel;
using Jounce.Core.ViewModel;
using System.Windows.Threading;
using System.Threading;
using Jounce.Framework.ViewModel;
using Jounce.Framework;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Bases.Models;
using System.ComponentModel.Composition;
using Gsafety.PTMS.ServiceReference.MessageService;
using Jounce.Core.Event;
using Jounce.Core.View;

namespace Gsafety.PTMS.VideoDownloadManagement.ViewModels
{
    [ExportAsViewModel(VideoDownloadName.VideoDownLoadMainVm)]
    public class VideoDownLoadMainVm : BaseEntityViewModel
    {
        public VideoDownLoadMainVm()
        {
        }

        //protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        //{
        //    base.ActivateView(viewName, viewParameters);
        //    EventAggregator.Publish(VideoDownloadName.VideoDownLoadMenuView.AsViewNavigationArgs());
        //}
    }

}

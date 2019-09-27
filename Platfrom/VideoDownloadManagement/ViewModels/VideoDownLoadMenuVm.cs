/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ce850d01-9655-43b4-91c8-66851026ec9d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoDownloadManagement.ViewModels
/////    Project Description:    
/////             Class Name: VideoDownLoadMenuVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-17 17:29:59
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-17 17:29:59
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;

namespace Gsafety.PTMS.VideoDownloadManagement.ViewModels
{
    [ExportAsViewModel(VideoDownloadName.VideoDownLoadMenuVm)]
    public class VideoDownLoadMenuVm : BaseViewModel
    {
        #region Attribute

        public Visibility MenuShow
        {
            get
            {
                //if (ApplicationContext.Instance.AuthenticationInfo.GroupName.Equals("AlarmFilterCommissioner"))
                //{
                //    return Visibility.Collapsed;
                //}
                return Visibility.Visible;
            }
        }

        public ICommand FristVideoCommand { get; private set; }
        public ICommand SecondVideoCommand { get; private set; }

        #endregion

        public VideoDownLoadMenuVm()
            : base()
        {
            FristVideoCommand = new ActionCommand<object>((obj) => FristVideo_Event(obj));
            SecondVideoCommand = new ActionCommand<object>((obj) => SecondVideo_Event(obj));
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
        }

        private void FristVideo_Event(object vehicle)
        {

        }

        private void SecondVideo_Event(object vehicle)
        {

        }

        private void VideoPaly(Vehicle vehicleInfo, int channel)
        {

        }
    }
}

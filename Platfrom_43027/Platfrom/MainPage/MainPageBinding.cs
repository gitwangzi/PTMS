using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c81d0840-e242-4ca5-898f-da2b34caa247      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage
/////    Project Description:    
/////             Class Name: MainPageBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 8/5/2013 1:23:47 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/5/2013 1:23:47 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.MainPage
{
    public class MainPageBinding
    {
        [Export]
        public ViewModelRoute Binding
        {
            get { return ViewModelRoute.Create(MainPageName.CentralPlatformVm, MainPageName.CentralPlatformV); }
        }

        [Export]
        public ViewModelRoute BingingMessageNotify
        {
            get { return ViewModelRoute.Create(MainPageName.MessageNotifitionVm, MainPageName.MessageNotifition); }
        }
        [Export]
        public ViewModelRoute BindingInstallPlatformV
        {
            get { return ViewModelRoute.Create(MainPageName.InstallPlatformVm, MainPageName.InstallPlatformV); }
        }
        [Export]
        public ViewModelRoute BindingMaintainPlatorm
        {
            get { return ViewModelRoute.Create(MainPageName.MaintainPlatformVm, MainPageName.MaintainPlatformV); }
        }

        [Export]
        public ViewModelRoute BindingSuperPlatform
        {
            get { return ViewModelRoute.Create(MainPageName.SuperPlatformVm, MainPageName.SuperPlatformV); }
        }

        [Export]
        public ViewModelRoute BindingAntProductNativeMenu
        {
            get
            {
                return ViewModelRoute.Create(MainPageName.AntProductNativeMenuVm, MainPageName.AntProductNativeMenuV);
            }
        }
        [Export]
        public ViewModelRoute BindingOrderClientInfo
        {
            get { return ViewModelRoute.Create(MainPageName.OrderClientPlatformVm, MainPageName.OrderClientPlatformV); }
        }
        [Export]
        public ViewModelRoute BindingCenterPlatorm
        {
            get { return ViewModelRoute.Create(MainPageName.CentralPlatformVm2, MainPageName.CentralPlatformV2); }
        }

        [Export]
        public ViewModelRoute BindingUserDetailInfoWindow
        {
            get { return ViewModelRoute.Create(MainPageName.UserDetailInfoWindowVm, MainPageName.UserDetailInfoWindow); }
        }

        

    }
}

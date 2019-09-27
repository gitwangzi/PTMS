/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d250a255-2536-4b04-bc8a-3b752c344099      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.ViewModels
/////    Project Description:    
/////             Class Name: MaintainPlatformVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 11:13:42
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 11:13:42
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.MainPage.Views;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Core.Command;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.MaintainPlatformVm)]
    public class MaintainPlatformVm : BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; private set; }
        public IActionCommand UserInformationCommmand { get; private set; }

        public string UserName
        {
            get { return ApplicationContext.Instance.AuthenticationInfo.UserName; }
        }
        public MaintainPlatformVm()
        {
            ChangePasswordCommand = new ActionCommand<object>(obj => ChangePassword_Event());
            UserInformationCommmand = new ActionCommand<object>(obj => UserInformation_Evnet());
            SubscriptionMessage();
        }

        private void ChangePassword_Event()
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void UserInformation_Evnet()
        {
            UserInformation userInfomation = new UserInformation();
            userInfomation.Show();

        }

        private void SubscriptionMessage()
        {
            //ApplicationContext.Instance.MessageManager.GetSuiteRunintStatusMessage();
            //ApplicationContext.Instance.MessageManager.GetDeleteUserMessage();
            //ApplicationContext.Instance.MessageManager.GetChangeUserMessage();
        }
    }
}

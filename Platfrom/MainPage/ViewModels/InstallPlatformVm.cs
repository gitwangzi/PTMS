
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4c2081e5-d9e1-470f-8469-2035fe8a4139      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.ViewModels
/////    Project Description:    
/////             Class Name: InstallPlatformVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 10:15:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 10:15:25
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
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Windows.Browser;
using Gsafety.Common.Controls;
using Gsafety.Ant.MainPage.Views;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.InstallPlatformVm)]
    public class InstallPlatformVm : BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; private set; }
        public IActionCommand UserInformationCommmand { get; private set; }
        public IActionCommand ExitCommand { get; private set; }

        public string UserName
        {
            get { return ApplicationContext.Instance.AuthenticationInfo.UserName; }
        }
        public InstallPlatformVm()
        {
            ChangePasswordCommand = new ActionCommand<object>(obj => ChangePassword_Event());
            UserInformationCommmand = new ActionCommand<object>(obj => UserInformation_Evnet());
            ExitCommand = new ActionCommand<object>(obj => Exit_Event());
            SubscriptionMessage();
        }

        private void ChangePassword_Event()
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void UserInformation_Evnet()
        {
            UserDetailInfoWindow userInfomation = new UserDetailInfoWindow();
            userInfomation.Show();

        }

        private void Exit_Event()
        {
            //if (MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAceptar"), ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAPrompt"), MessageDialogButton.OkAndCancel).DialogResult == true)
            //{
            //    HtmlPage.Window.Eval("window.location.reload();");
            //}
            var window = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAPrompt"), ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAceptar"), MessageDialogButton.OkAndCancel);
            window.Closed += closeWindow_Closed;
        }

        void closeWindow_Closed(object sender, EventArgs e)
        {
            var window = sender as ChildWindow;
            if (window.DialogResult == true)
            {
                HtmlPage.Window.Eval("window.location.reload();");
                HtmlPage.Window.Invoke("CloseShell");
            }
        }
        private void SubscriptionMessage()
        {
            //ApplicationContext.Instance.MessageManager.GetInspectMessage();
            //ApplicationContext.Instance.MessageManager.GetDeleteUserMessage();
            //ApplicationContext.Instance.MessageManager.GetChangeUserMessage();
        }

    }
}

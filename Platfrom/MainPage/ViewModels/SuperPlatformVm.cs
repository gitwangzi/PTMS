using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Browser;
using Gsafety.Ant.MainPage.Views;
using Gsafety.PTMS.MainPage.Views;
using Gsafety.Common.Controls;
using System;
using System.Windows.Controls;

namespace Gsafety.Ant.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.SuperPlatformVm)]
    public class SuperPlatformVm : PTMSBaseViewModel, IPartImportsSatisfiedNotification, IEventSink<RoleArgs>
    {
        public IActionCommand ChangePasswordCommand { get; private set; }

        SuperPlatformVm()
        {
            _rolefucs = "00-00";
            ExitCommand = new ActionCommand<object>(ojb => Exit_Command());
            UserInformationCommmand = new ActionCommand<object>(obj => UserInformationCommand());
            ChangePasswordCommand = new ActionCommand<object>(obj => ChangePassword_Event());
        }

        public IActionCommand UserInformationCommmand { get; private set; }
        public IActionCommand ExitCommand { get; private set; }
        ObservableCollection<MenuInfo> _orderclientmenus = new ObservableCollection<MenuInfo>();

        public ObservableCollection<MenuInfo> OrderClientMenus
        {
            get
            {
                return _orderclientmenus;
            }
        }

        private void Exit_Command()
        {
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

        private void UserInformationCommand()
        {
            UserDetailInfoWindow userDetailInfo = new UserDetailInfoWindow();
            userDetailInfo.Show();
        }

        private void ChangePassword_Event()
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }


        ObservableCollection<MenuInfo> _logmenus = new ObservableCollection<MenuInfo>();

        public ObservableCollection<MenuInfo> LogMenus
        {
            get
            {
                return _logmenus;
            }
        }



        public System.Windows.Visibility ClientVisibility
        {
            get;
            set;
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<RoleArgs>(this);
        }

        public void HandleEvent(RoleArgs publishedEvent)
        {
            OnRoleEvent(publishedEvent.FuncItems);
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            //object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(MainPageName.SuperPlatformV);
            //Frame frame = (mview as UserControl).FindName("ContentFrame") as Frame;
            //if (Super.CurrentSource == null)
            //    return;
            //frame.Refresh();
        }


        protected override void OnInitialUI(ObservableCollection<string> FuncItems)
        {
            //foreach (var item in FuncItems)
            //{
            //    if (item.Contains("00-00-00"))
            //    {
            ClientVisibility = Visibility.Visible;

            //MenuInfo mi = new MenuInfo("OrderClientManagement", "客户管理", "/OrderClientManageV");

            //_orderclientmenus.Add(mi);

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => OrderClientMenus);
            });

            //break;
            //    }
            //}
        }
    }
}

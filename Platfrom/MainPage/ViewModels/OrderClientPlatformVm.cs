using BaseLib.ViewModels;
using Gsafety.Ant.MainPage.Views;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.MainPage.Views;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace Gsafety.Ant.MainPage.ViewModels
{
    [ExportAsViewModel(MainPageName.OrderClientPlatformVm)]
    public class OrderClientPlatformVm : PTMSBaseViewModel, IPartImportsSatisfiedNotification, IEventSink<RoleArgs>
    {
        public OrderClientPlatformVm()
        {
            _rolefucs = "01-00";
            ExitCommand = new ActionCommand<object>(ojb => Exit_Command());
            ChangePasswordCommand = new ActionCommand<object>(ojb => ChangePassword());
            UserInformationCommmand = new ActionCommand<object>(ojb => UserInformation());
        }

        ObservableCollection<MenuInfo> _orderclientmenus = new ObservableCollection<MenuInfo>();

        public IActionCommand ExitCommand { get; private set; }
        public IActionCommand ChangePasswordCommand { get; private set; }
        public IActionCommand UserInformationCommmand { get; private set; }

        private void Exit_Command()
        {
            //if (MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAceptar"), ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_ExitAPrompt"), MessageBoxButton.OKCancel) == MessageBoxResult.OK)
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

        private void ChangePassword()
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void UserInformation()
        {
            UserDetailInfoWindow userDetailInfo = new UserDetailInfoWindow();
            userDetailInfo.Show();
        }

        public ObservableCollection<MenuInfo> OrderClientMenus
        {
            get
            {
                return _orderclientmenus;
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

        protected override void OnInitialUI(System.Collections.ObjectModel.ObservableCollection<string> FuncItems)
        {
            try
            {
                foreach (var item in FuncItems)
                {
                    if (item.Contains("01-00-00"))
                    {
                        ClientVisibility = Visibility.Visible;

                        MenuInfo mi = new MenuInfo("OrderClientInfo", "客户信息", "/OrderClientInfoV");

                        _orderclientmenus.Add(mi);

                        Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                        {
                            RaisePropertyChanged(() => OrderClientMenus);
                        });

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}

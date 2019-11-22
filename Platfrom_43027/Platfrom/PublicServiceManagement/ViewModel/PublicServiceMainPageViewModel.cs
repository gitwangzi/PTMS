using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using PublicServiceManagement;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using Jounce.Framework;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(PublicServiceName.PublicServiceMainPageVm)]
    public class PublicServiceMainPageViewModel : PTMSBaseViewModel, IPartImportsSatisfiedNotification, IEventSink<RoleArgs>
    {
        public PublicServiceMainPageViewModel()
        {

        }

        public void HandleEvent(RoleArgs publishedEvent)
        {
            OnRoleEvent(publishedEvent.FuncItems);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<RoleArgs>(this);
        }

        #region 属性

        ObservableCollection<MenuInfo> _messageMenus = new ObservableCollection<MenuInfo>();

        public ObservableCollection<MenuInfo> MessageMenus
        {
            get
            {
                return _messageMenus;
            }
        }


        ObservableCollection<MenuInfo> _registryMenus = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 运营管理
        /// </summary>
        public ObservableCollection<MenuInfo> RegistryMenus
        {
            get { return this._registryMenus; }
        }

        public System.Windows.Visibility LogVisibility
        {
            get;
            set;
        }

        public System.Windows.Visibility ClientVisibility
        {
            get;
            set;
        }


        #endregion


        protected override void OnInitialUI(ObservableCollection<string> FuncItems)
        {
            //"套件消息"
            //"手机消息"
            try
            {
                var mdvrMsg = new MenuInfo("mdvrMsg", ApplicationContext.Instance.StringResourceReader.GetString("SafeDeviceMessage"), "/" + PublicServiceName.MdvrMsgManageV);
                var phoneMsg = new MenuInfo("phoneMsg", ApplicationContext.Instance.StringResourceReader.GetString("PhoneMessage"), "/" + PublicServiceName.PhoneMsgManageV);

                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-07-01-01"))
                {
                    this._messageMenus.Add(mdvrMsg);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-07-01-02"))
                {
                    this._messageMenus.Add(phoneMsg);
                }


                var lostRegistry = new MenuInfo("LostRegistry", ApplicationContext.Instance.StringResourceReader.GetString("LostRegister"), "/" + PublicServiceName.LostRegistryManageV);
                var foundRegistry = new MenuInfo("FoundRegistry", ApplicationContext.Instance.StringResourceReader.GetString("GetRegister"), "/" + PublicServiceName.FoundRegistryManageV);

                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-07-02-01"))
                {
                    this._registryMenus.Add(lostRegistry);
                }
                if (ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-07-02-02"))
                {
                    this._registryMenus.Add(foundRegistry);
                }

                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => MessageMenus);
                });
                Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
                {
                    RaisePropertyChanged(() => this.RegistryMenus);
                });
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}

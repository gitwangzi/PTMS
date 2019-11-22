using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;

namespace Gsafety.PTMS.Manager.ViewModels
{
    /// <summary>
    /// ANT产品系统管理主界面
    /// </summary>
    [ExportAsViewModel(ManagerName.AntProductManagerMainPageVm)]
    public class AntProductManagerMainPageViewModel : PTMSBaseViewModel, IPartImportsSatisfiedNotification, IEventSink<RoleArgs>
    {
        public AntProductManagerMainPageViewModel()
        {

        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            //object mview = ApplicationContext.Instance.MenuManager.Router.ViewQuery(ManagerName.AntProductManagerMainPageV);
            //Frame frame = (mview as UserControl).FindName("SystemManageContentFrame") as Frame;

            //if (frame.CurrentSource == null)
            //{
            //    frame.Source = new System.Uri("/AntProductRoleManageV", System.UriKind.RelativeOrAbsolute);
            //}
            //frame.Refresh();
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

        ObservableCollection<MenuInfo> userManageMenus = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 用户管理菜单
        /// </summary>
        public ObservableCollection<MenuInfo> UserManageMenus
        {
            get
            {
                return userManageMenus;
            }
        }


        ObservableCollection<MenuInfo> roleManageMenus = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 角色管理菜单项
        /// </summary>
        public ObservableCollection<MenuInfo> RoleManageMenus
        {
            get { return this.roleManageMenus; }
        }



        ObservableCollection<MenuInfo> departmentMenus = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 组织机构菜单项
        /// </summary>
        public ObservableCollection<MenuInfo> DepartmentMenus
        {
            get
            {
                return departmentMenus;
            }
        }

        ObservableCollection<MenuInfo> configureMenu = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 配置菜单
        /// </summary>
        public ObservableCollection<MenuInfo> ConfigureMenu
        {
            get { return this.configureMenu; }
        }

        ObservableCollection<MenuInfo> commandMenu = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 命令菜单
        /// </summary>
        public ObservableCollection<MenuInfo> CommandMenu
        {
            get { return this.commandMenu; }
        }

        ObservableCollection<MenuInfo> systemLogMenu = new ObservableCollection<MenuInfo>();
        /// <summary>
        /// 系统日志菜单
        /// </summary>
        public ObservableCollection<MenuInfo> SystemLogMenu
        {
            get { return this.systemLogMenu; }
        }


        public Visibility LogVisibility
        {
            get;
            set;
        }

        public Visibility ClientVisibility
        {
            get;
            set;
        }


        #endregion

        #region 导航....
        private static string UserOnlineInf = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserOnline");
        private static string RoleManage = ApplicationContext.Instance.StringResourceReader.GetString("RoleManager");
        private static string UserOrganization = ApplicationContext.Instance.StringResourceReader.GetString("UserOrganization");
        private static string VehicleOrganizationManage = ApplicationContext.Instance.StringResourceReader.GetString("VehicleOrganizationManage");
        private static string MANAGE_Parm = ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Parm");
        private static string ExternalOnlineTime = ApplicationContext.Instance.StringResourceReader.GetString("ExternalOnlineTime");
        private static string SetMessage = ApplicationContext.Instance.StringResourceReader.GetString("SetMessage");
        private static string SetColor = ApplicationContext.Instance.StringResourceReader.GetString("SetColor");
        private static string SetSound = ApplicationContext.Instance.StringResourceReader.GetString("SetSound");
        private static string VehicleSetAlarm = ApplicationContext.Instance.StringResourceReader.GetString("VehicleSetAlarm");
        private static string DeviceSetAlarm = ApplicationContext.Instance.StringResourceReader.GetString("DeviceSetAlarm");

        private static string SetGpsRule = ApplicationContext.Instance.StringResourceReader.GetString("SetGpsRule");
        private static string SpeedSetRule = ApplicationContext.Instance.StringResourceReader.GetString("SpeedSetRule");
        private static string IPSetParma = ApplicationContext.Instance.StringResourceReader.GetString("IPSetParma");
        private static string Setheartbeat = ApplicationContext.Instance.StringResourceReader.GetString("Setheartbeat");
        private static string SetLEDParm = ApplicationContext.Instance.StringResourceReader.GetString("SetLEDParm");
        private static string SetQueryRule = ApplicationContext.Instance.StringResourceReader.GetString("SetQueryRule");

        private static string MANAGER_AlarmDealLog = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_AlarmDealLog");
        private static string MANAGER_CarAlertLog = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_CarAlertLog");
        private static string DeviceDisposeLog = ApplicationContext.Instance.StringResourceReader.GetString("DeviceDisposeLog");
        private static string InstallLog = ApplicationContext.Instance.StringResourceReader.GetString("InstallLog");
        private static string MANAGER_LoginLog = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_LoginLog");
        private static string MANAGER_VideoDowmLoadlog = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_VideoDowmLoadlog");
        private static string LocalDownLoadLog = ApplicationContext.Instance.StringResourceReader.GetString("LocalDownLoadLog");
        private static string VideoRunLog = ApplicationContext.Instance.StringResourceReader.GetString("VideoRunLog");
        private static string UserManagerLog = ApplicationContext.Instance.StringResourceReader.GetString("UserManagerLog");
        private static string RoleManagerLog = ApplicationContext.Instance.StringResourceReader.GetString("RoleManagerLog");
        private static string Log911 = ApplicationContext.Instance.StringResourceReader.GetString("Log911");
        private static string SysLoginLog = ApplicationContext.Instance.StringResourceReader.GetString("SysLoginLog");
        private static string VideoLog = ApplicationContext.Instance.StringResourceReader.GetString("VideoLog");
        private static string VisitLog = ApplicationContext.Instance.StringResourceReader.GetString("VisitLog");
        private static string BSC_VehicleType = ApplicationContext.Instance.StringResourceReader.GetString("VehicleType");
        private static string MANAGER_ErrorLog = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ErrorLog");
        #endregion

        protected override void OnInitialUI(ObservableCollection<string> FuncItems)
        {

            this.ClientVisibility = Visibility.Visible;
            //用户在线情况管理
            var menuUserOnlineInfo = new MenuInfo("ManagerManagement", UserOnlineInf, "/UserOnlineInfoV");


            //角色角色管理
            var menuRoleManage = new MenuInfo("ManagerManagement", RoleManage, "/AntProductRoleManageV");


            //用户管理
            var menuUserDepartment = new MenuInfo("ManagerManagement", UserOrganization, "/AntProductUserDepartmentV");

            //userManageMenus.Add(menuUserDepartment);
            //userManageMenus.Add(menuUserOnlineInfo);

            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-01-01"))
            {
                userManageMenus.Add(menuRoleManage);
            }
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-01-02"))
            {
                userManageMenus.Add(menuUserDepartment);
            }

            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-01-03"))
            {
                userManageMenus.Add(menuUserOnlineInfo);
            }


            //配置菜单
            //var menuParameterSetting = new MenuInfo("ManagerManagement", MANAGE_Parm, "/AntProductParameterSettingV");
            //var menuDeviceOutlineTimeSetting = new MenuInfo("ManagerManagement", ExternalOnlineTime, "/AntProductDeviceOutlineTimeSettingV");
            //var menuDeviceWarrantyNotifySetting = new MenuInfo("ManagerManagement", SetMessage, "/AntProductDeviceWarrantyNotifySettingV");
            //var menuAlarmTypeColorSetting = new MenuInfo("ManagerManagement", SetColor, "/AntProductAlarmTypeColorSettingV");
            //var menuAlarmTypeSoundSetting = new MenuInfo("ManagerManagement", SetSound, "/AntProductAlarmTypeSoundSettingV");

            //var menuVehicleAlarmSetting = new MenuInfo("ManagerManagement", VehicleSetAlarm, "/AntProductVehicleAlarmSettingV");
            //var menuDeviceAlarmSetting = new MenuInfo("ManagerManagement", DeviceSetAlarm, "/AntProductDeviceAlarmSettingV");
            var menuVehicleType = new MenuInfo("ManagerManagement", BSC_VehicleType, "/VehicleTypeManageViewV");

            //车辆类别
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-02-01"))
            {
                this.configureMenu.Add(menuVehicleType);

            }


            //this.configureMenu.Add(menuParameterSetting);
            //this.configureMenu.Add(menuDeviceOutlineTimeSetting);
            //this.configureMenu.Add(menuDeviceWarrantyNotifySetting);
            //this.configureMenu.Add(menuAlarmTypeColorSetting);
            //this.configureMenu.Add(menuAlarmTypeSoundSetting);
            //this.configureMenu.Add(menuVehicleAlarmSetting);
            //this.configureMenu.Add(menuDeviceAlarmSetting);

            //命令菜单
            var menuGpsConfigure = new MenuInfo("ManagerManagement", SetGpsRule, "/GPSConfigureV");
            var menuSpeedRoleConfigure = new MenuInfo("ManagerManagement", SpeedSetRule, "/SpeedRuleParameterSettingV");
            //var menuIpParameterConfigure = new MenuInfo("ManagerManagement", IPSetParma, "/IpParameterSettingV");
            var menuHeartBeatRoleConfigure = new MenuInfo("ManagerManagement", Setheartbeat, "/SetHeartBeatRuleV");
            //var menuLedScrrenParameterConfigure = new MenuInfo("ManagerManagement", SetLEDParm, "/SetLEDScrrenRuleV");
            //var menuLedScreenMessageParameterConfigure = new MenuInfo("ManagerManagement", "设置LED屏幕消息参数", "/SetLEDScreenMessageParameterV");
            //var menuSearchRoleConfigure = new MenuInfo("ManagerManagement", SetQueryRule, "/SearchRoleConfigurationV");
            //定位规则
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-03-01"))
            {
                this.commandMenu.Add(menuGpsConfigure);
            }
            //心跳
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-03-03"))
            {
                this.commandMenu.Add(menuHeartBeatRoleConfigure);
            }
            //this.commandMenu.Add(menuLedScrrenParameterConfigure);
            //this.commandMenu.Add(menuLedScreenMessageParameterConfigure);
            //this.commandMenu.Add(menuSearchRoleConfigure);

            //系统日志
            var menuAlarmDisposedlog = new MenuInfo("ManagerManagement", MANAGER_AlarmDealLog, "/AlarmDisposedLogV");
            var menuVehicleAlarmDisposedLog = new MenuInfo("ManagerManagement", MANAGER_CarAlertLog, "/AlertDisposeLogV");
            //var menuDeviceAlarmDisposedLog = new MenuInfo("ManagerManagement", DeviceDisposeLog, "/AntProductDeviceAlarmDisposedLogV");
            var menuInstallLog = new MenuInfo("ManagerManagement", InstallLog, "/InstallLogV");
            //var menuSystemLoginLog = new MenuInfo("ManagerManagement", MANAGER_LoginLog, "/AntProductSystemLoginLogV");
            var menuVedioDownloadLog = new MenuInfo("ManagerManagement", MANAGER_VideoDowmLoadlog, "/VideoDownloadLogV");
            //var menuLocalVedioDownloadLog = new MenuInfo("ManagerManagement", LocalDownLoadLog, "/AntProductLocalVedioDownloadLogV");
            var menuVedioPlayLog = new MenuInfo("ManagerManagement", VideoRunLog, "/VideoPlayLogV");
            var menuUserManageLog = new MenuInfo("ManagerManagement", UserManagerLog, "/OperateLogV");
            //var menuRoleManageLog = new MenuInfo("ManagerManagement", RoleManagerLog, "/AntProductRoleManageLogV");
            //var menuAssist911UserLog = new MenuInfo("ManagerManagement", Log911, "/AntProductAssist911UserLogV");
            var systemLoginLogView = new MenuInfo("ManagerManagement", SysLoginLog, "/SystemLoginLogView");
            var systemErrorLog = new MenuInfo("ManagerManagement", MANAGER_ErrorLog, "/ErrorLogV");
            //var videoLog = new MenuInfo("ManagerManagement", VideoLog, "/VideoLogV");
            //var visitLog = new MenuInfo("ManagerManagement", VisitLog, "/VisitLogV");
            //报警处理日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-01"))
            {
                this.systemLogMenu.Add(menuAlarmDisposedlog);
            }
            //车辆告警处理日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-02"))
            {

                this.systemLogMenu.Add(menuVehicleAlarmDisposedLog);
            }
            //安装日志日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-03"))
            {
                this.systemLogMenu.Add(menuInstallLog);
            }
            //视频下载日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-04"))
            {
                this.systemLogMenu.Add(menuVedioDownloadLog);
            }
            //播放日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-05"))
            {
                this.systemLogMenu.Add(menuVedioPlayLog);
            }
            //用户管理日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-06"))
            {
                this.systemLogMenu.Add(menuUserManageLog);
            }
            //系统登录日志
            if(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems.Contains("02-08-04-07"))
            {
                this.systemLogMenu.Add(systemLoginLogView);
            }

            //系统错误日志
            this.systemLogMenu.Add(systemErrorLog);

            //this.systemLogMenu.Add(videoLog);
            //this.systemLogMenu.Add(visitLog);
            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.UserManageMenus);
            });

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.RoleManageMenus);
            });

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.DepartmentMenus);
            });

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.ConfigureMenu);
            });

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.CommandMenu);
            });

            Jounce.Framework.JounceHelper.ExecuteOnUI(() =>
            {
                RaisePropertyChanged(() => this.SystemLogMenu);
            });

        }


    }
}

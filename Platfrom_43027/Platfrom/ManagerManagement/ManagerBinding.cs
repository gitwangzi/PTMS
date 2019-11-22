using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9df38ce6-a07e-4cd3-944f-d94f6d36886d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: ManagerBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:24:54 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:24:54 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.ComponentModel.Composition;

namespace Gsafety.PTMS.Manager
{
    public class ManagerBinding
    {
        [Export]
        public ViewModelRoute BingingManagerMainPage
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ManagerMainPageVm, ManagerName.ManagerMainPageV);
            }
        }

        [Export]
        public ViewModelRoute BindingMenu
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ManagerMenuVm, ManagerName.ManagerMenuV);
            }
        }



        [Export]
        public ViewModelRoute SetupStationList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SetupStationListViewModel, ManagerName.SetupStationListView);
            }
        }
        [Export]
        public ViewModelRoute SetupStationUserAdd
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SetupStationUserAddViewModel, ManagerName.SetupStationUserAddView);
            }
        }
        [Export]
        public ViewModelRoute SetupStationUserEdit
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SetupStationUserEditViewModel, ManagerName.SetupStationUserEditView);
            }
        }
        [Export]
        public ViewModelRoute TrafficUserAdd
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.TrafficUserAddViewModel, ManagerName.TrafficUserAddView);
            }
        }
        [Export]
        public ViewModelRoute TrafficUserEdit
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.TrafficUserEditViewModel, ManagerName.TrafficUserEditView);
            }
        }
        [Export]
        public ViewModelRoute UserList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.UserListViewModel, ManagerName.UserListView);
            }
        }
 
        [Export]
        public ViewModelRoute SysMaintainlist
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SysMaintainUserListViewModel, ManagerName.SysMaintainUserListView);
            }
        }
        [Export]
        public ViewModelRoute SysMaintainAdd
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SysMaintainUserAddViewModel, ManagerName.SysMaintainUserAdd);
            }
        }
        [Export]
        public ViewModelRoute SysMaintainEdit
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SysMaintainUserEditViewModel, ManagerName.SysMaintainUserEdit);
            }
        }

        //[Export]
        //public ViewModelRoute UserOnlineBinding
        //{
        //    get
        //    {
        //        return ViewModelRoute.Create(ManagerName.UserOnlineViewModel, ManagerName.UserOnlineView);
        //    }
        //}
        [Export]
        public ViewModelRoute SysManagerUserList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SysManagerUserListViewModel, ManagerName.SysManagerUserListView);
            }
        }

        #region LogManager
        [Export]
        public ViewModelRoute AlramDealList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmDealLogViewModel, ManagerName.AlarmDealLogView);
            }
        }

        [Export]
        public ViewModelRoute CarAlertDealList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.CarAlertLogViewModel, ManagerName.CarAlertLogView);
            }
        }

        [Export]
        public ViewModelRoute DeviceAlertLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.DeviceAlertLogViewModel, ManagerName.DeviceAlertLogView);
            }
        }

        [Export]
        public ViewModelRoute InstallLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.InstallLogVm, ManagerName.InstallLogV);
            }
        }

        [Export]
        public ViewModelRoute SuiteStatusChangeLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SuiteStatusChangeLogViewModel, ManagerName.SuiteStatusChangeLogView);
            }
        }

        //[Export]
        //public ViewModelRoute LoginLogList
        //{
        //    get
        //    {
        //        return ViewModelRoute.Create(ManagerName.LoginLogViewModel, ManagerName.LoginLogView);
        //    }
        //}
        [Export]
        public ViewModelRoute MaintainLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.MaintainLogViewModel, ManagerName.MaintainLogView);
            }
        }

        [Export]
        public ViewModelRoute VideoDowmLoadLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoDowmLoadlogViewModel, ManagerName.VideoDowmLoadlogView);
            }
        }
        [Export]
        public ViewModelRoute VideoDetailLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoDetailVM, ManagerName.VideoDetailV);
            }
        }
        [Export]
        public ViewModelRoute VideoLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoLogViewModel, ManagerName.VideoLogView);
            }
        }
        [Export]
        public ViewModelRoute VisitLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VisitLogViewModel, ManagerName.VisitLogView);
            }
        }
        //登陆日志绑定
        [Export]
        public ViewModelRoute SystemLoginLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SystemLoginLogViewModel, ManagerName.SystemLoginLogView);
            }
        }
        [Export]
        public ViewModelRoute LogDataLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoLogVM, ManagerName.VideoLogV);
            }
        }
        [Export]
        public ViewModelRoute LogManagerLogList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VisitLogVM, ManagerName.VisitLogV);
            }
        }
        #endregion

        #region ConfigManager
        [Export]
        public ViewModelRoute BasicinfoSettingList
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.BasicinfoSettingViewModel, ManagerName.BasicinfoSetting);
            }
        }
        [Export]
        public ViewModelRoute EmailSeting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.EmailTemPlateSettingViewModel, ManagerName.EmailTemPlateSetting);
            }
        }
        [Export]
        public ViewModelRoute AlertColorSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlertTypeColorSetViewModel, ManagerName.AlertTypeColorSet);
            }
        }
        [Export]
        public ViewModelRoute BaseSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.BaseSettingViewModel, ManagerName.BaseSetting);
            }
        }
        #endregion
        #region  AuthorityManager
        [Export]
        public ViewModelRoute BingUserAuthority
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.UserAuthorityViewModel, ManagerName.UserAuthorityView);
            }
        }

        [Export]
        public ViewModelRoute BingUserAuthorityManage
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.UserAuthorityManageViewModel, ManagerName.UserAuthorityManageView);
            }
        }
        #endregion

        #region CommandManage

        [Export]
        public ViewModelRoute GpsSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.GpsSettingViewModel, ManagerName.GpsSettingView);
            }
        }

        [Export]
        public ViewModelRoute GpsSettingAdd
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.GpsSettingAddViewModel, ManagerName.GpsSettingAddView);
            }
        }

        [Export]
        public ViewModelRoute GpsSettingToVehicle
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.GpsSettingToVechileViewModel, ManagerName.GpsSettingToVechileView);
            }
        }

        [Export]
        public ViewModelRoute GpsSettingModify
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.GpsSettingModifyViewModel, ManagerName.GpsSettingModifyView);
            }
        }

        [Export]
        public ViewModelRoute DetailGpsSettings
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.DetailGpsSettingsViewModel, ManagerName.DetailGpsSettingsView);
            }
        }

        [Export]
        public ViewModelRoute ConfigInfo
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ConfigInfoViewModel, ManagerName.ConfigInfoView);
            }
        }

        [Export]
        public ViewModelRoute ConfigRulesDetail
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ConfigRulesDetailViewModel, ManagerName.ConfigRulesDetailView);
            }
        }

        #region TemperatureAlertRule
        [Export]
        public ViewModelRoute TemperatureSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.TemperatureSettingViewModel, ManagerName.TemperatureSettingView);
            }
        }
        [Export]
        public ViewModelRoute TemperatureSettingAdd
        {
            get
            {

                return ViewModelRoute.Create(ManagerName.TemperatureAddRuleViewModel, ManagerName.TemperatureAddRuleView);
            }
        }
        [Export]
        public ViewModelRoute TemperatureDetail
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.TemperatureDetailViewModel, ManagerName.TemperatureDetailView);
            }
        }
        [Export]
        public ViewModelRoute TemperatureRuleUpdate
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.TemperatureUpdateVM, ManagerName.TemperatureUpdateV);
            }
        }
        #endregion

        #region AlarmSetting
        [Export]
        public ViewModelRoute AlarmSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmSettingViewModel, ManagerName.AlarmSettingView);
            }

        }

        [Export]
        public ViewModelRoute AlarmSettingToVehicle
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmSettingToVehicleViewModel, ManagerName.AlarmSettingToVehicleView);
            }

        }

        [Export]
        public ViewModelRoute AlarmSettingAdd
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmSettingAddViewModel, ManagerName.AlarmSettingAddView);
            }
        }

        [Export]
        public ViewModelRoute AlarmSettingModify
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmSettingModifyViewModel, ManagerName.AlarmSettingModifyView);
            }
        }

        [Export]
        public ViewModelRoute DetailAlarmSettings
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.DetailAlarmSettingsViewModel, ManagerName.DetailAlarmSettingsView);
            }
        }
        #endregion

        #region AbnormalDoorRule
        [Export]
        public ViewModelRoute AbnormalDoorSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AbnormalDoorSettingViewModel, ManagerName.AbnormalDoorSettingView);
            }

        }
        [Export]
        public ViewModelRoute AbnormalDoorAddRule
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AbnormalDoorAddRuleViewModel, ManagerName.AbnormalDoorAddRuleView);
            }

        }

        [Export]
        public ViewModelRoute AbnormalDoorDetailView
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AbnormalDoorDetailViewModel, ManagerName.AbnormalDoorDetailView);
            }

        }
        [Export]
        public ViewModelRoute AbnormalDoorUpdate
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AbnormalDoorRuleUpdateVM, ManagerName.AbnormalDoorRuleUpdateV);
            }

        }
        #endregion

        #region GpsSetting
        [Export]
        public ViewModelRoute ModifyGpsSettings
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ModifyGpsSettingsVM, ManagerName.ModifyGpsSettingsView);
            }

        }

        [Export]
        public ViewModelRoute CurrentSettingsRule
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.CurrentSettingRuleVM, ManagerName.CurrentSettingRuleView);
            }
        }

        [Export]
        public ViewModelRoute RuleSettingLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.RuleSettingLogVM, ManagerName.RuleSettingLogView);
            }
        }

        #endregion

        [Export]
        public ViewModelRoute SendInfomation
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SendInfomationVM, ManagerName.SendInfomationView);
            }
        }
        #endregion

        #region Role
        [Export]
        public ViewModelRoute AntProductRoleManage
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductRoleManageVm, ManagerName.AntProductRoleManageV);
            }
        }

        [Export]
        public ViewModelRoute AntProductRoleManageDetail
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductRoleMangeDetailVm, ManagerName.AntProductRoleMangeDetailV);
            }
        }

        [Export]
        public ViewModelRoute AntProductRoleSelect
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductRoleSelectVm, ManagerName.AntProductRoleSelectV);
            }
        }
        #endregion


        [Export]
        public ViewModelRoute AntProductSystemManageMainViewBiding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductManagerMainPageVm, ManagerName.AntProductManagerMainPageV);
            }
        }

        [Export]
        public ViewModelRoute AntProductVehicleDepartmentViewBinding
        {
            get { return ViewModelRoute.Create(ManagerName.AntProductVehicleDepartmentVm, ManagerName.AntProductVehicleDepartmentV); }
        }

        [Export]
        public ViewModelRoute AntProductAddVehicleDepartmentViewBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductAddVehicleDepartmentVm, ManagerName.AntProductAddVehicleDepartmentV);
            }
        }

        [Export]
        public ViewModelRoute AddVehicleInfoFromDepartmentViewBing
        {
            get { return ViewModelRoute.Create(ManagerName.AddVehicleInfoFromDepartmentVm, ManagerName.AddVehicleInfoFromDepartmentV); }
        }

        [Export]
        public ViewModelRoute VehicleDepartmentListViewBing
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VehicleDepartmentListVm, ManagerName.VehicleDepartmentListV);
            }
        }

        /// <summary>
        /// 人员组织机构Manage
        /// </summary>
        [Export]
        public ViewModelRoute UserDepartmentViewBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductUserDepartmentVm, ManagerName.AntProductUserDepartmentV);
            }
        }

        /// <summary>
        /// 人员部门明细界面
        /// </summary>
        [Export]
        public ViewModelRoute UserDepartmentDetailViewBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductUserDepartmentDetailVm, ManagerName.AntProductUserDepartmentDetailV);
            }
        }

        /// <summary>
        /// 用户列表界面
        /// </summary>
        [Export]
        public ViewModelRoute AntProductUserManageViewBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductUserManageVm, ManagerName.AntProductUserManageV);
            }
        }

        /// <summary>
        /// 用户列表界面
        /// </summary>
        [Export]
        public ViewModelRoute AntProductUserDetailViewBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AntProductUserDetailVm, ManagerName.AntProductUserDetailV);
            }
        }

        /// <summary>
        /// 心跳细节规则
        /// </summary>
        [Export]
        public ViewModelRoute HeartBeatRuleBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SetHeartBeatRuleVm, ManagerName.SetHeartBeatRuleV);
            }
        }
        /// <summary>
        /// 定位信息规则
        /// </summary>
        [Export]
        public ViewModelRoute LocationReportRuleBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.GPSConfigureVm, ManagerName.GPSConfigureV);
            }
        }
        /// <summary>
        /// LED规则
        /// </summary>
        [Export]
        public ViewModelRoute VideoRuleBinding
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SetLEDScrrenRuleVm, ManagerName.SetLEDScrrenRuleV);
            }
        }

        [Export]
        public ViewModelRoute BindingVehicleTypeManage
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VehicleTypeManageViewVm, ManagerName.VehicleTypeManageViewV);
            }
        }

        [Export]
        public ViewModelRoute BindingVehicleTypeDetail
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VehicleTypeDetailViewVm, ManagerName.VehicleTypeDetailViewV);
            }
        }

        [Export]
        public ViewModelRoute BindingUserOnLine
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.UserOnlineInfoVm, ManagerName.UserOnlineInfoV);
            }
        }

        [Export]
        public ViewModelRoute BindingSpeedRuleParameterSetting
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.SpeedRuleParameterSettingVm, ManagerName.SpeedRuleParameterSettingV);
            }
        }

        [Export]
        public ViewModelRoute UserManageLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.OperateLogVm, ManagerName.OperateLogV);
            }
        }

        [Export]
        public ViewModelRoute VideoDownloadLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoDownloadLogVm, ManagerName.VideoDownloadLogV);
            }
        }

        [Export]
        public ViewModelRoute VideoPlayLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.VideoPlayLogVm, ManagerName.VideoPlayLogV);
            }
        }

        [Export]
        public ViewModelRoute AlarmDisposeLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlarmDisposedLogVm, ManagerName.AlarmDisposedLogV);
            }
        }

        [Export]
        public ViewModelRoute AlertDisposeLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.AlertDisposeLogVm, ManagerName.AlertDisposeLogV);
            }
        }

        [Export]
        public ViewModelRoute SystemErrorLog
        {
            get
            {
                return ViewModelRoute.Create(ManagerName.ErrorLogVM, ManagerName.ErrorLogV);
            }
        }
    }
}

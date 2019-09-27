/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 12a6bab8-caf3-4a71-b38d-cc7a0e3333ba      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain
/////    Project Description:    
/////             Class Name: MaintainName
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:17:15 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:17:15 PM
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

namespace Gsafety.PTMS.Maintain
{
    public class MaintainName
    {
        public const string GuaranteePeriodV = "GuaranteePeriodV";
        public const string GuaranteePeriodVm = "GuaranteePeriodVm";

        #region 安全套件

        /// <summary>
        /// 安全套件查询
        /// </summary>
        public const string SecuritySuiteV = "SecuritySuite";
        public const string SecuritySuiteVm = "SecuritySuiteVm";
        public const string SecuritySuiteDisplayV = "SecuritySuiteDisplay";
        public const string SecuritySuiteDisplayVm = "SecuritySuiteDisplayVm";

        /// <summary>
        /// 正在安装的套件
        /// </summary>
        public const string SuiteInstallingV = "SuiteInstalling";
        public const string SuiteInstallingVm = "SuiteInstallingVm";

        /// <summary>
        /// 套件历史安装记录
        /// </summary>
        public const string SuiteHistoryRecordV = "SuiteHistoryRecord";
        public const string SuiteHistoryRecordVm = "SuiteHistoryRecordVm";

        /// <summary>
        /// 套件告警信息
        /// </summary>
        public const string SuiteAlertInfoV = "SuiteAlertInfo";
        public const string SuiteAlertInfoVm = "SuiteAlertInfoVm";

        /// <summary>
        /// 套件自检（开关机状态）
        /// </summary>
        public const string SuiteInspectV = "SuiteInspect";
        public const string SuiteInspectVm = "SuiteInspectVm";
        public const string SuiteInspectDisplayV = "SuiteInspectDisplay";
        public const string SuiteInspectDisplayVm = "SuiteInspectDisplayVm";

        /// <summary>
        /// 套件运维信息
        /// </summary>
        public const string SuiteRunningV = "SuiteRunning";
        public const string SuiteRunningVm = "SuiteRunningVm";
        public const string SuiteRunningDisplayV = "SuiteRunningDisplay";
        public const string SuiteRunningDisplayVm = "SuiteRunningDisplayVm";

        #endregion

        #region 套件维护

        /// <summary>
        /// 异常套件维修安排
        /// </summary>
        public const string MaintenanceHandleV = "MaintenanceHandle";
        public const string MaintenanceHandleVm = "MaintenanceHandleVm";
        public const string MaintenanceHandleDetailV = "MaintenanceHandleDetail";
        public const string MaintenanceHandleDetailVm = "MaintenanceHandleDetailVm";

        /// <summary>
        /// 使用期限
        /// </summary>
        public const string SuiteLifeV = "SuiteLife";
        public const string SuiteLifeVm = "SuiteLifeVm";
        public const string SuiteLifeDetailV = "SuiteLifeDetail";
        public const string SuiteLifeDetailVm = "SuiteLifeDetailVm";

        /// <summary>
        /// 安排记录
        /// </summary>
        public const string HandleRecordV = "HandleRecord";
        public const string HandleRecordVm = "HandleRecordVm";
        public const string HandleRecordDetailV = "HandleRecordDetail";
        public const string HandleRecordDetailVm = "HandleRecordDetailVm";

        /// <summary>
        /// 维修记录MaintenanceScrapVm
        /// </summary>
        public const string MaintainRecordV = "MaintainRecord";
        public const string MaintainRecordVm = "MaintainRecordVm";
        public const string MaintenanceListView = "MaintenanceListView";
        public const string MaintenanceListViewModel = "MaintenanceListViewModel";
        public const string MaintenanceDetailView = "MaintenanceDetailView";
        public const string MaintenanceDetailViewModel = "MaintenanceDetailViewModel";
        public const string MaintainRecordReport = "MaintainRecordReport";
        public const string MaintainRecordReportVM = "MaintainRecordReportVM";
        public const string MaintenanceSimpleV = "MaintenanceSimpleView";
        public const string MaintenanceSimpleVm = "MaintenanceSimpleVm";
        public const string MaintenanceScrapV = "MaintenanceScrapView";
        public const string MaintenanceScrapVm = "MaintenanceScrapVm";


        #endregion

        #region 套件升级

        /// <summary>
        /// 升级版本映射
        /// </summary>
        public const string VersionMappingV = "VersionMapping";
        public const string VersionMappingVm = "VersionMappingVm";
        public const string VersionMappingEditV = "VersionMappingEdit";
        public const string VersionMappingEditVm = "VersionMappingEditVm";
        public const string VersionMappingAddV = "VersionMappingAdd";
        public const string VersionMappingAddVm = "VersionMappingAddVm";

        /// <summary>
        /// 套件升级 
        /// </summary>
        public const string SuiteUpgradeV = "SuiteUpgrade";
        public const string SuiteUpgradeVm = "SuiteUpgradeVm";

        /// <summary>
        /// 套件升级记录
        /// </summary>
        public const string UpgradeRecordV = "UpgradeRecord";
        public const string UpgradeRecordVm = "UpgradeRecordVm";
        public const string UpgradeRecordDisplayV = "UpgradeRecordDisplay";
        public const string UpgradeRecordDisplayVm = "UpgradeRecordDisplayVm";

        /// <summary>
        /// 升级超时
        /// </summary>
        public const string UpgradeOvertimeV = "UpgradeOvertime";
        public const string UpgradeOvertimeVm = "UpgradeOvertimeVm";

        /// <summary>
        /// 升级状态
        /// </summary>
        public const string UpgradeStatusV = "UpgradeStatus";
        public const string UpgradeStatusVm = "UpgradeStatusVm";

        #endregion

    }
}

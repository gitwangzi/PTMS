/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9f23b348-f1ca-4e49-8b2c-8e90cc568e4b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation
/////    Project Description:    
/////             Class Name: InstallationName
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:16:41 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:16:41 PM
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

namespace Gsafety.PTMS.Installation
{
    public class InstallationName
    {
        public const string CategoryName = "DeviceInstallation";
        public const string InstalledRecordV = "InstalledRecord";
        public const string InstalledRecordVm = "InstalledRecordVm";

        public const string InstallVehicleCheckV = "InstallVehicleCheckV";
        public const string InstallVehicleCheckVm = "InstallVehicleCheckVm";

        public const string InstallSuiteCheckV = "InstallSuiteCheckV";
        public const string InstallSuiteCheckVm = "InstallSuiteCheckVm";

        public const string InstallVehcileSuiteCheckVm = "InstallVehcileSuiteCheckVm";
        public const string InstallVehcileSuiteCheckV = "InstallVehcileSuiteCheckV";

        public const string InstallInitiateSuiteV = "InstallInitiateSuiteV";
        public const string InstallInitiateSuiteVm = "InstallInitiateSuiteVm";

        public const string InstallSuiteFunctionCheckV = "InstallSuiteFunctionCheckV";
        public const string InstallSuiteFunctionCheckVm = "InstallSuiteFunctionCheckVm";

        public const string InstallConfirmV = "InstallConfirmV";
        public const string InstallConfirmVm = "InstallConfirmVm";

        public const string InstallGPSVehicleCheckV = "InstallGPSVehicleCheckV";
        public const string InstallGPSVehicleCheckVm = "InstallGPSVehicleCheckVm";

        public const string InstallGPSCheckV = "InstallGPSCheckV";
        public const string InstallGPSCheckVm = "InstallGPSCheckVm";

        public const string InstallVehcileGPSCheckVm = "InstallVehcileGPSCheckVm";
        public const string InstallVehcileGPSCheckV = "InstallVehcileGPSCheckV";

        public const string InstallGPSConfirmV = "InstallGPSConfirmV";
        public const string InstallGPSConfirmVm = "InstallGPSConfirmVm";

        public const string InstalledGPSRecordV = "InstalledGPSRecordV";
        public const string InstalledGPSRecordVm = "InstalledGPSRecordVm";

        public const string DeviceMaintainV = "DeviceMaintain";
        public const string DeviceMaintainVm = "DeviceMaintainVm";

        public const string HistoryInstallationInfoV = "HistoryInstallationInfo";
        public const string HistoryInstallationInfoVm = "HistoryInstallationInfoVm";

        public const string InstallationInfoV = "InstallationInfo";
        public const string InstallationInfoVm = "InstallationInfoVm";

        public const string SetupStationSuiteEditV = "SetupStationSuiteEdit";
        public const string SetupStationSuiteEditVm = "SetupStationSuiteEditVm";

        public const string SuiteMaintenanceV = "SuiteMaintenance";
        public const string SuiteMaintenanceVm = "SuiteMaintenanceVm";

        public const string UnfinishedRecordV = "UnfinishedRecord";
        public const string UnfinishedRecordVm = "UnfinishedRecordVm";

        public const string UnfinishedGPSRecordV = "UnfinishedGPSRecordV";
        public const string UnfinishedGPSRecordVm = "UnfinishedGPSRecordVm";

        public const string InstallverifyV = "InstallverifyView";
        public const string InstallverifyVm = "InstallverifyVm";

        public const string WaitMaintainV = "WaitMaintain";
        public const string WaitMaintainVm = "WaitMaintainVm";

        public const string VehicleRegisterV = "VehicleRegister";
        public const string VehicleRegisterVm = "VehicleRegisterVm";

        public const string SuiteMaintainingV = "SuiteMaintaining";
        public const string SuiteMaintainingVm = "SuiteMaintainingVm";


        public const string SimpleMaintenanceV = "SimpleMaintenance";
        public const string SimpleMaintenanceVm = "SimpleMaintenanceVm";

        public const string SubstitutionMaintenanceV = "SubstitutionMaintenance";
        public const string SubstitutionMaintenanceVm = "SubstitutionMaintenanceVm";

        public const string ScrappedRegistrationV = "ScrappedRegistration";
        public const string ScrappedRegistrationVm = "ScrappedRegistrationVm";

        public const string HistoricalMaintenanceDetailsV = "HistoricalMaintenanceDetails";
        public const string HistoricalMaintenanceDetailsVm = "HistoricalMaintenanceDetailsVm";

        public const string VehicleOnlineV = "VehicleOnline";
        public const string VehicleOnlineVm = "VehicleOnlineVM";

        public const string MaintainApplcationManagementV = "MaintainApplicationManageV";
        public const string MaintainApplcationManagementVm = "MaintainApplicationManageVm";

        public const string MaintainApplcationManagementScheduledV = "MaintainApplcationManagementScheduledV";
        public const string MaintainApplcationManagementScheduledVm = "MaintainApplcationManagementScheduledVm";

        public const string MaintainRecordUnfinishedManageV = "MaintainRecordUnfinishedManageV";
        public const string MaintainRecordUnfinishedManageVm = "MaintainRecordUnfinishedManageVm";

        public const string MaintainRecordManageV = "MaintainRecordManageV";
        public const string MaintainRecordManageVm = "MaintainRecordManageVm";

        public const string DeviceAlertManageV = "DeviceAlertManageV";
        public const string DeviceAlertManageVm = "DeviceAlertManageVm";

        public const string InstallStatisticsV = "InstallStatisticsV";
        public const string InstallStatisticsVm = "InstallStatisticsVm";

        public const string DeviceAlertStatisticsV = "DeviceAlertStatisticsV";
        public const string DeviceAlertStatisticsVm = "DeviceAlertStatisticsVm";

        /// <summary>
        /// 安全套件
        /// </summary>
        public const string DevSuiteManageV = "InstallDevSuiteManageV";

        /// <summary>
        /// 安全套件ViewModel
        /// </summary>
        public const string DevSuiteManageVm = "InstallDevSuiteManageVm";

        /// <summary>
        /// 定位设备
        /// </summary>
        public const string DevGPSManageV = "InstallDevGPSManageV";

        /// <summary>
        ///定位设备ViewModel
        /// </summary>
        public const string DevGPSManageVm = "InstallDevGPSManageVm";
        /// <summary>
        /// 车辆管理
        /// </summary>
        public const string VehicleManageV = "InstallVehicleManageV";

        /// <summary>
        ///车辆管理ViewModel
        /// </summary>
        public const string VehicleManageVm = "InstallVehicleManageVm";
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 73597c16-bbe5-461c-81b6-76019f88d3ec      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation
/////    Project Description:    
/////             Class Name: InstallationBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:25:08 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:25:08 PM
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
using System.ComponentModel.Composition;

using Jounce.Core.ViewModel;

using Gsafety.PTMS.Installation;


namespace Gsafety.PTMS.Installation
{
    public class InstallationBinding
    {
        /// <summary>
        /// 安全套件车辆检查
        /// </summary>
        [Export]
        public ViewModelRoute BindingSuiteVechileCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallVehicleCheckVm, InstallationName.InstallVehicleCheckV);
            }
        }

        /// <summary>
        /// 安全套件套件检查
        /// </summary>
        [Export]
        public ViewModelRoute BindingSuiteSuiteCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallSuiteCheckVm, InstallationName.InstallSuiteCheckV);
            }
        }


        /// <summary>
        ///车辆安全套件绑定关系检查 
        /// </summary>
        [Export]
        public ViewModelRoute BindingSuiteVehcileSuiteRelationCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallVehcileSuiteCheckVm, InstallationName.InstallVehcileSuiteCheckV);
            }
        }

        /// <summary>
        /// 安全套件初始化
        /// </summary>
        [Export]
        public ViewModelRoute BindingSuiteInitial
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallInitiateSuiteVm, InstallationName.InstallInitiateSuiteV);
            }
        }

        /// <summary>
        /// 安全套件功能检查
        /// </summary>
        [Export]
        public ViewModelRoute BindingSuiteFunctionCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallSuiteFunctionCheckVm, InstallationName.InstallSuiteFunctionCheckV);
            }
        }

        /// <summary>
        /// 安全套件安装确认
        /// </summary>
        [Export]
        public ViewModelRoute BindingUploadPictureAndConfirmPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallConfirmVm, InstallationName.InstallConfirmV);
            }
        }

        [Export]
        public ViewModelRoute BindingSuiteInstalledRecordPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstalledRecordVm, InstallationName.InstalledRecordV);
            }
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 安全套件车辆检查
        /// </summary>
        [Export]
        public ViewModelRoute BindingGPSVechileCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallGPSVehicleCheckVm, InstallationName.InstallGPSVehicleCheckV);
            }
        }

        /// <summary>
        /// 定位设备检查
        /// </summary>
        [Export]
        public ViewModelRoute BindingGPSSuiteCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallGPSCheckVm, InstallationName.InstallGPSCheckV);
            }
        }

        /// <summary>
        /// 定位设备确认
        /// </summary>
        [Export]
        public ViewModelRoute BindingGPSConform
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallGPSConfirmVm, InstallationName.InstallGPSConfirmV);
            }
        }

        /// <summary>
        ///车辆定位设备绑定关系检查 
        /// </summary>
        [Export]
        public ViewModelRoute BindingGPSVehcileSuiteRelationCheck
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallVehcileGPSCheckVm, InstallationName.InstallVehcileGPSCheckV);
            }
        }

        [Export]
        public ViewModelRoute BindingGPSUnFinishedRecord
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.UnfinishedGPSRecordVm, InstallationName.UnfinishedGPSRecordV);
            }
        }

        [Export]
        public ViewModelRoute BindingGPSFinishedRecord
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstalledGPSRecordVm, InstallationName.InstalledGPSRecordV);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Export]
        public ViewModelRoute BindingDeviceMaintainPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.DeviceMaintainVm, InstallationName.DeviceMaintainV);
            }
        }

        [Export]
        public ViewModelRoute BindingHistoryInstallationInfoPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.HistoryInstallationInfoVm, InstallationName.HistoryInstallationInfoV);
            }
        }

        [Export]
        public ViewModelRoute BindingInstallationInfoPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallationInfoVm, InstallationName.InstallationInfoV);
            }
        }

        [Export]
        public ViewModelRoute BindingSetupStationSuiteEditPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.SetupStationSuiteEditVm, InstallationName.SetupStationSuiteEditV);
            }
        }

        [Export]
        public ViewModelRoute BindingSuiteMaintenancePage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.SuiteMaintenanceVm, InstallationName.SuiteMaintenanceV);
            }
        }

        [Export]
        public ViewModelRoute BindingUnfinishedRecordPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.UnfinishedRecordVm, InstallationName.UnfinishedRecordV);
            }
        }

        [Export]
        public ViewModelRoute BindingInstallverifyPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallverifyVm, InstallationName.InstallverifyV);
            }
        }

        [Export]
        public ViewModelRoute BindingWaitMaintainPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.WaitMaintainVm, InstallationName.WaitMaintainV);
            }
        }

        [Export]
        public ViewModelRoute BindingSuiteMaintainingPage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.SuiteMaintainingVm, InstallationName.SuiteMaintainingV);
            }
        }


        [Export]
        public ViewModelRoute BindingVehicleRegister
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.VehicleRegisterVm, InstallationName.VehicleRegisterV);
            }
        }

        [Export]
        public ViewModelRoute BindingSubstitutionMaintenance
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.SubstitutionMaintenanceVm, InstallationName.SubstitutionMaintenanceV);
            }
        }

        [Export]
        public ViewModelRoute BindingScrappedRegistration
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.ScrappedRegistrationVm, InstallationName.ScrappedRegistrationV);
            }
        }

        [Export]
        public ViewModelRoute BindingSimpleMaintenance
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.SimpleMaintenanceVm, InstallationName.SimpleMaintenanceV);
            }
        }

        [Export]
        public ViewModelRoute BindingHistoricalMaintenanceDetails
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.HistoricalMaintenanceDetailsVm, InstallationName.HistoricalMaintenanceDetailsV);
            }
        }

        [Export]
        public ViewModelRoute VehicleOnline
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.VehicleOnlineVm, InstallationName.VehicleOnlineV);
            }
        }

        [Export]
        public ViewModelRoute MaintainApplcationManagement
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.MaintainApplcationManagementVm, InstallationName.MaintainApplcationManagementV);
            }
        }

        [Export]
        public ViewModelRoute MaintainApplcationManagementScheduled
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.MaintainApplcationManagementScheduledVm, InstallationName.MaintainApplcationManagementScheduledV);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 安全套件管理
        /// </summary>
        [Export]
        public ViewModelRoute MaintainRecordUnfinishedManage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.MaintainRecordUnfinishedManageVm, InstallationName.MaintainRecordUnfinishedManageV);
            }
        }

        [Export]
        public ViewModelRoute MaintainRecordManage
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.MaintainRecordManageVm, InstallationName.MaintainRecordManageV);
            }
        }

        [Export]
        public ViewModelRoute DevSuiteManagement
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.DevSuiteManageVm, InstallationName.DevSuiteManageV);
            }
        }

        /// <summary>
        /// 定位设备管理
        /// </summary>
        [Export]
        public ViewModelRoute DevGPSManagement
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.DevGPSManageVm, InstallationName.DevGPSManageV);
            }
        }

        /// <summary>
        /// 车辆管理
        /// </summary>
        [Export]
        public ViewModelRoute VechileManagement
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.VehicleManageVm, InstallationName.VehicleManageV);
            }
        }
        /// <summary>
        /// 设备告警管理
        /// </summary>
        [Export]
        public ViewModelRoute DeviceAlertManagement
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.DeviceAlertManageVm, InstallationName.DeviceAlertManageV);
            }
        }

        [Export]
        public ViewModelRoute InstallStatistics
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.InstallStatisticsVm, InstallationName.InstallStatisticsV);
            }
        }

        [Export]
        public ViewModelRoute DeviceStatistics
        {
            get
            {
                return ViewModelRoute.Create(InstallationName.DeviceAlertStatisticsVm, InstallationName.DeviceAlertStatisticsV);
            }
        }
    }
}

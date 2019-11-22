/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4b62ac5f-4957-4662-abe1-4a7527384f03      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite
/////    Project Description:    
/////             Class Name: SecuritySuiteName
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:14:24 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:14:24 PM
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

namespace Gsafety.PTMS.SecuritySuite
{
    public class SecuritySuiteName
    {

        public const string SuiteContainer = "SuiteContainer";                  
        public const string CategoryName = "SecuritySuite";                      
        public const string InstallMenuName = "SuiteInstall";                    
        public const string StatusMenuName = "SuiteStatus";                      
        public const string MaintainMenuName = "MaintainInfo";                   
        public const string VehicleTrafficMenuName = "VehicleTraffic";
        public const string VehicleEquipmentMenuName = "VehicleEquipment";

        #region MAINPAGE

        public const string SuiteMainPageV = "SuiteMainPage";                   
        public const string SuiteMainPageVm = "SuiteMainPageVm";                

        public const string SuiteMenuV = "SuiteMenu";                            
        public const string SuiteMenuVm = "SuiteMenuVm";                         

        #endregion

        #region INSTALL

        public const string InstallFinishV = "InstallFinish";                    
        public const string InstallFinishVm = "InstallFinishVm";                

        public const string InstallingRecordV = "InstallingRecord";              
        public const string InstallingRecordVm = "InstallingRecordVm";           


        #endregion

        #region VEHICLE SATTUS(ONLINE ,OFFLINE,SECURITYSUITESTATUS)

        public const string OnLineV = "OnLine";                                 
        public const string OnLineVm = "OnLineVm";                               

        public const string OffLineV = "OffLine";                                
        public const string OffLineVm = "OffLineVm";                            

        public const string SuiteStatusManagementV = "SuiteStatusManagement";                 
        public const string SuiteStatusManagementVm = "SuiteStatusManagementVm";

        public const string VehicleElectronicFenceV = "VehicleElectronicFence";
        public const string VehicleElectronicFenceVM = "VehicleElectronicFenceVM";


        #endregion

        public const string SwitchingStatusV = "SwitchingStatus";                                  
        public const string SwitchingStatusVm = "SwitchingStatusVm";
        public const string SwitchingStatusDisplayV = "SwitchingStatusDisplay";
        public const string SwitchingStatusDisplayVm = "SwitchingStatusDisplayVm";

        public const string SwitchingSuiteAlertInfoV = "SwitchingSuiteAlertInfo";
        public const string SwitchingSuiteAlertInfoVm = "SwitchingSuiteAlertInfoVm";

        #region 设备维修

        public const string DeviceMaintanceDetail = "DeviceMaintanceDetail";
        public const string DeviceMaintanceDetailVM = "DeviceMaintanceDetailVM";

        #endregion


        public const string SuiteUpgradeV = "SuiteUpgrade";                       ///////////套件升级View
        public const string SuiteUpgradeVm = "SuiteUpgradeVm";                    ///////////套件升级Vm


        #region 车辆交通行驶细节 

        public const string TravelPlanImplementationV = "TravelPlanImplement";
        public const string TravelPlanImplementationVM = "TravelPlanImplementationVM";

        public const string TravelPlanDetailV = "TravelPlanDetail";
        public const string TravelPlanDetailVM = "TravelPlanDetailVM";

        #endregion

        #region 安全套件状态管理

        public const string InfoSuiteStatusV = "InfoSuiteStatus";
        public const string InfoSuiteStatusVm = "InfoSuiteStatusVm";        ///////////安全套件状态管理主Vm
        #endregion

    }
}

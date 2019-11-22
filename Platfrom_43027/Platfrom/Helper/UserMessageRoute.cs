/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a9eb8b01-dd9e-49fd-b2ef-0431734f29be      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: UserMessageRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 16:04:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 16:04:26
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    public class UserMessageRoute
    {
        #region Business Alert Message ID

        [Route(typeName: "OverSpeed")]
        public const string OverSpeed = "MDVR.OverSpeed.";

        [Route(typeName: "InOutAera")]
        public const string InOutAera = "MDVR.InOutAera.";

        [Route(typeName: "InOutRoute")]
        public const string InOutRoute = "MDVR.InOutRoute.";

        [Route(typeName: "RouteOffset")]
        public const string RouteOffset = "MDVR.RouteOffset.";

        [Route(typeName: "FatigueDrive")]
        public const string FatigueDrive = "MDVR.FatigueDrive.";

        [Route(typeName: "DangerWarning")]
        public const string DangerWarning = "MDVR.DangerWarning.";

        [Route(typeName: "OverSpeedWarning")]
        public const string OverSpeedWarning = "MDVR.OverSpeedWarning.";

        [Route(typeName: "FatigueDriveWarning")]
        public const string FatigueDriveWarning = "MDVR.FatigueDriveWarning.";

        [Route(typeName: "DriveTime")]
        public const string DriveTime = "MDVR.DriveTime.";

        [Route(typeName: "VehicleStolen")]
        public const string VehicleStolen = "MDVR.VehicleStolen.";

        [Route(typeName: "IllegalIgnition")]
        public const string IllegalIgnition = "MDVR.IllegalIgnition.";

        [Route(typeName: "IllegalDisplacement")]
        public const string IllegalDisplacement = "MDVR.IllegalDisplacement.";

        [Route(typeName: "CollisionWaring")]
        public const string CollisionWaring = "MDVR.CollisionWaring.";

        [Route(typeName: "RolloverWaring")]
        public const string RolloverWaring = "MDVR.RolloverWaring.";

        #endregion

        #region Device Alert Message ID

        [Route(typeName: "GNSS_ModuleBroken")]
        public const string GNSS_ModuleBroken = "MDVR.GNSS_ModuleBroken.";

        [Route(typeName: "GNSS_AerialNoExist")]
        public const string GNSS_AerialNoExist = "MDVR.GNSS_AerialNoExist.";

        [Route(typeName: "GNSS_AerialBroken")]
        public const string GNSS_AerialBroken = "MDVR.GNSS_AerialBroken.";

        [Route(typeName: "PowerUndervoltage")]
        public const string PowerUndervoltage = "MDVR.PowerUndervoltage.";

        [Route(typeName: "PowerLost")]
        public const string PowerLost = "MDVR.PowerLost.";

        [Route(typeName: "LED_Broken")]
        public const string LED_Broken = "MDVR.LED_Broken.";

        [Route(typeName: "TTS_ModuleBroken")]
        public const string TTS_ModuleBroken = "MDVR.TTS_ModuleBroken.";

        [Route(typeName: "CameraBroken")]
        public const string CameraBroken = "MDVR.CameraBroken.";

        #endregion

        /// <summary>
        /// Handing Alarm
        /// </summary>
        public const string HandingAlarmKey = "APP.UserMessage.HandingAlarm";

        /// <summary>
        /// Complete Alarm
        /// </summary>
        public const string CompleteAlarmKey = "APP.UserMessage.CompleteAlarm";

        /// <summary>
        /// Cancel Alarm
        /// </summary>
        public const string DisarmAlarmKey = "APP.UserMessage.DisarmAlarm";


        public const string ChangeGroupVechleKey = "APP.UserMessge.ChangeGroupVechleKey";

        public const string ChangeGroupKey = "APP.UserMessge.ChangeGroupKey";
        /// <summary>
        /// Device Install
        /// </summary>
        public const string CompleteSuiteInstallKey = "APP.UserMessage.CompleteSuiteInstallKey";

        public const string InstallCompleteNotification = "APP.UserMessage.InstallCompleteNotification";

        public const string AuthenticationReponse = "APP.UserMessage.AuthenticationResponse";

        public const string AuthenticationRequest = "APP.UserMessage.AuthenticationRequest";

        /// <summary>
        /// Device Maintain
        /// </summary>
        public const string SuiteMaintainKey = "APP.UserMessage.DeviceMaintain";

        /// <summary>
        /// Start Install
        /// </summary>
        public const string StartSuiteInstallKey = "APP.UserMessage.StartSuiteInstallKey";

        /// <summary>
        /// Delete Install
        /// </summary>
        public const string DeleteSuiteInstallKey = "APP.UserMessage.DeleteSuiteInstallKey";

        /// <summary>
        /// Device Install
        /// </summary>
        public const string CompleteGPSInstallKey = "APP.UserMessage.CompleteGPSInstallKey";

        /// <summary>
        /// Device Maintain
        /// </summary>
        public const string GPSMaintainKey = "APP.UserMessage.GPSMaintainKey";

        /// <summary>
        /// Start Install
        /// </summary>
        public const string StartGPSInstallKey = "APP.UserMessage.StartGPSInstallKey";

        /// <summary>
        /// Delete Install
        /// </summary>
        public const string DeleteGPSInstallKey = "APP.UserMessage.DeleteGPSInstallKey";
        /// <summary>
        /// Handing Alert
        /// </summary>
        public const string HandingAlertKey = "APP.UserMessage.HandingAlert";

        /// <summary>
        /// Complete Alert
        /// </summary>
        public const string CompleteAlertKey = "APP.UserMessage.CompleteAlert";

        /// <summary>
        /// Delete User
        /// </summary>
        public const string DeleteUserKey = "APP.UserMessage.DeleteUser";

        /// <summary>
        /// Change User
        /// </summary>
        public const string ChangeUserKey = "APP.UserMessage.ChangeUser";

        /// <summary>
        /// user send route
        /// </summary>
        public const string OriginalRouteKey = "APP.UserMessage.RouteCMD";

        /// <summary>
        /// user send plan
        /// </summary>
        public const string OriginalTravePlanKey = "APP.UserMessage.TravePlan";

        /// <summary>
        /// user send plan
        /// </summary>
        public const string OriginalElectronicFenceKey = "APP.UserMessage.ElectronicFence";

        /// <summary>
        /// user send monitor info
        /// </summary>
        public const string OriginalMonitorPointKey = "APP.UserMessage.MonitorPoint";

        /// <summary>
        /// send upgrade info
        /// </summary>
        public const string OriginalUpgradeNotifyKey = "APP.UserMessage.UpgradeNotify";

        /// <summary>
        /// MDVRGPS Information Directive
        /// </summary>
        public const string OriginalLocationMonitorKey = "APP.UserMessage.LocationMonitor";

        /// <summary>
        /// MDVRGPS Information Directive
        /// </summary>
        public const string CancelLocationMonitorKey = "APP.UserMessage.CancelLocationMonitor";

        /// <summary>
        /// End Monitoring
        /// </summary>
        public const string LocationMonitorEndKey = "APP.UserMessage.MonitorEnd.";

        public const string UserLogin = "APP.UserMessage.UserLogin";
    }
}

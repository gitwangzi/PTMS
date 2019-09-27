/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5290236b-5425-4e29-8716-2c99ed562f61      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: SettingRoute
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/23 14:11:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/23 14:11:30
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
    public class SettingRoute
    {
        #region GpsUpload
        public const string OriginalSettingGPSUploadCMDKey = "MDVR.SettingGPSUploadCMDKey.";
        public const string HandleSettingGPSUploadReplyKey = "MDVR.SettingGPSUploadReply.";
        public const string HandleSettingGPSUploadCMDKey = "MDVR.C30.";

        /// <summary>
        /// 表示传送要设置的对象实体
        /// </summary>
        public const string GpsSettingObjectKey = "PTMS.GpsSettingObjectKey.";

        public const string TemperatureSettingObjectKey = "PTMS.TemperatureSettingObjectKey.";

        public const string OneKeyAlarmSettingObjectKey = "PTMS.OneKeyAlarmSettingObjectKey.";

        public const string AbnormalDoorSettingObjectKey = "PTMS.AbnormalDoorSettingObjectKey.";

        public const string OverSpeedObjectKey = "PTMS.OverSpeedObjectKey.";

        public const string ElectricFenceObjectKey = "PTMS.ElectricFenceObjectKey.";

        public const string DeleteRuleObjectKey = "PTMS.DeleteRuleObjectKey.";

        public const string SendInfomationKey = "PTMS.SendInfomationKey.";

        public const string VideoListCMDKey = "PTMS.VideoListCMDKey.";

        public const string DownloadMdvrFile = "PTMS.DownloadMdvrFile";

       
        #endregion
        public const string OriginalHeartBeatKey = "MDVR.HeartBeat.";

        public const string HandleHeartBeatResponseKey = "MDVR.HeartBeatResponse.";



    }
}

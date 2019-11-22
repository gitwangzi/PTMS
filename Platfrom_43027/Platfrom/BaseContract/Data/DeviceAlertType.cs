/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7d19f7dc-8bac-4c83-9d7c-126cc9f14295      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: DeviceAlertType
/////          Class Version: v1.0.0.0
/////            Create Time: 8/26/2013 5:17:04 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/26/2013 5:17:04 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    /// <summary>
    /// Device Alarm Type 
    /// </summary>
    public enum DeviceAlertType
    {
        LowTemperature =10,
        /// <summary>
        /// Temperature Alarm
        /// </summary>
        OverTemperature = 11,
        /// <summary>
        /// GPS Receiver Error Alarm
        /// </summary>
        GpsFault = 12,
        /// <summary>
        /// Camera Block Alarm
        /// </summary>
        VedioShelter = 13,
        /// <summary>
        /// Camera No-Signal Alarm
        /// </summary>
        VedioNoSignal = 14,
        /// <summary>
        /// Fire Alarm
        /// </summary>
        AbnormalFire = 15,
        /// <summary>
        /// MDVR Card Error Alarm
        /// </summary>
        SdFault = 16,
        /// <summary>
        /// Three Time Password Error Alarm
        /// </summary>
        PasswordFault = 17,
        /// <summary>
        /// Abnormal Valtage Error Alarm
        /// </summary>
        AbnormalValtage = 18,
        /// <summary>
        /// Device Offline in 72 Hours Alarm
        /// </summary>
        Offline72 = 21,
        /// <summary>
        /// Device Offline in 48 Hours Alarm
        /// </summary>
        Offline48 = 22,
        /// <summary>
        /// Device Offline in 24 Hours Alarm
        /// </summary>
        offline24 = 23,
        /// <summary>
        /// Damage Alarm
        /// </summary>
        Damage = 31,
        /// <summary>
        /// Security Suite No-Signal(not in table ,just for the operation type)
        /// </summary>
        SuitNoSignal=101,
        /// <summary>
        /// No Ant GPS Data Upload(not in table ,just for the operation type)
        /// </summary>
        NoANTGPSSignal=102
    }
}

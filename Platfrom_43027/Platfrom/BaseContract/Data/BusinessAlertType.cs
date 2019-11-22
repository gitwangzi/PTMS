/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7d19d7dc-8bac-4c13-9e7c-126cd3f14295      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-GUOH
/////                 Author: TEST(guoh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: BusinessAlertType
/////          Class Version: v1.0.0.0
/////            Create Time: 8/31/2013 13:50:04 
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/31/2013 13:50:04 
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
    /// BusinessAlertType
    /// </summary>
    public enum BusinessAlertType
    {
        /// <summary>
        /// InFence Alarm
        /// </summary>
         
        InFence = 11,
        /// <summary>
        /// OutFence Alarm
        /// </summary>
        OutFence = 12,
        /// <summary>
        /// OverSpeed Alarm
        /// </summary>
        OverSpeed = 13,
        /// <summary>
        /// Common LowSpeed Alarm
        /// </summary>
        UnderSpeed = 14,
        /// <summary>
        /// OverSpeed In fence Alarm
        /// </summary>
        OverSpeedFence = 15,
        /// <summary>
        ///LowSpeed In Fence Alarm
        /// </summary>
        UnderSpeedFence = 16,
        /// <summary>
        /// Out of Orbit Alarm
        /// </summary>
        DepartureRoute = 17,
        /// <summary>
        /// AbnormalDoor Alarm
        /// </summary>
        AbnormalDoor = 18,
        /// <summary>
        /// OverMileage Alarm
        /// </summary>
        OverMileage = 21,

        /// <summary>
        /// OverSpeed (Platform Monitor)
        /// </summary>
        MonitorOverSpeed = 31,
        /// <summary>
        /// Recovery Speed(Platform Monitor)
        /// </summary>
        MonitorNormalSpeed = 32,

        /// <summary>
        /// Out of Orbit(Platform Monitor)
        /// </summary>
        MonitorOutRoute = 33,
        /// <summary>
        /// In Orbit(Platform Monitor)
        /// </summary>
        MonitorInRoute = 34,
       

        /// <summary>
        /// MonitorInFenceOverSpeed2Normal(Platform Monitor)
        /// </summary>
        MonitorInFenceOverSpeed2Normal = 35,
       
        /// <summary>
        /// MonitorInFenceUnderSpeed2Normal(Platform Monitor)
        /// </summary>
        MonitorInFenceUnderSpeed2Normal = 36,

        /// <summary>
        /// MonitorTravelPlanBegin
        /// </summary>
        MonitorTravelPlanBegin = 37,     
        
        /// <summary>
        /// MonitorTravelPlanEnd
        /// </summary>
        MonitorTravelPlanEnd = 38,                          

        /// <summary>
        /// MonitorTravelPlanCancel
        /// </summary>
        MonitorTravelPlanCancel = 39,                      

        /// <summary>
        /// MonitorEnterPoint
        /// </summary>
        MonitorEnterPoint = 40,                            

    }
}

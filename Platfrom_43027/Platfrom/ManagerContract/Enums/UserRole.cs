/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
///// Guid: 9d8c2ee2-20b4-434b-ae6b-6f354a75cb63      
/////clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-ShiHS
///// Author: TEST(ShiHongSheng)
/////======================================================================
/////Project Name: Gsafety.PTMS.Manager.Repository
/////Project Description:    
/////Class Name: AntLogManageRepository
/////Class Version: v1.0.0.0
/////Create Time: 2013/10/17 16:57:29
/////Class Description:  
/////======================================================================
/////Modified Time: 2013/10/17 16:57:29
/////Modified by:(ShiHS)
/////Modified Description: 
/////======================================================================
/////Modified Time: 2014/10/10 16:57:29
/////Modified by:(ShiHS)
/////Modified Description: 
/////======================================================================
using System;
using System.ComponentModel;
namespace Gsafety.PTMS.Manager.Contract
{
    [Flags]
    public enum UserRole : short
    {
        /// <summary>
        /// Monitor
        /// </summary>
        [Description("E9_PTMS")]
        Monitor = 1,
        /// <summary>
        /// E9_SecurityManager
        /// </summary>
        [Description("E9_SecurityManager")]
        SecurityManager = 2,
        ///// <summary>
        ///// VehicleCompany
        ///// </summary>
        //[Description("E9_VehicleCompany")]
        //VehicleCompany = 2,
        /// <summary>
        /// InstallStation
        /// </summary>
        [Description("E9_InstallStation")]
        InstallStation = 3,
        /// <summary>
        /// Maintenance
        /// </summary>
        [Description("E9_Maintenance")]
        Maintenance = 4,
        /// <summary>
        /// Arads
        /// </summary>
        [Description("E9_Arads")]
        Arads = 5,
        /// <summary>
        /// Other
        /// </summary>
        [Description("E9_Other")]
        Other = 6,
        /// <summary>
        /// AlarmFilterCommissioner
        /// </summary>
        [Description("E9_AlarmFilter")]
        AlarmFilterCommissioner = 7,
    }
}

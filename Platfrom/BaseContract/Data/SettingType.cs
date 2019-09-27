/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 69804c2c-f954-4739-ac35-3d98aac989a2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SettingType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 14:36:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 14:36:07
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract]
    public enum SettingType
    {
        /// <summary>
        /// Province Code
        /// </summary>
        [EnumMember]
        ProvinceCode,
        /// <summary>
        /// Province Code
        /// </summary>
        [EnumMember]
        CityCode,
        /// <summary>
        /// Country
        /// </summary>
        [EnumMember]
        CountryWide,
        /// <summary>
        /// Vehicle
        /// </summary>
        [EnumMember]
        VehicleType,
        /// <summary>
        /// Vehicle
        /// </summary>
        [EnumMember]
        Vehicle,

        [EnumMember]
        Group,

        /// <summary>
        /// FenceID, this type is to modify the Vehicle_fence ,and then send the command by fenceid
        /// </summary>
        [EnumMember]
        FenceID
    }
}

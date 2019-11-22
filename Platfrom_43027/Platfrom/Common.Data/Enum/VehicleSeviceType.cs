/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: df5daad1-2328-44e9-98d0-b9a1badf6acc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleSeviceType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/29 18:43:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/29 18:43:26
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data.Enum
{
    public enum VehicleSeviceType
    {
        /// <summary>
        /// Comercial
        /// </summary>
        Comercial = 1,
        /// <summary>
        /// Public
        /// </summary>
        Public = 2,
        /// <summary>
        /// Private
        /// </summary>
        Private = 3,   
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 99,
    }
}

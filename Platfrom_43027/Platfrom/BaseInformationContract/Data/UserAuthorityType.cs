/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: b0d421af-1dcd-4671-a38c-52220f3ec9cb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: UserAuthorityType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/12 9:43:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/12 9:43:04
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    public enum UserAuthorityType
    {
        /// <summary>
        /// Company
        /// </summary>
        Company = -1,
        /// <summary>
        /// CountryLevel
        /// </summary>
        CountryLevel = 0,
        /// <summary>
        /// ProvinceLevel
        /// </summary>
        ProvinceLevel = 1,
        /// <summary>
        /// CityLevel
        /// </summary>
        CityLevel = 2,
    } 
}

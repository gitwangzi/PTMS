/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ba24c4b7-935f-47ea-99d8-5e1216f5c285      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract.Data
/////    Project Description:    
/////             Class Name: UserAuthorityType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/7 16:13:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/7 16:13:29
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract.Data
{
    public enum UserAuthorityType
    {
        Company = -1,

        CountryLevel = 0,

        ProvinceLevel = 1,

        CityLevel = 2,
    }
}

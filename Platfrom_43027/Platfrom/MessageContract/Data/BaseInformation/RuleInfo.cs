/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 48035ca7-5bc6-409c-9fa8-54190ca694a0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.BaseInformation
/////    Project Description:    
/////             Class Name: RuleInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/12 17:47:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/12 17:47:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    public class RuleInfo
    {
        public List<Province> ListProvince { get; set; }
    }

    public class Province
    {
        public string ProvinceId { get; set; }

        public List<City> ListCity { get; set; }
    }

    public class City
    {
        public string CityId { get; set; }

        public List<Company> ListCompany { get; set; }
    }

    public class Company
    {
        public string CompanyId { get; set; }
    }
}

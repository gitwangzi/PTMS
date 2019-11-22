/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 89adb5c5-418c-4a4e-8021-05e494037d5a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Alarm
/////    Project Description:    
/////             Class Name: ECU911Mapping
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/12/2 11:11:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/12/2 11:11:08
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
    public class ECU911Mapping
    {
        public string ID { get; set; }
        public string DistrictCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ECU911Url { get; set; }
        public string ECU911Name { get; set; }
    }
}

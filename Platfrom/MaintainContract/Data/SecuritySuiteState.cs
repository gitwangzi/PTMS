/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4d0a06cd-c482-4715-85b1-a5457ca80bec      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIUXP
/////                 Author: TEST(liuxp)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain.Contract.Data
/////    Project Description:    
/////             Class Name: SecuritySuiteState
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 10:55:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 10:55:27
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.BaseInformation.Contract.Data;

namespace Gsafety.PTMS.Maintain.Contract.Data
{
    public enum SecuritySuiteState
    {
        Initial,
        Setup,
        Running,
        Exception,
        Maintain,
        Invalidate
    }
}

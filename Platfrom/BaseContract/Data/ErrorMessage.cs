/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1648b651-dace-44f0-b65a-751dc738022c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: ErrorMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/12 10:53:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/12 10:53:52
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
    /// Error AckMessage from Server
    /// </summary>
    public enum ErrorMessage
    {
        /// <summary>
        /// Vehicle not Exist
        /// </summary>
        ValidateNotRight,
        /// <summary>
        /// Vehicle not Running
        /// </summary>
        SUITE_VehicleNotRunning,
        /// <summary>
        /// Vehicle not Error
        /// </summary>
        SUITE_VehicleAbnormal
    }
}

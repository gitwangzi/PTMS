/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: eddef0fe-0477-4a37-ac64-2bfad418217b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract.Data
/////    Project Description:    
/////             Class Name: VehicleType
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/30 10:00:24
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/30 10:00:24
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract.Data
{
    /// <summary>
    /// Vehicle Type
    /// </summary>
    public enum VehicleType
    {
        /// <summary>
        /// Taxi
        /// </summary>
        Taxi = 1,
        /// <summary>
        /// Bus
        /// </summary>
        Bus = 2,
        /// <summary>
        /// Long-distance bus
        /// </summary>
        Flota = 3,
    }
}

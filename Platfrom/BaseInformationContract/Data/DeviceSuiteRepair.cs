/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f1f49f8b-7c83-4dc1-9fd3-76585227f7ff      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: GAOZITIAN-PC
/////                 Author: TEST(gaozt)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: DeviceSuiteRepair
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/6 17:35:09
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/6 17:35:09
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    public class DeviceSuiteRepair
    {
        /// <summary>
        /// Old Device Suite
        /// </summary>
        [DataMember]
        public DeviceSuite OldDeviceSuite;

        /// <summary>
        /// New Device Suite
        /// </summary>
        [DataMember]
        public DeviceSuite NewDeviceSuite;
    }
}

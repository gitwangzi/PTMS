/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cc6a79b5-9638-43bf-bcb1-c64e7cab9e33      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: VehicleCheckResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:09:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 14:09:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Common.Data
{
    [DataContract]
    public class VehicleCheckResult
    {
        /// <summary>
        /// Result
        /// </summary>
        [DataMember]
        public int Result { get; set; }
        /// <summary>
        /// InvalidCode 1 not find，2 installed
        /// </summary>
        [DataMember]
        public int InvalidCode { get; set; }
        /// <summary>
        /// Type :1 Taxi 2 public bus 3 bus
        /// </summary>
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        [DataMember]
        public int Status { get; set; }
    }
}

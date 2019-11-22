/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e34e0083-596b-45fe-a01c-d6b9e0a9755c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Contract
/////    Project Description:    
/////             Class Name: Vehicle
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 15:46:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/1 15:46:21
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Traffic.Contract
{
    [DataContract]
    public class Vehicle
    {
        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string Vehicle_ID { get; set; }

        [DataMember]
        public string MDVR_CODE_SN { get; set; }
    }
}

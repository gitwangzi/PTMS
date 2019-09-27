/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3f59a76b-589d-4afc-b4c3-3c74b860ee1a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data
/////    Project Description:    
/////             Class Name: HeartbeatInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 11/25/2013 5:20:06 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/25/2013 5:20:06 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [DataContract]
    [Serializable]
    public class HeartbeatInfo
    {
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public Nullable<DateTime> CurrentTime { get; set; }
    }
}

/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 84dbf55f-81a0-4fa7-902e-db8ab59373b8      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.VideoLog.Contract
/////    Project Description:    
/////             Class Name: VerifyUserResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2014-01-17 09:48:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014-01-17 09:48:30
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.VideoLog.Contract
{
    [DataContract]
    public  class VerifyUserResult
    {
        [DataMember]
        public string result { get; set; }
    }
}

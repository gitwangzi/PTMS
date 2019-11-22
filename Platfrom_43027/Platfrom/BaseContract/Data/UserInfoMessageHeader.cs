/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a02dd31b-19d8-4733-9f77-15da0a0d20b9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: UserInfoMessageHeader
/////          Class Version: v1.0.0.0
/////            Create Time: 10/22/2013 10:38:29 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 10/22/2013 10:38:29 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    public class UserInfoMessageHeader
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Province { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public string Group { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string CompanyId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
    }
}

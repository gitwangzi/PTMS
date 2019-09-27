/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a2a5d6b2-6b74-4938-a6b7-671553daf752      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: RuleOperationType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/9 15:54:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/9 15:54:39
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    [Serializable]
    public enum RuleOperationType
    {
        [EnumMember]
        Add,
        [EnumMember]
        Delete,
        [EnumMember]
        Default,
        [EnumMember]
        Update
    }
}

using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9214ff47-af33-4971-83ae-6b8588a45c63      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.DeleteRule
/////    Project Description:    
/////             Class Name: RuleDeleteModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/5 15:57:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/5 15:57:34
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
    public class RuleDeleteModel
    {
        [DataMember]
        public DeleteType DeleteType;
        [DataMember]
        public RuleType RuleType;
        [DataMember]
        public string RuleId;
        [DataMember]
        public List<string> Value;
    }
}

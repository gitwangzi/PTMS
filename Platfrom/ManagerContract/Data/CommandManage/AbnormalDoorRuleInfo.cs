/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4c8f082e-0a9c-4088-a48b-30e90d149d4c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: AbnormalDoorRule
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/4 16:11:51
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/4 16:11:51
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
    [Serializable]
    [DataContract]
    public class AbnormalDoorRuleInfo : RuleBaseInfo
    {
        [DataMember]
        public int Speed { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            return builder.ToString();
        }

    }
}

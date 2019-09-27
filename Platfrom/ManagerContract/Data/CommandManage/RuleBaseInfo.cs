/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0c7df0bd-aaef-4ffb-b039-c1888ee3ce0e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: RuleBaseInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/30 13:52:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/30 13:52:15
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
    public class RuleBaseInfo
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string RuleName { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
        [DataMember]
        public DateTime? CreateTime { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public string UserDescription { get; set; }
        [DataMember]
        public bool ValID { get; set; }
        [DataMember]
        public int UsingCount { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RuleName)))
            {
                builder.AppendLine("RuleName:" + RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(IsDefault)))
            {
                builder.AppendLine("IsDefault:" + IsDefault.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Creator)))
            {
                builder.AppendLine("Creator:" + Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UserDescription)))
            {
                builder.AppendLine("UserDescription:" + UserDescription.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ValID)))
            {
                builder.AppendLine("ValID:" + ValID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UsingCount)))
            {
                builder.AppendLine("UsingCount:" + UsingCount.ToString());
            }
            return builder.ToString();
        }

    }
}

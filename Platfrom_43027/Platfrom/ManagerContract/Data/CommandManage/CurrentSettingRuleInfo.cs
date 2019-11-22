/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5ea89888-af54-44e7-8f3b-0681ca3d0840      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: CurrentSettingRuleInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/10 13:54:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/10 13:54:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Manager.Contract.Data.CommandManage
{
    [DataContract]
    public class CurrentSettingRuleInfo
    {
        [DataMember]
        public string RuleName { get; set; }
        
        [DataMember]
        public string VehicleID { get; set; }

        [DataMember]
        public string MdvrCoreID { get; set; }

        [DataMember]
        public DateTime SendTime { get; set; }

        [DataMember]
        public string CommandType { get; set; }

        [DataMember]
        public CommandSendStatus CommandStatus { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(RuleName)))
            {
                builder.AppendLine("RuleName:" + RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreID)))
            {
                builder.AppendLine("MdvrCoreID:" + MdvrCoreID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SendTime)))
            {
                builder.AppendLine("SendTime:" + SendTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CommandType)))
            {
                builder.AppendLine("CommandType:" + CommandType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CommandStatus)))
            {
                builder.AppendLine("CommandStatus:" + CommandStatus.ToString());
            }
            return builder.ToString();
        }

    }

    public class CurrentSettingRuleInfoHelper
    {
        public decimal ObjectID { get; set; }

        public string Name { get; set; }
    }
}

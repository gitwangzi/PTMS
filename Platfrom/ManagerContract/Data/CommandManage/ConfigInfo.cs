/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f9d3a055-b95c-431e-959f-8dc8cd0a381f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: ConfigInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/20 13:38:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/20 13:38:39
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
    [DataContract]
   public class ConfigInfo
    {
        ///<summary>
        ///VehicleID
        ///</summary>
        [DataMember]
        public string VehicleID;
        
        ///<summary>
        ///SuiteID
        ///</summary>
        [DataMember]
        public string SuiteID;

        ///<summary>
        ///MDVRID
        ///</summary>
        [DataMember]
        public string MDVRID;

        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_RuleName;

        ///<summary>
        ///Gps_RuleID
        ///</summary>
        [DataMember]
        public string Gps_RuleID;

        ///<summary>
        ///Alarm_RuleID
        ///</summary>
        [DataMember]
        public string Alarm_RuleID;

        ///<summary>
        ///Alarm_RuleName
        ///</summary>
        [DataMember]
        public string Alarm_RuleName;

        ///<summary>
        ///AbnormalDoor_RuleName
        ///</summary>
        [DataMember]
        public string AbnormalDoor_RuleName;

        ///<summary>
        ///AbnormalDoor_RuleID
        ///</summary>
        [DataMember]
        public string AbnormalDoor_RuleID;

        ///<summary>
        ///Temperature_RuleName
        ///</summary>
        [DataMember]
        public string Temperature_RuleName;

        ///<summary>
        ///Temperature_RuleID
        ///</summary>
        [DataMember]
        public string Temperature_RuleID;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteID)))
            {
                builder.AppendLine("SuiteID:" + SuiteID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVRID)))
            {
                builder.AppendLine("MDVRID:" + MDVRID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleName)))
            {
                builder.AppendLine("Gps_RuleName:" + Gps_RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleID)))
            {
                builder.AppendLine("Gps_RuleID:" + Gps_RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_RuleID)))
            {
                builder.AppendLine("Alarm_RuleID:" + Alarm_RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_RuleName)))
            {
                builder.AppendLine("Alarm_RuleName:" + Alarm_RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalDoor_RuleName)))
            {
                builder.AppendLine("AbnormalDoor_RuleName:" + AbnormalDoor_RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalDoor_RuleID)))
            {
                builder.AppendLine("AbnormalDoor_RuleID:" + AbnormalDoor_RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Temperature_RuleName)))
            {
                builder.AppendLine("Temperature_RuleName:" + Temperature_RuleName.ToString());
            }
            return builder.ToString();
        }

    }
}

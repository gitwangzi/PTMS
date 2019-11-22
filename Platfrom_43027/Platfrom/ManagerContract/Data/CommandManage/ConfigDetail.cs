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
/////             Class Name: ConfigDetail
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
    public class ConfigDetail
    {
        ///<summary>
        ///VehcileID
        ///</summary>
        [DataMember]
        public string VehcileID;
        ///<summary>
        ///Suite_ID
        ///</summary>
        [DataMember]
        public string Suite_ID;
        ///<summary>
        ///MDVR_ID
        ///</summary>
        [DataMember]
        public string MDVR_ID;
        ///<summary>
        ///Gps_RuleID
        ///</summary>
        [DataMember]
        public string Gps_RuleID;
        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_RuleName;
        ///<summary>
        ///Gps_IfMonitor
        ///</summary>
        [DataMember]
        public short? Gps_IfMonitor;
        ///<summary>
        ///Gps_UploadSum
        ///</summary>
        [DataMember]
        public int? Gps_UploadSum;
        ///<summary>
        ///Gps_UploadType
        ///</summary>
        [DataMember]
        public short? Gps_UploadType;
        ///<summary>
        ///Gps_CreateTime
        ///</summary>
        [DataMember]
        public DateTime? Gps_CreateTime;
        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_Creator;
        ///<summary>
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public string Gps_Description;
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public int? Gps_Distance;
        ///Gps_RuleName
        ///</summary>
        [DataMember]
        public int? Gps_Time;
        ///<summary>
        ///RuleID
        ///</summary>
        [DataMember]
        public string Alarm_RuleID;
        ///<summary>
        ///RuleName
        ///</summary>
        [DataMember]
        public string Alarm_RuleName;
        ///<summary>
        ///ButtonTime
        ///</summary>
        [DataMember]
        public short? Alarm_ButtonTime;
        ///<summary>
        ///OverSpeedAlarm
        ///</summary>
        [DataMember]
        public int? Alarm_OverSpeed;
        ///<summary>
        ///NormalAlarm
        ///</summary>
        [DataMember]
        public int? Alarm_Normal;
        ///<summary>
        ///Creator
        ///</summary>
        [DataMember]
        public string Alarm_Creator;
        ///<summmary>
        ///CreateTime
        ///</summary>
        [DataMember]
        public DateTime? Alarm_CreateTime;
        ///<summary>
        ///Description
        ///</summary>
        [DataMember]
        public string Alarm_Description;

        ///<summary>
        ///AbnormalDoor_ruleName
        ///</summary>
        [DataMember]
        public string AbnormalDoor_ruleName;

        ///<summary>
        ///AbnormalDoor_ruleID
        ///</summary>
        [DataMember]
        public string AbnormalDoor_ruleID;

        ///<summary>
        ///Temperature_ruleName
        ///</summary>
        [DataMember]
        public string Temperature_ruleName;

        ///<summary>
        ///Temperature_ruleID
        ///</summary>
        [DataMember]
        public string Temperature_ruleID;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(VehcileID)))
            {
                builder.AppendLine("VehcileID:" + VehcileID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Suite_ID)))
            {
                builder.AppendLine("Suite_ID:" + Suite_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MDVR_ID)))
            {
                builder.AppendLine("MDVR_ID:" + MDVR_ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleID)))
            {
                builder.AppendLine("Gps_RuleID:" + Gps_RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_RuleName)))
            {
                builder.AppendLine("Gps_RuleName:" + Gps_RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_IfMonitor)))
            {
                builder.AppendLine("Gps_IfMonitor:" + Gps_IfMonitor.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_UploadSum)))
            {
                builder.AppendLine("Gps_UploadSum:" + Gps_UploadSum.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_UploadType)))
            {
                builder.AppendLine("Gps_UploadType:" + Gps_UploadType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_CreateTime)))
            {
                builder.AppendLine("Gps_CreateTime:" + Gps_CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Creator)))
            {
                builder.AppendLine("Gps_Creator:" + Gps_Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Description)))
            {
                builder.AppendLine("Gps_Description:" + Gps_Description.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Distance)))
            {
                builder.AppendLine("Gps_Distance:" + Gps_Distance.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Gps_Time)))
            {
                builder.AppendLine("Gps_Time:" + Gps_Time.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_RuleID)))
            {
                builder.AppendLine("Alarm_RuleID:" + Alarm_RuleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_RuleName)))
            {
                builder.AppendLine("Alarm_RuleName:" + Alarm_RuleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_ButtonTime)))
            {
                builder.AppendLine("Alarm_ButtonTime:" + Alarm_ButtonTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_OverSpeed)))
            {
                builder.AppendLine("Alarm_OverSpeed:" + Alarm_OverSpeed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Normal)))
            {
                builder.AppendLine("Alarm_Normal:" + Alarm_Normal.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Creator)))
            {
                builder.AppendLine("Alarm_Creator:" + Alarm_Creator.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_CreateTime)))
            {
                builder.AppendLine("Alarm_CreateTime:" + Alarm_CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Description)))
            {
                builder.AppendLine("Alarm_Description:" + Alarm_Description.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalDoor_ruleName)))
            {
                builder.AppendLine("AbnormalDoor_ruleName:" + AbnormalDoor_ruleName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AbnormalDoor_ruleID)))
            {
                builder.AppendLine("AbnormalDoor_ruleID:" + AbnormalDoor_ruleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Temperature_ruleName)))
            {
                builder.AppendLine("Temperature_ruleName:" + Temperature_ruleName.ToString());
            }
            return builder.ToString();
        }

    }
}

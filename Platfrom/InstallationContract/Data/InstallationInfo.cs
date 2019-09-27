/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 157b9e12-4267-458a-a1be-565baff7fdf2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: InstallationInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:55:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 14:55:25
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Installation.Contract
{
    [DataContract]
    public class InstallationInfo
    {
        /// <summary>
        /// Installation unique number
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// License plate number
        /// </summary>
        [DataMember]
        public string VehicleID { get; set; }
        /// <summary>
        /// Car Type Name
        /// </summary>
        [DataMember]
        public string VehicleTypeName { get; set; }
        /// <summary>
        /// Security Device table's primary key
        /// </summary>
        [DataMember]
        public string DeviceKey { get; set; }
        /// <summary>
        /// Security Device SN No.
        /// </summary>
        [DataMember]
        public string DeviceSN { get; set; }
        /// <summary>
        /// MDVR chip number
        /// </summary>
        [DataMember]
        public string DeviceCoreId { get; set; }
        /// <summary>
        /// Installation Point Name
        /// </summary>
        [DataMember]
        public string InstallationStationName { get; set; }
        /// <summary>
        /// Installation point number
        /// </summary>
        [DataMember]
        public string InstallationStationId { get; set; }
        /// <summary>
        /// Installer
        /// </summary>
        [DataMember]
        public string InstallationStaff { get; set; }
        /// <summary>
        /// Record man
        /// </summary>
        [DataMember]
        public string RecordStaff { get; set; }
        /// <summary>
        /// Start the installation time
        /// </summary>
        [DataMember]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Installation Completion Date
        /// </summary>
        [DataMember]
        public DateTime? FinishTime { get; set; }
        /// <summary>
        /// Installation Instructions
        /// </summary>
        [DataMember]
        public String Note { get; set; }
        /// <summary>
        /// Completed step
        /// </summary>
        [DataMember]
        public int? CheckStep { get; set; }

        [DataMember]
        public string Organization { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleID)))
            {
                builder.AppendLine("VehicleID:" + VehicleID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleTypeName)))
            {
                builder.AppendLine("VehicleTypeName:" + VehicleTypeName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceKey)))
            {
                builder.AppendLine("SuiteKey:" + DeviceKey.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceSN)))
            {
                builder.AppendLine("SuiteID:" + DeviceSN.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + DeviceCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallationStationName)))
            {
                builder.AppendLine("InstallationStationName:" + InstallationStationName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallationStationId)))
            {
                builder.AppendLine("InstallationStationId:" + InstallationStationId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallationStaff)))
            {
                builder.AppendLine("InstallationStaff:" + InstallationStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RecordStaff)))
            {
                builder.AppendLine("RecordStaff:" + RecordStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CreateTime)))
            {
                builder.AppendLine("CreateTime:" + CreateTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(FinishTime)))
            {
                builder.AppendLine("FinishTime:" + FinishTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CheckStep)))
            {
                builder.AppendLine("CheckStep:" + CheckStep.ToString());
            }
            return builder.ToString();
        }

    }
}

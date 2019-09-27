/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ce1deb7f-caec-49a1-abb5-276c220bd4d0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: SecuritySuite
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 9:58:26
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 9:58:26
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Common.Enum;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    /// <summary>
    /// DeviceSuite
    /// </summary>
    [DataContract]
    public class DeviceSuite
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string Id;
        /// <summary>
        /// SuiteId
        /// </summary>
        [DataMember]
        public string SuiteId;
        /// <summary>
        /// MDVR SN
        /// </summary>
        [DataMember]
        public string MdvrCoreId;
        /// <summary>
        /// MDVR ID
        /// </summary>
        [DataMember]
        public string MdvrId;
        /// <summary>
        /// MDVR SIM ID
        /// </summary>
        [DataMember]
        public string MdvrSimId;
        /// <summary>
        /// Mdvr Sim PhoneNumber
        /// </summary>
        [DataMember]
        public string MdvrSimPhoneNumber;
        /// <summary>
        ///Device Type
        /// </summary>
        [DataMember]
        public VehicleTypeEnum DeviceType;
        /// <summary>
        /// First Camera Id
        /// </summary>
        [DataMember]
        public string Camera1Id;
        /// <summary>
        /// Second Camera Id
        /// </summary>
        [DataMember]
        public string Camera2Id;
        /// <summary>
        /// Third Camera ID
        /// </summary>
        [DataMember]
        public string Camera3Id;
        /// <summary>
        /// fourth camera id
        /// </summary>
        [DataMember]
        public string Camera4Id;
        /// <summary>
        /// first alarm button id
        /// </summary>
        [DataMember]
        public string AlarmButton1Id;
        /// <summary>
        /// second alarm button id
        /// </summary>
        [DataMember]
        public string AlarmButton2Id;
        /// <summary>
        /// thrid alarm button
        /// </summary>
        [DataMember]
        public string AlarmButton3Id;
        /// <summary>
        /// Ups Id
        /// </summary>
        [DataMember]
        public string UpsId;
        /// <summary>
        /// Sd Card Id
        /// </summary>
        [DataMember]
        public string SdCardId;
        /// <summary>
        /// Door Sensor Id
        /// </summary>
        [DataMember]
        public string DoorSensorId;
        /// <summary>
        /// Software Version
        /// </summary>
        [DataMember]
        public string SoftwareVersion;
        /// <summary>
        /// DeviceSuiteStatus
        /// </summary>
        [DataMember]
        public DeviceSuiteStatus status;
        /// <summary>
        /// Note
        /// </summary>
        [DataMember]
        public string Note;
        /// <summary>
        /// InstallStaff
        /// </summary>
        [DataMember]
        public string InstallStaff;
        /// <summary>
        /// InstallStatus
        /// </summary>
        [DataMember]
        public InstallStatusType InstallStatus = InstallStatusType.UnInstall;
        /// <summary>
        /// CheckStep
        /// </summary>
        public short? CheckStep;
        /// <summary>
        /// VehicleId
        /// </summary>
        [DataMember]
        public string VehicleId;
        /// <summary>
        /// UpdateFlag
        /// </summary>
        [DataMember]
        public bool UpdateFlag;

        //[DataMember]
        //public bool DeleteFlag;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrId)))
            {
                builder.AppendLine("MdvrId:" + MdvrId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrSimId)))
            {
                builder.AppendLine("MdvrSimId:" + MdvrSimId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrSimPhoneNumber)))
            {
                builder.AppendLine("MdvrSimPhoneNumber:" + MdvrSimPhoneNumber.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DeviceType)))
            {
                builder.AppendLine("DeviceType:" + DeviceType.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Camera1Id)))
            {
                builder.AppendLine("Camera1Id:" + Camera1Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Camera2Id)))
            {
                builder.AppendLine("Camera2Id:" + Camera2Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Camera3Id)))
            {
                builder.AppendLine("Camera3Id:" + Camera3Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Camera4Id)))
            {
                builder.AppendLine("Camera4Id:" + Camera4Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmButton1Id)))
            {
                builder.AppendLine("AlarmButton1Id:" + AlarmButton1Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmButton2Id)))
            {
                builder.AppendLine("AlarmButton2Id:" + AlarmButton2Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(AlarmButton3Id)))
            {
                builder.AppendLine("AlarmButton3Id:" + AlarmButton3Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UpsId)))
            {
                builder.AppendLine("UpsId:" + UpsId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SdCardId)))
            {
                builder.AppendLine("SdCardId:" + SdCardId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(DoorSensorId)))
            {
                builder.AppendLine("DoorSensorId:" + DoorSensorId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SoftwareVersion)))
            {
                builder.AppendLine("SoftwareVersion:" + SoftwareVersion.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(status)))
            {
                builder.AppendLine("status:" + status.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallStaff)))
            {
                builder.AppendLine("InstallStaff:" + InstallStaff.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InstallStatus)))
            {
                builder.AppendLine("InstallStatus :" + InstallStatus.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CheckStep)))
            {
                builder.AppendLine("CheckStep:" + CheckStep.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(UpdateFlag)))
            {
                builder.AppendLine("UpdateFlag:" + UpdateFlag.ToString());
            }
            //if (!string.IsNullOrEmpty(Convert.ToString(DeleteFlag)))
            //{
            //    builder.AppendLine("DeleteFlag:" + DeleteFlag.ToString());
            //}
            return builder.ToString();
        }
    }
}

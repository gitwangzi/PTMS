/////Copyright (C) Gsafety 2015 .All Rights Reserved.
/////======================================================================
/////                   Guid: 94674757-0a21-46c7-9971-124851be60bd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract.Data
/////    Project Description:    
/////             Class Name: SelfInspectDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2015/1/12 10:19:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2015/1/12 10:19:29
/////            Modified by:
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
    public class SelfInspectDetail
    {

        /// <summary>
        /// Self Test unique number
        /// </summary>
        [DataMember]
        public string SelfInspectID { get; set; }
        /// <summary>
        /// SD card recording
        /// </summary>
        [DataMember]
        public string RecSD { get; set; }
        /// <summary>
        /// GPS Self Test Information
        /// </summary>
        [DataMember]
        public string GpsInfo { get; set; }
        /// <summary>
        /// The current temperature inside the machine
        /// </summary>
        [DataMember]
        public string CurInTemperature { get; set; }
        /// <summary>
        /// SIM card status
        /// </summary>
        [DataMember]
        public string SimCard { get; set; }
        /// <summary>
        /// Input Voltage
        /// </summary>
        [DataMember]
        public string CurVoltage { get; set; }
        /// <summary>
        /// License plate number
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }
        /// <summary>
        /// Security Suite No.
        /// </summary>
        [DataMember]
        public string SuiteId { get; set; }
        /// <summary>
        /// Channel 1 video case
        /// </summary>
        [DataMember]
        public string Channel1 { get; set; }
        /// <summary>
        /// Channel 2 video case
        /// </summary>
        [DataMember]
        public string Channel2 { get; set; }
        /// <summary>
        /// Channel 3 video case
        /// </summary>
        [DataMember]
        public string Channel3 { get; set; }
        /// <summary>
        /// Channel 4 video case
        /// </summary>
        [DataMember]
        public string Channel4 { get; set; }
        /// <summary>
        /// State of emergency alarm button 1
        /// </summary>
        [DataMember]
        public string Sensor1 { get; set; }
        /// <summary>
        /// State of emergency alarm button 2
        /// </summary>
        [DataMember]
        public string Sensor2 { get; set; }
        /// <summary>
        /// State of emergency alarm button 3
        /// </summary>
        [DataMember]
        public string Sensor3 { get; set; }

        /// <summary>
        ///the voltage of back up power 
        /// </summary>
        [DataMember]
        public string StandbyPower { get; set; }

        /// <summary>
        ///SdCapacity
        /// </summary>
        [DataMember]
        public string SdCapacity { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(SelfInspectID)))
            {
                builder.AppendLine("SelfInspectID:" + SelfInspectID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RecSD)))
            {
                builder.AppendLine("RecSD:" + RecSD.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsInfo)))
            {
                builder.AppendLine("GpsInfo:" + GpsInfo.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CurInTemperature)))
            {
                builder.AppendLine("CurInTemperature:" + CurInTemperature.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SimCard)))
            {
                builder.AppendLine("SimCard:" + SimCard.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CurVoltage)))
            {
                builder.AppendLine("CurVoltage:" + CurVoltage.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VehicleId)))
            {
                builder.AppendLine("VehicleId:" + VehicleId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SuiteId)))
            {
                builder.AppendLine("SuiteId:" + SuiteId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel1)))
            {
                builder.AppendLine("Channel1:" + Channel1.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel2)))
            {
                builder.AppendLine("Channel2:" + Channel2.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel3)))
            {
                builder.AppendLine("Channel3:" + Channel3.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Channel4)))
            {
                builder.AppendLine("Channel4:" + Channel4.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Sensor1)))
            {
                builder.AppendLine("Sensor1:" + Sensor1.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Sensor2)))
            {
                builder.AppendLine("Sensor2:" + Sensor2.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Sensor3)))
            {
                builder.AppendLine("Sensor3:" + Sensor3.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(StandbyPower)))
            {
                builder.AppendLine("StandbyPower:" + StandbyPower.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SdCapacity)))
            {
                builder.AppendLine("SdCapacity:" + SdCapacity.ToString());
            }
            return builder.ToString();
        }

    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7f8eb1e8-4122-42fa-bd7f-e9f94379f929      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.Contract
/////    Project Description:    
/////             Class Name: SelfInspectInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/1 14:47:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 14:47:39
/////            Modified by: BilonLiu
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
    /// <summary>
    /// Inspect Information
    /// </summary>
    [DataContract]
    [Serializable]
    public class InspectInfo
    {
        /// <summary>
        /// Inspect ID
        /// </summary>
        [DataMember]
        public string InspectID { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        [DataMember] 
        public string Longitude { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        [DataMember] 
        public string Latitude { get; set; }

        /// <summary>
        /// Speed
        /// </summary>
        [DataMember] 
        public string Speed { get; set; }

        /// <summary>
        /// Note
        /// </summary>
        [DataMember] 
        public string Note { get; set; }

        /// <summary>
        /// Direction
        /// </summary>
        [DataMember] 
        public string Direction { get; set; }

        /// <summary>
        /// Gps Valid
        /// </summary>
        [DataMember] 
        public string GpsValid { get; set; }

        /// <summary>
        /// Gps Time
        /// </summary>
        [DataMember] 
        public Nullable<System.DateTime> GpsTime { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// MDVR Inspect Time(MDVR Device Time)_TIME_
        /// </summary>
        [DataMember] 
        public DateTime InspectTime { get; set; }

        /// <summary>
        /// Record SD Card
        /// </summary>
        [DataMember]
        public string RecSD { get; set; }

        /// <summary>
        /// Gps Information
        /// </summary>
        [DataMember]
        public string GpsInfo { get; set; }

        /// <summary>
        /// Sensor1 State
        /// </summary>
        [DataMember]
        public string Sensor1 { get; set; }

        /// <summary>
        /// Sensor2 State
        /// </summary>
        [DataMember]
        public string Sensor2 { get; set; }

        /// <summary>
        /// Sensor3 State
        /// </summary>
        [DataMember]
        public string Sensor3 { get; set; }

        /// <summary>
        /// Standby Power Voltage Number
        /// </summary>
        [DataMember]
        public string StandbyPower { get; set; }

        /// <summary>
        /// 3G Module  is On Work
        /// </summary>
        [DataMember]
        public string Module3G { get; set; }

        /// <summary>
        /// Channel1 Video State
        /// </summary>
        [DataMember]
        public string Channel1 { get; set; }

        /// <summary>
        /// Channel2 Video State
        /// </summary>
        [DataMember]
        public string Channel2 { get; set; }

        /// <summary>
        /// Channel3 Video State
        /// </summary>
        [DataMember]
        public string Channel3 { get; set; }

        /// <summary>
        /// Channel4 Video State
        /// </summary>
        [DataMember]
        public string Channel4 { get; set; }

        /// <summary>
        /// Current In Temperature
        /// </summary>
        [DataMember]
        public string CurInTemperature { get; set; }

        /// <summary>
        /// SIM Card State
        /// </summary>
        [DataMember]
        public string SimCard { get; set; }

        /// <summary>
        /// Current Voltage
        /// </summary>
        [DataMember]
        public string CurVoltage { get; set; }

        /// <summary>
        /// Sd Card Capacity
        /// </summary>
        [DataMember]
        public string SdCapacity { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(InspectID)))
            {
                builder.AppendLine("InspectID:" + InspectID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Longitude)))
            {
                builder.AppendLine("Longitude:" + Longitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Latitude)))
            {
                builder.AppendLine("Latitude:" + Latitude.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Speed)))
            {
                builder.AppendLine("Speed:" + Speed.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Note)))
            {
                builder.AppendLine("Note:" + Note.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Direction)))
            {
                builder.AppendLine("Direction:" + Direction.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsValid)))
            {
                builder.AppendLine("GpsValid:" + GpsValid.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsTime)))
            {
                builder.AppendLine("GpsTime:" + GpsTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(MdvrCoreId)))
            {
                builder.AppendLine("MdvrCoreId:" + MdvrCoreId.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(InspectTime)))
            {
                builder.AppendLine("InspectTime:" + InspectTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(RecSD)))
            {
                builder.AppendLine("RecSD:" + RecSD.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(GpsInfo)))
            {
                builder.AppendLine("GpsInfo:" + GpsInfo.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(Module3G)))
            {
                builder.AppendLine("Module3G:" + Module3G.ToString());
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
            if (!string.IsNullOrEmpty(Convert.ToString(SdCapacity)))
            {
                builder.AppendLine("SdCapacity:" + SdCapacity.ToString());
            }
            return builder.ToString();
        }

    }
}

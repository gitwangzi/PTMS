/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c6f6c3d9-1c47-4fe8-b44f-ef9710c3b1fc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Maintain
/////    Project Description:    
/////             Class Name: SuiteRunningDetail
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/31 11:04:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/31 11:04:18
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
    /// <summary>
    /// Basic Info
    /// </summary>
    [DataContract]
    [Serializable]
    public class BasicInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Vehicle Id
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Gps Time
        /// </summary>
        [DataMember]
        public DateTime GpsTime { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }
    }

    /// <summary>
    /// Enviroment
    /// </summary>
    [DataContract]
    [Serializable]
    public class Enviroment
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// Voltage
        /// </summary>
        [DataMember]
        public string Voltage { get; set; }

        /// <summary>
        /// Voltage Flag(0:Low;1:Normal;2:High)
        /// </summary>
        [DataMember]
        public Nullable<short> VoltageFlag { get; set; }

        /// <summary>
        /// Temperature In Machine
        /// </summary>
        [DataMember]
        public Nullable<decimal> TemperatureIn { get; set; }

        /// <summary>
        /// Temperature Out of Machine
        /// </summary>
        [DataMember]
        public Nullable<decimal> TemperatureOut { get; set; }

        /// <summary>
        /// The  Flag of Temperature In Machine(0:Low;1:Normal;2:High)
        /// </summary>
        [DataMember]
        public Nullable<short> TemperatureInFlag { get; set; }

        /// <summary>
        /// The  Flag of Temperature Out of Machine(0:Low;1:Normal;2:High)
        /// </summary>
        [DataMember]
        public Nullable<short> TemperatureOutFlag { get; set; }

        /// <summary>
        /// Car Key State ,ACC Key State,Two State: ON And OFF
        /// </summary>
        [DataMember]
        public string AccStatus { get; set; }

        /// <summary>
        /// Battery Status E/D/C/L/F"Not Exist","Discharge","Incharge","Study" Error
        /// </summary>
        [DataMember]
        public string BatteryStatus { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// GpsTime
        /// </summary>
        [DataMember]
        public DateTime GpsTime { get; set; }
    }

    /// <summary>
    /// Hardware
    /// </summary>
    [DataContract]
    [Serializable]
    public class Hardware
    {
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// SD Card  STAT_N/E/U,is Not Exist/Exist/Not Formatting
        /// </summary>
        [DataMember]
        public string RecsdStatus { get; set; }

        /// <summary>
        /// Memery Card Error ,Hard Disk Full Y/N , is Yes/ No
        /// </summary>
        [DataMember]
        public string RecsdFull { get; set; }

        /// <summary>
        /// Read /Write Error,Y/N is Yes/ No
        /// </summary>
        [DataMember]
        public string RecsdWrError { get; set; }

        /// <summary>
        /// N/A/V, is Not Exist / Signal Valid / Signal Invalid
        /// </summary>
        [DataMember]
        public string GpsValid { get; set; }

        /// <summary>
        ///Signal Persistent Weak ,Y/N 
        /// </summary>
        [DataMember]
        public string GpsAntenna { get; set; }

        /// <summary>
        /// GPS poor contact Y/N
        /// </summary>
        [DataMember]
        public string GpsPoorcnt { get; set; }

        /// <summary>
        /// Camera1 R/N
        /// </summary>
        [DataMember]
        public string Camera1Status { get; set; }

        /// <summary>
        /// Camera2
        /// </summary>
        [DataMember]
        public string Camera2Status { get; set; }

        /// <summary>
        /// State Y/N
        /// Camera1
        /// </summary>
        [DataMember]
        public string Camera1Recstat { get; set; }

        /// <summary>
        /// State
        /// Camera2
        /// </summary>
        [DataMember]
        public string Camera2Recstat { get; set; }

        /// <summary>
        /// RecordStatus(All)OK/Fail
        /// </summary>
        [DataMember]
        public string RecordStatus { get; set; }

        /// <summary>
        /// Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// GpsTime
        /// </summary>
        [DataMember]
        public DateTime GpsTime { get; set; }
    }
}

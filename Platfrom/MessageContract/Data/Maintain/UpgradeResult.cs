/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 296e6d6f-a3ff-45d2-a203-b235845f51e6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: UpgradeResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/28 16:36:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/28 16:36:36
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
    [DataContract]
    [Serializable]
    public class UpgradeResult
    {
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
        /// xml Context
        /// </summary>
        [DataMember]
        public string Context { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }

        /// <summary>
        /// UpdateResult
        /// </summary>
        [DataMember]
        public string UpdateResult { get; set; }

        /// <summary>
        /// Current Ver
        /// </summary>
        [DataMember]
        public string CurFiremareVer { get; set; }

        /// <summary>
        /// Last Ver
        /// </summary>
        [DataMember]
        public string LasTFiremareVer { get; set; }

        /// <summary>
        /// Last Update Time
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> LastFiremareUpdateTime { get; set; }

        /// <summary>
        /// The Start Time of Update  
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> UpdateRebootTime { get; set; }

        /// <summary>
        /// The End Time of Update  
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> UpdateEndTime { get; set; }

        /// <summary>
        /// Error Number
        /// </summary>
        [DataMember]
        public string ErrorNumber { get; set; }
    }
}

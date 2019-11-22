/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 71e3bb3d-109e-4aa7-b96f-3f58bf8350bf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: GPS
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:12:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:12:00
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Alarm.Contract.Data
{
    [DataContract]
    public class GPS
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string MdvrCoreId { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public Nullable<DateTime> GpsTime { get; set; }

        /// <summary>
        /// car NO
        /// </summary>
        [DataMember]
        public string VehicleId { get; set; }

        [DataMember]
        public string GpsValid { get; set; }

        [DataMember]
        public int Source { get; set; }

    }
}

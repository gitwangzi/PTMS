/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 46dd5a17-59b4-442e-91c7-f60d6b7f0d2e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Monitor.Contract.Data
/////    Project Description:    
/////             Class Name: SnapshotReturn
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/24 10:48:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/24 10:48:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Monitor.Contract.Data
{
     [DataContract]
    public class VehicleGPS
    {

         [DataMember]
         public VehicleTypeEnum Type { get; set; }

         [DataMember]
         public string Longitude { get; set; }

         [DataMember]
         public string Latitude { get; set; }

         public VehicleGPS()
         {
         }

         public VehicleGPS(VehicleTypeEnum type, string longitude, string latitude)
         {
             Type = type;
             Longitude = longitude;
             Latitude = latitude;
         }
    }
}

/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0fa00214-d90e-43f4-b202-6f69d7798c1b      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: EndLocationMonitor
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/16 04:26:04
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/16 04:26:04
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
    [Serializable]
    [DataContract]
    public class EndLocationMonitor
    {

        [DataMember]
        public string VechileID { get; set; }

        [DataMember]
        public LocationMonitorEndType EndType { get; set; }
    }
}

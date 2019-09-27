/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6a3464de-8fe7-44c2-9869-38eb4a88fef1      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: CancelLocationMonitorCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/12 23:54:07
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/12 23:54:07
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
    public class CancelLocationMonitorCMD
    {
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public string VechileID { get; set; }

        [DataMember]
        public string UniqueID { get; set; }

        //[DataMember]
        //public string RaptorGpsID { get; set; }
    }
}

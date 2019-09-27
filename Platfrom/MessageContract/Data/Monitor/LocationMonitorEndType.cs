/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 5de86ab4-3b63-4d1b-8c3e-388d62ee737f      
/////             clrversion: 4.0.30319.34014
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Monitor
/////    Project Description:    
/////             Class Name: LocationMonitorEndType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/4/16 04:31:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/4/16 04:31:15
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
    public enum LocationMonitorEndType
    {
        [EnumMember]
        RequestFails = 0x01,
        [EnumMember]
        RequestTimeout = 0x02,
        [EnumMember]
        MonitorEnd =0x03,
    }
}

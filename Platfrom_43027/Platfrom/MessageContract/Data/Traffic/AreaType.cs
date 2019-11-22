/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e93cf817-b8e0-4f94-a967-099de2a25b23      
/////             clrversion: 4.0.30319.34003
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: AreaType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/13 05:21:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/13 05:21:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Message.Contract.Data
{
    [Serializable]
    [DataContract]
    public enum AreaType
    {
        [EnumMember]
        ElectronicFence = 0x01,
        [EnumMember]
        MonitoringPoint = 0x02,
    }
}

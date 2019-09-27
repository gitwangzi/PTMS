/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 69881f5d-71a7-4f41-bf33-68d86496484b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: TemperatureMarkType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 15:02:40
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 15:02:40
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
    public enum TemperatureMarkType
    {
        [EnumMember]
        Forbid,
        [EnumMember]
        AfterSendUp,
        [EnumMember]
        Original
    }
}

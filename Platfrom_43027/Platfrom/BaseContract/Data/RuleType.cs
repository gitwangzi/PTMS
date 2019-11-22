/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 64894d82-5d66-4426-9cef-7ae46fefa6e2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Contract.Data
/////    Project Description:    
/////             Class Name: RuleType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/5 15:47:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/5 15:47:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    [Serializable]
    public enum RuleType
    {
        [EnumMember]
        Fence,
        [EnumMember]
        OverSpeed,
        [EnumMember]
        OneKeyAlarm,
        [EnumMember]
        AbnormalDoor,
        [EnumMember]
        TemperatureAlert,
        [EnumMember]
        GpsSend
    }
}

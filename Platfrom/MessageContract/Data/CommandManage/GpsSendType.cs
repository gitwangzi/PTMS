/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: dbfaf5e0-3d69-4edd-b51f-cc669feec72a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: GpsSendType
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 14:36:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 14:36:48
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
    public enum GpsSendType
    {
        [EnumMember]
        Distance,
        [EnumMember]
        Time,
        [EnumMember]
        DistanceAndTime
    }
}

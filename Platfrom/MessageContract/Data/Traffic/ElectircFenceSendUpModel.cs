using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1f43e923-192c-4467-a3f6-783b4a0ca238      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: ElectircFenceSettingModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/27 11:12:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/27 11:12:48
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
    public class ElectircFenceSendSettingModel : BaseSettingModel
    {
        [DataMember]
        public ElectricFenceCMD Setting { get; set; }

        [DataMember]
        public ElectricFenceOperType ElectricFenceOperation { get; set; }
    }
}

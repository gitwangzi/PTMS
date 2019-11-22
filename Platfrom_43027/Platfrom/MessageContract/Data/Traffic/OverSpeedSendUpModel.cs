using Gsafety.PTMS.Base.Contract.Data;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0bd1b771-2a29-498e-a013-5845e4bbd6ee      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: OverSpeedSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/27 11:25:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/27 11:25:58
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
    public class OverSpeedSendSettingModel : BaseSettingModel
    {
        [DataMember]
        public SettingOverSpeedCMD Setting { get; set; }
    }
}

/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: f52b196f-29cb-4429-95ff-c8805d9cd3de      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: TemperatureSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 17:13:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 17:13:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Base.Contract.Data;

namespace Gsafety.PTMS.Message.Contract.Data
{
    [Serializable]
    [DataContract]
    public class TemperatureSendUpModel : BaseSettingModel
    {
        [DataMember]
        public SettingTemperatureCMD Setting;
    }
}

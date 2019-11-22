/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 62b9b8cd-c747-4624-80ae-d4f84f73cb83      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: OneKeyAlarmSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 17:16:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 17:16:44
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
    public class OneKeyAlarmSendUpModel : BaseSettingModel
    {
        [DataMember]
        public SettingOneKeyAlarmCMD Setting;

        [DataMember]
        public SettingDelayAlarmCMD DelayAlarmSetting;
    }
}

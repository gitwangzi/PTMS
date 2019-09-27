/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: db3254e3-d5d3-47d3-b056-d29c21dcf94b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: AbnormalDoorSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/31 17:18:08
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/31 17:18:08
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
    public class AbnormalDoorSendUpModel : BaseSettingModel
    {
        [DataMember]
        public SettingAbnormalDoorCMD Setting;
    }
}

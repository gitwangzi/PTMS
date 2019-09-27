/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a5ed82c2-370f-4e14-9ccd-7f8cae086d05      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: SettingOverSpeedModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/25 13:58:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/25 13:58:01
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
    [DataContract]
    [Serializable]
    public class SettingOverSpeedModel : BaseSettingModel
    {
        [DataMember]
        public SettingOverSpeedCMD Setting;
    }
}

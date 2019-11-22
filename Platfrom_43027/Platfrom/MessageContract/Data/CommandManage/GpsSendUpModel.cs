/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: e0316b1f-3845-4237-ad59-e119ff1ecbaf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: GpsSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/28 14:35:05
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/28 14:35:05
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
    public class GpsSendUpModel : BaseSettingModel
    {
        /// <summary>
        /// Gps Setting Model
        /// </summary>
        [DataMember]
        public SettingGpsSendUpCMD Setting;
    }
}

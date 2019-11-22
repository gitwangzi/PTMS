using Gsafety.PTMS.Common.Enum;
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 00b8dfc7-871e-4e81-8313-e06df2d364cf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.CommandManage
/////    Project Description:    
/////             Class Name: BaseSendUpModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/25 13:49:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/25 13:49:14
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
    [Serializable]
    [DataContract]
    public class BaseSettingModel
    {
        [DataMember]
        public List<SelectInfoModel> Value;
        [DataMember]
        public RuleOperationType OperationType { get; set; }
    }
}

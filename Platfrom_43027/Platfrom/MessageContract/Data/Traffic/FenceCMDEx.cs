/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2805eea4-72b6-4851-b1e5-474eededa6d8      
/////             clrversion: 4.0.30319.34011
/////Registered organization: 
/////           Machine Name: DENGZL
/////                 Author: DENGZL(dzl)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: FenceCMDEx
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/2/21 04:35:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/2/21 04:35:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Ant.Message.Contract
{
    /// <summary>
    /// 电子围栏，Monitor alert需要的扩展信息
    /// </summary>
    [Serializable]
    [DataContract]
    public class FenceCMDExpandInfo
    {
        [DataMember]
        public string VehicleID { get; set; }
        [DataMember]
        public string RaptorID { get; set; }
        [DataMember]
        public string CenterPoint { get; set; }
        [DataMember]
        public decimal Radius { get; set; }
    }
}

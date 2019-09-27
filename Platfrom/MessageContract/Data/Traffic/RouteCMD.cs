/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 53965707-d75a-4813-8d01-2778410243ab      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGZG
/////                 Author: TEST(zhangzg)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Message.Contract.Data.Traffic
/////    Project Description:    
/////             Class Name: RouteCMD
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/5 11:01:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/5 11:01:02
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gsafety.PTMS.Base.Contract.Data;



namespace Gsafety.PTMS.Message.Contract.Data
{
    [Serializable]
    [DataContract]
    public class RouteCMD : DownwardBase
    {
        /// <summary>
        /// Route ID
        /// </summary>
        [DataMember]
        public string RouteId { get; set; }


        /// <summary>
        /// Operation Type
        /// ASCII Code 0-9
        /// 1:Add Route,2:Modify Route,3:Delete Route
        /// </summary>
        [DataMember]
        public int OperType { get; set; }

        /// <summary>
        /// Route Name
        /// </summary>
        [DataMember]
        public string RouteName { get; set; }

        /// <summary>
        /// Route
        /// </summary>
        [DataMember]
        public string Route { get; set; }

        /// <summary>
        /// Mdvr Core Id
        /// </summary>
        [DataMember]
        public string MdvrCoreId { get; set; }
    }
}

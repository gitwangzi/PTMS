/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9a4066fe-a88a-4885-9568-85b87ec49893      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: VisitLogInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:35:59 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/27/2013 10:35:59 AM
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    /// <summary>
    /// VisitLogInfo
    /// </summary>
    [DataContract]
    public class VisitLogInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// Visitor
        /// </summary>
        [DataMember]
        public string Visitor { get; set; }
        /// <summary>
        /// VisiterContent
        /// </summary>
        [DataMember]
        public string VisiterContent { get; set; }
        /// <summary>
        /// VisitTime
        /// </summary>
        [DataMember]
        public DateTime VisitTime { get; set; }
        /// <summary>
        /// CONTENT_TYPE
        /// </summary>
        [DataMember]
        public int? CONTENT_TYPE { get; set; }
        /// <summary>
        /// TargetVihcle
        /// </summary>
        [DataMember]
        public string TargetVihcle { get; set; }
        /// <summary>
        /// OrgCode
        /// </summary>
        [DataMember]
        public string OrgCode { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Id)))
            {
                builder.AppendLine("Id:" + Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Visitor)))
            {
                builder.AppendLine("Visitor:" + Visitor.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VisiterContent)))
            {
                builder.AppendLine("VisiterContent:" + VisiterContent.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(VisitTime)))
            {
                builder.AppendLine("VisitTime:" + VisitTime.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(CONTENT_TYPE)))
            {
                builder.AppendLine("CONTENT_TYPE:" + CONTENT_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(TargetVihcle)))
            {
                builder.AppendLine("TargetVihcle:" + TargetVihcle.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(OrgCode)))
            {
                builder.AppendLine("OrgCode:" + OrgCode.ToString());
            }
            return builder.ToString();
        }

    }
}

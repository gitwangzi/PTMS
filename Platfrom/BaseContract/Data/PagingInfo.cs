/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6049bae5-0310-41f1-ba30-c8175696cb2a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Base.Entity
/////    Project Description:    
/////             Class Name: PagingInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/31/2013 10:05:46 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/31/2013 10:05:46 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.Base.Contract.Data
{
    [DataContract]
    public class PagingInfo
    {
        /// <summary>
        /// The current page's size
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        /// <summary>
        /// The request page's index
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(PageSize)))
            {
                builder.AppendLine("PageSize:" + PageSize.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PageIndex)))
            {
                builder.AppendLine("PageIndex:" + PageIndex.ToString());
            }
            return builder.ToString();
        }
    }
}

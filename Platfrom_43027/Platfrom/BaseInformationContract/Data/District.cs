/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 42f24b58-f437-4519-8584-7b3e1c9608bc      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Contract.Data
/////    Project Description:    
/////             Class Name: IDistrict
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 17:37:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/22 17:37:10
/////            Modified by: BilongLiu
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    [DataContract]
    public class District
    {
        /// <summary>
        /// District Code
        /// </summary>
        [DataMember]
        public string Code { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public string Name;
        /// <summary>
        /// ParentName
        /// </summary>
        [DataMember]
        public string ParentName;
        /// <summary>
        /// ParentCode
        /// </summary>
        [DataMember]
        public string ParentCode;

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Code)))
            {
                builder.AppendLine("Code:" + Code.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Name)))
            {
                builder.AppendLine("Name:" + Name.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ParentName)))
            {
                builder.AppendLine("ParentName:" + ParentName.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(ParentCode)))
            {
                builder.AppendLine("ParentCode:" + ParentCode.ToString());
            }

            return builder.ToString();
        }
    }
}

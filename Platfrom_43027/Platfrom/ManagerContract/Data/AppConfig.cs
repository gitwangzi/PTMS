/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9abc0402-9525-4792-a328-35e4bd47ef22      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Contract.Data
/////    Project Description:    
/////             Class Name: AppConfig
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 10:19:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 10:19:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Manager.Contract.Data
{
    [DataContract]
    public class AppConfig
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string SECTION_NAME { get; set; }
        [DataMember]
        public string SECTION_DESC { get; set; }
        [DataMember]
        public string SECTION_VALUE { get; set; }
        [DataMember]
        public string SECTION_TYPE { get; set; }
        [DataMember]
        public string PARENT { get; set; }
        [DataMember]
        public string SECTION_LEVEL { get; set; }
        [DataMember]
        public string SECTION_UNIT { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(ID)))
            {
                builder.AppendLine("ID:" + ID.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_NAME)))
            {
                builder.AppendLine("SECTION_NAME:" + SECTION_NAME.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_DESC)))
            {
                builder.AppendLine("SECTION_DESC:" + SECTION_DESC.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_VALUE)))
            {
                builder.AppendLine("SECTION_VALUE:" + SECTION_VALUE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_TYPE)))
            {
                builder.AppendLine("SECTION_TYPE:" + SECTION_TYPE.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PARENT)))
            {
                builder.AppendLine("PARENT:" + PARENT.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_LEVEL)))
            {
                builder.AppendLine("SECTION_LEVEL:" + SECTION_LEVEL.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_UNIT)))
            {
                builder.AppendLine("SECTION_UNIT:" + SECTION_UNIT.ToString());
            }
            return builder.ToString();
        }

    }
}

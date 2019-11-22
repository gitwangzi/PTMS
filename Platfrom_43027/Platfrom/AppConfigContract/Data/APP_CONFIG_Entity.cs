/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ebe8e57b-7f1d-449c-b241-a8ae8d3d0a45      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGJINCAI
/////                 Author: TEST(JinCaiWang)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Contract.Data
/////    Project Description:    
/////             Class Name: AlarmInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/8 11:07:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/8 11:07:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AppConfig.Contract.Data
{
    [DataContract]
    public class APP_CONFIG_Entity
    {
        public APP_CONFIG_Entity()
        {
            this.ID = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Section Name
        /// </summary>
        [DataMember]
        public string SECTION_NAME { get; set; }

        /// <summary>
        /// Section Description
        /// </summary>
        [DataMember]
        public string SECTION_DESC { get; set; }

        /// <summary>
        /// Section Value
        /// </summary>
        [DataMember]
        public string SECTION_VALUE { get; set; }
        
        /// <summary>
        /// Section Type
        /// </summary>
        [DataMember]
        public string SECTION_TYPE { get; set; }

        /// <summary>
        /// Section Level
        /// </summary>
        [DataMember]
        public string SECTION_LEVEL { get; set; }

        /// <summary>
        /// Parent
        /// </summary>
        [DataMember]
        public string PARENT { get; set; }

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
            if (!string.IsNullOrEmpty(Convert.ToString(SECTION_LEVEL)))
            {
                builder.AppendLine("SECTION_LEVEL:" + SECTION_LEVEL.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(PARENT)))
            {
                builder.AppendLine("PARENT:" + PARENT.ToString());
            }
            return builder.ToString();
        }

    }
}

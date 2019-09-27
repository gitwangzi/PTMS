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
    public class AppConfigModel
    {

        public AppConfigModel()
        {
            this.Children = new List<AppConfigModel>();
        }

        /// <summary>
        /// Config Value
        /// </summary>
        [DataMember]
        public APP_CONFIG_Entity Value { get; set; }

        /// <summary>
        /// Child Point
        /// </summary>
        [DataMember]
        public List<AppConfigModel> Children { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Value)))
            {
                builder.AppendLine("Value:" + Value.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Children)))
            {
                builder.AppendLine("Children:" + Children.ToString());
            }
            return builder.ToString();
        }

    }
}

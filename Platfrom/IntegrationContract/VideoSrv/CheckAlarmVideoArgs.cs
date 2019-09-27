/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0e737d55-f154-4a6a-8569-cbb71a47aacf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: CheckAlarmVideoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 16:01:34
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 16:01:34
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Contract
{
    [DataContract]
    public class CheckAlarmVideoArgs
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [DataMember]
        public string Mdvr_Id { get; set; }

        /// <summary>
        /// 用于确定某某时间10秒左右的一次报警
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }
        /// <summary>
        /// 报警ID
        /// </summary>
        [DataMember]
        public string Alarm_Id { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(Mdvr_Id)))
            {
                builder.AppendLine("Mdvr_Id:" + Mdvr_Id.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Date)))
            {
                builder.AppendLine("Date:" + Date.ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(Alarm_Id)))
            {
                builder.AppendLine("Alarm_Id:" + Alarm_Id.ToString());
            }
            return builder.ToString();
        }

    }
}

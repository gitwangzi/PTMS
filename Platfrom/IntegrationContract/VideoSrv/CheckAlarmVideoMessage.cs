/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e1c009bd-0e8f-4289-a3ec-8ee82aacc6e1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: CheckAlarmVideoMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 16:04:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 16:04:29
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
    public class CheckAlarmVideoMessage
    {
        /// <summary>
        /// ͨ��1�ļ�URL�б�
        /// </summary>
        [DataMember]
        public string  Channel1 { get; set; }
        /// <summary>
        /// ͨ��1��15���ļ�URL�б�
        /// </summary>
        [DataMember]
        public string Channel1_15 { get; set; }
        /// <summary>
        /// ͨ��2���ļ��б�
        /// </summary>
        [DataMember]
        public string Channel2 { get; set; }
        /// <summary>
        /// ͨ��2 ��15���ļ�URL�б�
        /// </summary>
        [DataMember]
        public string Channel2_15 { get; set; }

    }
}

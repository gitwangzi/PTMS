/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e1487629-9f38-4f0a-99cd-e669f2de5ca7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: CheckAlarmVideo_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:08:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:08:11
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Repository
{
    public class CheckAlarmVideo_dtoResult:VideoMessage
    {
        public CheckAlarmVideo_dtoResult()
            : base()
        {

        }
        /// <summary>
        /// ͨ��1�ļ�URL�б�
        /// </summary>

        public string  channel1 { get; set; }
        /// <summary>
        /// ͨ��1��15���ļ�URL�б�
        /// </summary>

        public string  channel1_15 { get; set; }
        /// <summary>
        /// ͨ��2���ļ��б�
        /// </summary>

        public string  channel2 { get; set; }
        /// <summary>
        /// ͨ��2 ��15���ļ�URL�б�
        /// </summary>

        public string  channel2_15 { get; set; }
    }
}

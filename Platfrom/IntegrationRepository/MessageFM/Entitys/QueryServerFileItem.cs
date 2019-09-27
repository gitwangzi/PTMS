/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 15c92417-84bf-4909-9b58-00bc3bc7996e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: QueryServerFileItem
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:52:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:52:16
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
   public  class QueryServerFileItem
    {
        /// <summary>
        ///  文件唯一编号URL（由捷诺产生，以后根据该编号进行文件下载）
        /// </summary>

        public string server_file_id { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>

        public string server_file_size { get; set; }
        /// <summary>
        /// 文件对应视频开始时间
        /// </summary>

        public string start_time { get; set; }
        /// <summary>
        /// 文件对应视频结束时间
        /// </summary>

        public string end_time { get; set; }
        /// <summary>
        /// 播放时间（单位：秒）
        /// </summary>

        public string duration_time { get; set; }

        /// <summary>
        /// 通道
        /// </summary>
        public string channel { get; set; }
    }
}

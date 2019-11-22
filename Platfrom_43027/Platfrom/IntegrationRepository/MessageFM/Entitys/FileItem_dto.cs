/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 74527670-9306-48b2-b556-2ee479013a4b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: FileItem
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:26:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:26:37
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Integration.Repository
{
   
    public class FileItem_dto
    {
        /// <summary>
        /// 文件唯一编号（由捷诺产生，以后根据该编号进行文件下载）
        /// </summary>
       
        public string mdvr_file_id { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
       
        public string mdvr_file_size { get; set; }
        /// <summary>
        /// 是否已经下载到服务器
        /// </summary>
        
        public string download_flag { get; set; }
        /// <summary>
        /// 文件对应视频开始时间
        /// </summary>
        
        public string start_time { get; set; }
        /// <summary>
        /// 文件对应视频结束时间
        /// </summary>
       
        public string end_time { get; set; }

        /// <summary>
        /// 通道号
        /// </summary>
        public string channel { get; set; }
    }
}

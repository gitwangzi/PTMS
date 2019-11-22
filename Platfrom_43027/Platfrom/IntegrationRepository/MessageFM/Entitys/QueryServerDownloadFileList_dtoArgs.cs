/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4bae5058-87a4-48a3-8e93-be38459aa81f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: QueryServerDownloadFileList_dtoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-10 14:22:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-10 14:22:44
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
    public class QueryServerDownloadFileList_dtoArgs : VideoArgs
    {
        public QueryServerDownloadFileList_dtoArgs()
            : base()
        {
           
        }

        /// <summary>
        /// 方法名称
        /// </summary>

        public string method { get { return "QueryServerDownloadFileList"; } set { } }        
         
        /// <summary>
        /// 查询起始时间，范围包括此时间。如：2013-01-01 01:01:01
        /// </summary>

        public string start_time { get; set; }
        /// <summary>
        /// 下载的截止时间，范围包括此时间。如：2013-01-02 01:01:01
        /// </summary>

        public string end_time { get; set; }

        /// <summary>
        /// 下载状态
        /// </summary>
        public string download_status { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public string page_size{get;set;}

        /// <summary>
        /// 当前页，从1开始
        /// </summary>
        public string page_num { get; set; }

        public override int out_time
        {
            get
            {
                return 10;
            }
            set
            {

            }
        }

    }
}

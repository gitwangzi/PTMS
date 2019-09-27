/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a499054e-e60e-47f4-bdb9-bbd7fb2fe202      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryServerFileListArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 10:50:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 10:50:41
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
   
    public class QueryServerFileList_dtoArgs : VideoArgs
    {
        public QueryServerFileList_dtoArgs()
            : base()
        {
        }
        /// <summary>
        /// 方法名称
        /// </summary>
    
        public string method { get { return "QueryServerFileList"; } set { } }
       

        /// <summary>
        /// 1:第一路视频，2:第二路视频；
        /// </summary>
        
        public string channel { get; set; }

        /// <summary>
        /// 搜索方式，1：常规录像,2：报警录像,3：所有录像
        /// </summary>
            
        public string video_type { get; set; }

        /// <summary>
        /// 查询起始时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>
        
        public string start_time { get; set; }

        /// <summary>
        /// 下载的截止时间，范围包括此时间。如：2013-01-01 01:01:01;
        /// </summary>        
        
        public string end_time { get; set; }


        /// <summary>
        /// 分页大小
        /// </summary>
        public string page_size { get; set; }

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
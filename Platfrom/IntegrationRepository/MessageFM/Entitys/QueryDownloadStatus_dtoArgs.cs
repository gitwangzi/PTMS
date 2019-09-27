/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3fdf0219-c267-41a5-a48f-e6eef6f0a6f7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryDownloadStatusArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:53:03
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:53:03
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
    
    public class QueryDownloadStatus_dtoArgs:VideoArgs 
    {
        public QueryDownloadStatus_dtoArgs()
            : base()
        {

        }
        /// <summary>
        /// 方法名称
        /// </summary>
       
        public string method { get { return "QueryDownloadStatus"; } set { } }

        /// <summary>
        ///  文件唯一编号
        /// </summary>
       
        public string mdvr_file_id { get; set; }
        /// <summary>
        /// 2：按照时间偏移量下载
        /// </summary>

        public string offset_flag { get { return "2"; } set { } }
        /// <summary>
        /// 下载文件的开始偏移时间
        /// </summary>
       
        public string offset_starttime { get; set; }

        /// <summary>
        /// 下载文件的结束偏移时间
        /// </summary>
       
        public string offset_endtime { get; set; }

        public override int out_time
        {
            get
            {
                return 60;
            }
            set
            {
                 
            }
        }

    }
}
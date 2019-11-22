/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 2892f5a0-bfe5-43e3-be36-e03910fa6c97      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryMdvrFileListMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:21:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:21:56
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

    public class QueryMdvrFileList_dtoResult : VideoMessage, IMulData
    {
        public QueryMdvrFileList_dtoResult()
            : base()
        {

        }
        /// <summary>
        /// 错误码
        /// </summary>

        public string error_code { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>

        public string error_message { get; set; }

        /// <summary>
        /// 所有文件数
        /// </summary>

        public string all_file_count { get; set; }
        /// <summary>
        /// 当前文件数
        /// </summary>

        public string now_file_count { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileItem_dto> files { get; set; }


        public override T FromJsonString<T>(List<string> jsonStr)
        {
            var tem = base.FromJsonString<T>(jsonStr) as QueryMdvrFileList_dtoResult;
            if (tem.files != null && this.files != null)
            {
                tem.files.AddRange(this.files);
            }
            tem.IsCompleted = tem.all_file_count == tem.now_file_count;
            if (!tem.IsCompleted)
            {
                if (tem.files != null)
                {
                    tem.IsCompleted = tem.all_file_count == tem.files.Count.ToString();
                }
                else
                {
                    tem.IsCompleted = true;
                }
            }

            return tem as T;
        }

    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c114b945-f3d9-43b5-bfc7-74df2ce48919      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: QueryServerDownloadFileList_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-10 14:26:15
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-10 14:26:15
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
    public class QueryServerDownloadFileList_dtoResult : VideoMessage, IMulData
    {
        public QueryServerDownloadFileList_dtoResult()
            : base()
        {

        }

        public string total_file_count { get; set; }
        public string all_file_count { get; set; }
        public string now_file_count { get; set; }

        public List<QueryFileDownLoadItem> files { get; set; }



        public override T FromJsonString<T>(List<string> jsonStr)
        {
            var tem = base.FromJsonString<T>(jsonStr) as QueryServerDownloadFileList_dtoResult;
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

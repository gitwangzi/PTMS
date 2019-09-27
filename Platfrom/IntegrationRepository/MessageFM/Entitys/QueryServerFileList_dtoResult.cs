/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6f934fbe-c283-4d78-b222-977a188299c3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryServerFileListMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 10:58:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 10:58:39
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

    public class QueryServerFileList_dtoResult : VideoMessage, IMulData
    {
        public QueryServerFileList_dtoResult()
            : base()
        {
        }

        public string total_file_count { get; set; }
        public string all_file_count { get; set; }
        public string now_file_count { get; set; }

        public List<QueryServerFileItem> files { get; set; }


        public override T FromJsonString<T>(List<string> jsonStr)
        {
            var tem = base.FromJsonString<T>(jsonStr) as QueryServerFileList_dtoResult;
            if (jsonStr.Count > 1)
            {
                jsonStr.RemoveAt(0);
                for (int i = 0; i < jsonStr.Count; i++)
                {
                    var xx = base.FromJsonString<T>(jsonStr) as QueryServerFileList_dtoResult;
                    tem.files.AddRange(xx.files);
                    jsonStr.RemoveAt(i);
                }
            }
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

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: cd999339-75bc-4d6f-855b-5e5e7833d89c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: QueryDownloadStatus_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 16:55:44
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 16:55:44
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
    public class QueryDownloadStatus_dtoResult : VideoMessage
    {
        public QueryDownloadStatus_dtoResult()
            : base()
        {

        }

        public string status { get; set; }
        public string percent { get; set; }
        public int result { get; set; }
        public string url { get; set; }
        public string duration_time { get; set; }

        public string channel { get; set; }
    }
}

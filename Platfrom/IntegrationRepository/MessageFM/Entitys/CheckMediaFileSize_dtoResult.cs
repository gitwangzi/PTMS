/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: aedd1410-ef75-45e7-93d0-2cb11fee1cf3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: CheckMediaFileSize_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:04:12
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:04:12
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
    public class CheckMediaFileSize_dtoResult : VideoMessage
    {
        public CheckMediaFileSize_dtoResult()
            : base()
        {
        }

        public string stream_file_id { get; set; }
        public string size { get; set; }
        public string start_time { get; set; }
        public string duration_time { get; set; }
    }
}

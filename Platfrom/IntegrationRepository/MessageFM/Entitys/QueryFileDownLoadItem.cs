/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 39c52ac2-8382-40f3-be91-0b1e3f775c21      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: QueryFileDownLoadItem
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-10-15 10:40:47
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-10-15 10:40:47
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
    public class QueryFileDownLoadItem : QueryServerFileItem
    {
        public string offset_flag { get; set; }
        public string offset_starttime { get; set; }
        public string offset_endtime { get; set; }
        public string mdvr_file_id { get; set; }
        public string download_status { get; set; }
        public string download_percent { get; set; }

    }
}

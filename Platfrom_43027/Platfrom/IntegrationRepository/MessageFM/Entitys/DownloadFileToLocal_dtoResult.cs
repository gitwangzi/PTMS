/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 685249b1-ac07-453d-a699-101c7fdcc1ce      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: DownloadFileToLocal_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:00:59
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:00:59
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
    public class DownloadFileToLocal_dtoResult:VideoMessage
    {
        public DownloadFileToLocal_dtoResult()
            : base()
        {
        }
        public string url { get; set; }
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 771191d4-98a4-48d0-a3ff-8e29fac9f3d4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: DownloadFileToLocal_dtoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 16:57:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 16:57:49
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
    public class DownloadFileToLocal_dtoArgs : VideoArgs
    {
        public DownloadFileToLocal_dtoArgs()
            : base()
        {

        }

        public string method { get { return "DownloadFileToLocal"; } set { } }

        public string mdvr_file_id { get; set; }
        public string offset_flag { get { return "2"; } set { } }
        public string offset_start_time { get; set; }
        public string offset_end_time { get; set; }

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

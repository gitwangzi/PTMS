/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 314c28eb-da98-43a7-a0ad-e50a35ea044e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: CheckMediaFileSize_dtoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:02:30
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:02:30
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
    public class CheckMediaFileSize_dtoArgs : VideoArgs 
    {
        public CheckMediaFileSize_dtoArgs()
            : base()
        {
        }
        public string method { get { return "CheckMediaFileSize"; } set { } }

        public string date { get; set; }

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

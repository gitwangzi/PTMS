/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 64613b38-f998-45bc-86c4-f6bb0b6e0d24      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: VideoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-08-31 14:11:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-08-31 14:11:35
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
    
    public class VideoArgs
    {
        public VideoArgs()
        {
            msg_id = Guid.NewGuid().ToString();
        }


        /// <summary>
        /// MDVRID
        /// </summary>
        public string mdvr_id { get; set; }

        /// <summary>
        /// ÏûÏ¢ID
        /// </summary>
       
        public string msg_id { get; set; }
         
        public virtual  int   out_time { get; set; }

        public string user_name { get; set; }

        public string fileName { get; set; }
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ea843f93-1fe8-4d49-9c84-20e7d4284795      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM
/////    Project Description:    
/////             Class Name: IMulData
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-11 14:45:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-11 14:45:18
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
   public  interface IMulData
    {
        /// <summary>
        /// 所有文件数
        /// </summary>
        string all_file_count { get; set; }
        /// <summary>
        /// 当前文件数
        /// </summary>

        string now_file_count { get; set; }

    }
}

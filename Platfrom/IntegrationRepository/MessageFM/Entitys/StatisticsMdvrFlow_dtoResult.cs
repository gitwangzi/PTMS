/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 22cfe8dc-38d9-4466-b40f-ab8e88107462      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: StatisticsMdvrFlow_dtoResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-11-04 18:43:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-11-04 18:43:58
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
    public class StatisticsMdvrFlow_dtoResult : VideoMessage
    {
        public StatisticsMdvrFlow_dtoResult()
            : base()
        {

        }

        public string mdvr_id { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string flow_all { get; set; }
        public string flow_common { get; set; }
        public string flow_alarm { get; set; }
        public string flow_download { get; set; }
    }
}

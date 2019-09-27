/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 97321e38-de8c-4353-a888-038b15e1ebe3      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: StatisticsMdvrFlow_dtoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-11-04 18:40:45
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-11-04 18:40:45
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
    public class StatisticsMdvrFlow_dtoArgs : VideoArgs
    {
        public StatisticsMdvrFlow_dtoArgs()
            : base()
        { }

        public string method { get { return "StatisticsMdvrFlow"; } set { } }
        public string start_time { get; set; }
        public string end_time { get; set; }
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

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4328746e-82c4-4dcc-af2a-807eff2478dd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository.MessageFM.Entitys
/////    Project Description:    
/////             Class Name: CheckAlarmVideo_dtoArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 17:06:11
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 17:06:11
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
    public class CheckAlarmVideo_dtoArgs : VideoArgs
    {
        public CheckAlarmVideo_dtoArgs()
            : base()
        {

        }
        public string method { get { return "CheckAlarmVideo"; } set { } }

        public string date { get; set; }
        public string alarm_id { get; set; }

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
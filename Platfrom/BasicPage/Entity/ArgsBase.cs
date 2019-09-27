/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 70c47189-a6ef-4022-8d35-7ee2686c41bf      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: ArgsBase
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-04 15:01:13
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-04 15:01:13
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.IO;
using System.Linq;

namespace Gsafety.PTMS.Video.Args
{
    public abstract class ArgsBase
    {

        public double  Width { get; set; }


        public double  Height { get; set; }

        /// <summary>
        /// ChannelId£º0,1
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// MDVRID
        /// </summary>
        public string MdvrId { get; set; }


        public bool IsAutoPlay { get; set; }

        public bool IsVideoWall { get; set; }

        public string Key { get; set; }


        public string UserName { get; set; }


        public string CarNo { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool Ifdivmenu
        {
            get;
            set;
        }
 
    }
}

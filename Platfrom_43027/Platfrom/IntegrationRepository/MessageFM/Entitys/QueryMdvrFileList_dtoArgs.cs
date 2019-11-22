/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1eff9c43-59e3-4113-b29c-95b4b7112b5f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: QueryMdvrFileListArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:07:58
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:07:58
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
   
    public class QueryMdvrFileList_dtoArgs : VideoArgs
    {
        public QueryMdvrFileList_dtoArgs()
            : base()
        {

        }
        /// <summary>
        /// ��������
        /// </summary>
        
        public string method { get { return "QueryMdvrFileList"; } set { } }
        /// <summary>
        /// 1:��һ·��Ƶ��2:�ڶ�·��Ƶ
        /// </summary>
       
        public string channel { get; set; }
        /// <summary>
        /// ������ʽ:1������¼��2������¼��3������¼��
        /// </summary>
        
        public string video_type { get; set; }

        /// <summary>
        /// ��ѯ��ʼʱ�䣬��Χ������ʱ�䡣�磺2013-01-01 01:01:01
        /// </summary>
        
        public string start_time { get; set; }
        /// <summary>
        /// ���صĽ�ֹʱ�䣬��Χ������ʱ�䡣�磺2013-01-02 01:01:01
        /// </summary>
       
        public string end_time { get; set; }

        public override int out_time
        {
            get
            {
                return 60;
            }
            set
            {

            }
        }
    }
}

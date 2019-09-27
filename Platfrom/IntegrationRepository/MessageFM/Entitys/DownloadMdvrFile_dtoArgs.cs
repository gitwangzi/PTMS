/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 21f30e64-a2da-4189-9d4b-21b22203e243      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: DownloadMdvrFileArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:36:01
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:36:01
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
    
    public class DownloadMdvrFile_dtoArgs : VideoArgs
    {
        public DownloadMdvrFile_dtoArgs()
            : base()
        {
        }
        /// <summary>
        /// ��������
        /// </summary>
       
        public string method { get { return "DownloadMdvrFile"; } set { } }

        /// <summary>
        /// �ļ�Ψһ���
        /// </summary>
       
        public string mdvr_file_id { get; set; }
        /// <summary>
        /// ����ʱ��ƫ��������
        /// </summary>
       
        public string offset_flag { get { return "2"; } set { } }
        /// <summary>
        /// �����ļ��Ŀ�ʼƫ��ʱ�� ��λ���룩
        /// </summary>
        
        public string offset_starttime { get; set; }
        /// <summary>
        /// �����ļ��Ľ���ƫ��ʱ�� ��λ���룩
        /// </summary>
       
        public string offset_endtime { get; set; }
        /// <summary>
        /// �����ļ��Ŀ�ʼʱ�� 2013-08-29 12:13:14 (Ϊ�˼���WKP)
        /// </summary>
        
        public string download_starttime { get; set; }
        /// <summary>
        /// �����ļ��Ŀ�ʼʱ�� 2013-08-29 12:15:16 (Ϊ�˼���WKP)
        /// </summary>
        
        public string download_endtime { get; set; }
        /// <summary>
        /// 0����ʼ����  1��ֹͣ����
        /// </summary>
        public string download_flag { get; set; }


        /// <summary>
        /// ͨ��
        /// </summary>
        public string channel { get; set; }


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

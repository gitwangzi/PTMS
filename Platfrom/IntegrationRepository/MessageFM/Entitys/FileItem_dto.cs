/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 74527670-9306-48b2-b556-2ee479013a4b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Contract.VideoSrv
/////    Project Description:    
/////             Class Name: FileItem
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-02 11:26:37
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-02 11:26:37
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
   
    public class FileItem_dto
    {
        /// <summary>
        /// �ļ�Ψһ��ţ��ɽ�ŵ�������Ժ���ݸñ�Ž����ļ����أ�
        /// </summary>
       
        public string mdvr_file_id { get; set; }
        /// <summary>
        /// �ļ���С
        /// </summary>
       
        public string mdvr_file_size { get; set; }
        /// <summary>
        /// �Ƿ��Ѿ����ص�������
        /// </summary>
        
        public string download_flag { get; set; }
        /// <summary>
        /// �ļ���Ӧ��Ƶ��ʼʱ��
        /// </summary>
        
        public string start_time { get; set; }
        /// <summary>
        /// �ļ���Ӧ��Ƶ����ʱ��
        /// </summary>
       
        public string end_time { get; set; }

        /// <summary>
        /// ͨ����
        /// </summary>
        public string channel { get; set; }
    }
}

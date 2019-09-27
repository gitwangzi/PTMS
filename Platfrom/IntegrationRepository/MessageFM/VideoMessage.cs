/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: dc788bdd-c24b-43a0-895c-aec2734c1352      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: VideoMessage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-08-31 13:53:00
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-08-31 13:53:00
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
    
    public class VideoMessage
    {
        private JsonMessageSerializer _ser;

        public virtual  bool IsCompleted { get; set; }
        public VideoMessage()
        {
            _ser = new JsonMessageSerializer();
            ErrorCode = -1;
        }

        /// <summary>
        /// 消息ID
        /// </summary>
        
        public string msg_id { get; set; }


        /// <summary>
        /// 1:视频服务器连接中断
        /// </summary>
        public int ErrorCode { get; set; }

        public virtual  T FromJsonString<T>(List<string> jsonStr) where T:VideoMessage,new ()
        {
            var result = _ser.FromJsonString<T>(jsonStr[0]);
            if (result.msg_id != msg_id)
            {
                throw new Exception("The JsonStr ID:{0}!=CurrID:{1}");
            }
            result.IsCompleted = true;
            return result;
        }
    }
}

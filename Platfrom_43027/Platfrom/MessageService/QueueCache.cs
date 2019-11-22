/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ed7e7f5c-eae5-4ca9-b922-2cfeec3ba5c5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Ant.Message
/////    Project Description:    
/////             Class Name: QueueCache
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/18 14:58:38
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/18 14:58:38
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gsafety.Ant.Message
{
    public static class QueueCache
    {
        private static Dictionary<string, DateTime> _queueCache;

        static QueueCache()
        {
            _queueCache = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="time"></param>
        public static void Add(string queue, DateTime time)
        {
            if (_queueCache.ContainsKey(queue))
            {
                _queueCache.Remove(queue);
            }
            _queueCache.Add(queue, DateTime.Now);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (_queueCache.ContainsKey(key))
            {
                _queueCache.Remove(key);
            }
        }

        /// <summary>
        /// 全部数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, DateTime> GetQueueCache()
        {
            return _queueCache;
        }
    }
}
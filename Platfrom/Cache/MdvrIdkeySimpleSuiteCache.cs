/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e931cc0c-4a12-4df5-ae2e-5141c6986eb9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Cache
/////    Project Description:    
/////             Class Name: WorkingSuiteCache
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 14:42:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 14:42:20
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.Message.Contract.Data;

namespace Gsafety.PTMS.Analysis.Cache
{
    public static class MdvrIdkeySimpleSuiteCache
    {
        private static Hashtable _htSuitInfo;

        static MdvrIdkeySimpleSuiteCache()
        {
            _htSuitInfo = Hashtable.Synchronized(new Hashtable());
        }

        public static void BatchAdd(List<SimpleSuiteInfo> suiteInfo)
        {
            foreach (SimpleSuiteInfo item in suiteInfo)
            {
                if (!string.IsNullOrEmpty(item.MdvrCoreId))
                {
                    if (_htSuitInfo.ContainsKey(item.MdvrCoreId))
                    {
                        _htSuitInfo.Remove(item.MdvrCoreId);
                    }
                    _htSuitInfo.Add(item.MdvrCoreId, item);
                }
            }
        }

        public static SimpleSuiteInfo GetValue(string key)
        {
            SimpleSuiteInfo sSuiteInfo = null;

            if (_htSuitInfo != null && _htSuitInfo.Count > 0)
            {
                if (_htSuitInfo.ContainsKey(key))
                {
                    sSuiteInfo = _htSuitInfo[key] as SimpleSuiteInfo;
                }
            }
            return sSuiteInfo;
        }

        public static void Add(SimpleSuiteInfo item)
        {
            if (!string.IsNullOrEmpty(item.MdvrCoreId))
            {
                if (_htSuitInfo.ContainsKey(item.MdvrCoreId))
                {
                    _htSuitInfo.Remove(item.MdvrCoreId);
                }
                _htSuitInfo.Add(item.MdvrCoreId, item);
            }
        }

        public static void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (_htSuitInfo.ContainsKey(key))
                {
                    _htSuitInfo.Remove(key);
                }
            }
        }
    }
}

using Gsafety.PTMS.Message.Contract.Data;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7e58ae1e-3dce-4d02-a081-06bdeb7fc8ad      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SoftwareUpgrade
/////    Project Description:    
/////             Class Name: UpgradingCache
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/7 10:51:02
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/7 10:51:02
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SoftwareUpgrade
{
    public static class UpgradingCache
    {

        private static Hashtable _htUpgradingInfo;

        static UpgradingCache()
        {
            _htUpgradingInfo = new Hashtable();
        }

        public static int Count()
        {
            if (_htUpgradingInfo == null)
                return 0;
            return _htUpgradingInfo.Count;
        }

        public static void Add(UpgradeCMD item)
        {
            if (!string.IsNullOrEmpty(item.DvId))
            {
                if (_htUpgradingInfo.ContainsKey(item.DvId))
                {
                    _htUpgradingInfo.Remove(item.DvId);
                }
                _htUpgradingInfo.Add(item.DvId, item);
            }
        }

        public static void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                if (_htUpgradingInfo.ContainsKey(key))
                {
                    _htUpgradingInfo.Remove(key);
                }
            }
        }
    }
}

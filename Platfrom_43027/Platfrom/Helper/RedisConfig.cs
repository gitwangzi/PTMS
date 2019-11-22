/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: e62763d2-679d-4953-8fe4-1ddf07306522      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: RedisHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/9/5 10:34:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/9/5 10:34:36
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Gsafety.PTMS.Analysis.Helper
{
    public sealed class RedisConfig:ConfigurationSection 
    {
        public static RedisConfig GetConfig()
        {
            RedisConfig section = (RedisConfig)ConfigurationManager.GetSection("RedisConfig");
            return section;
        }

        [ConfigurationProperty("WriteServerList",IsRequired=false)]
        public string WriteServerList
        {
            get { return (string)base["WriteServerList"]; }
            set { base["WriteServerList"] = value; }
        }

        [ConfigurationProperty("ReadServerList", IsRequired = false)]
        public string ReadServerList
        {
            get { return (string)base["ReadServerList"]; }
            set { base["ReadServerList"] = value; }
        }

        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false,DefaultValue=5)]
        public int MaxWritePoolSize
        {
            get 
            {
                int _maxWritePoolSize= (int)base["MaxWritePoolSize"];
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            }
            set { base["MaxWritePoolSize"] = value; }
        }

        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxReadPoolSize
        {
            get
            {
                int _maxReadPoolSize = (int)base["MaxReadPoolSize"];
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            }
            set { base["MaxReadPoolSize"] = value; }
        }

        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get { return (bool)base["AutoStart"]; }
            set { base["AutoStart"] = value; }
        }

        [ConfigurationProperty("RecordLog", IsRequired = false, DefaultValue = false)]
        public bool RecordLog
        {
            get { return (bool)base["RecordLog"]; }
            set { base["RecordLog"] = value; }
        }
    }
}

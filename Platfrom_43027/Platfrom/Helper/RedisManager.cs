/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 361ae151-8389-4b41-844a-e647096b05cd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: RedisManager
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/9/5 11:51:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/9/5 11:51:16
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;

namespace Gsafety.PTMS.Analysis.Helper
{
    public class RedisManager<T> where T : class
    {
        private RedisConfig redisConfig = RedisConfig.GetConfig();
        private PooledRedisClientManager prcm;

        public RedisManager()
        {
            CreateManager();
        }

        private void CreateManager()
        {
            string[] writeServerList = SplitString(redisConfig.WriteServerList, ",");
            string[] readServerList = SplitString(redisConfig.ReadServerList, ",");
            prcm = new PooledRedisClientManager(readServerList, writeServerList,
                    new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = redisConfig.MaxWritePoolSize,
                        MaxReadPoolSize = redisConfig.MaxReadPoolSize,
                        AutoStart = redisConfig.AutoStart,
                    }
                );
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }

        #region string 512M（redis3.0)
        public bool Item_Set(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Set<T>(key, t);
            }
        }

        public T Item_Get(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        public bool Item_Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }

        #endregion

        #region List 2*32-1
        public void List_Add(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                //RedisTypedClient<T> rtc = new RedisTypedClient<T>(rc);
                IRedisList<T> irl = rtc.Lists[key];
                rtc.AddItemToList(irl, t);
            }
        }

        public bool List_Remove(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                IRedisList<T> irl = rtc.Lists[key];
                return rtc.RemoveItemFromList(irl, t) > 0;
            }
        }

        public void List_RemoveAll(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                rtc.Lists[key].RemoveAll();
            }
        }

        public long List_Count(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.GetListCount(key);
            }
        }

        public List<T> List_GetRANGE(string key, int start, int count)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                return rtc.Lists[key].GetRange(start, start + count - 1);
            }
        }

        public List<T> List_GetList(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                return rtc.Lists[key].GetRange(0, rtc.Lists[key].Count);
            }
        }

        public List<T> List_GetList(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRANGE(key, start, pageSize);
        }

        #endregion

        #region Set 2*32-1，
        public void Set_Add(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                rtc.Sets[key].Add(t);
            }
        }

        public bool Set_Contains(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                return rtc.Sets[key].Contains(t);
            }
        }

        public bool Set_Remove(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                IRedisTypedClient<T> rtc = redis.As<T>();
                return rtc.Sets[key].Remove(t);
            }
        }
        #endregion

        #region Hash 2*32-1

        public bool Hash_Add(string key, string dataKey, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHashIfNotExists(key, dataKey, value);
            }
        }

        public bool Hash_Remove(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveEntryFromHash(key, dataKey);
            }
        }

        public bool Hash_RemoveAll(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }

        public bool Hash_Set(string key, string dataKey, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHash(key, dataKey, value);
            }
        }

        public void Hash_SetList(string key, Dictionary<string, T> dc)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var value = dc.ToList().ConvertAll(x => new KeyValuePair<string, string>(x.Key, ServiceStack.Text.JsonSerializer.SerializeToString<T>(x.Value)));
                redis.SetRangeInHash(key, value);
            }
        }

        public T Hash_Get(string key, string dataKey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                if (value != null)
                {
                    return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
                }
                else
                {
                    return null;
                }
            }
        }

        public List<T> Hash_GetAll(string key)
        {
            IRedisClient redis = prcm.GetClient();

            try
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
            finally
            {
                redis.Dispose();
            }

        }

        public bool Hash_Exist(string key, string datakey)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.HashContainsEntry(key, datakey);
            }
        }

        #endregion

        #region
        public bool SortedSet_Add(string key, T t, double score)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.AddItemToSortedSet(key, value, score);
            }
        }

        public bool SortedSet_Remove(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }

        public long SortedSet_Trim(string key, int size)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.RemoveRangeFromSortedSet(key, size, 9999999);
            }
        }

        public long SortedSet_Count(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.GetSortedSetCount(key);
            }
        }

        public List<T> SortedSet_GetList(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetRangeFromSortedList(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }

        public List<T> SortedSet_GetListAll(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var list = redis.GetAllItemsFromSortedSet(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }

        #endregion

        #region
        public void SetExpireAt(string key, DateTime dateTime)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryAt(key, dateTime);
            }
        }

        public void SetExpireIn(string key, TimeSpan timeSpan)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                redis.ExpireEntryIn(key, timeSpan);
            }
        }

        #endregion
    }
}

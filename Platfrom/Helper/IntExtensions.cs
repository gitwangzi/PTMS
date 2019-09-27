/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 839f1109-582b-43ca-add7-9d83223b3112      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: $userdomain
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis.Helper
/////    Project Description:    
/////             Class Name: IntExtensions
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/16 14:18:46
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/16 14:18:46
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    public static class IntExtensions
    {
        public static IEnumerable<int> Times(this int times)
        {
            int iteratorVarialble0 = 0;
            while (true)
            {
                if (iteratorVarialble0 >= times)
                {
                    yield break;
                }
                yield return iteratorVarialble0;
                iteratorVarialble0=(int) (iteratorVarialble0+1);
            }
        }

        public static void Times(this int times, Action actionFn)
        {
            for (int i = 0; i < times; i = (int)(i + 1))
            {
                actionFn();
            }
        }

        public static void Times(this int times,Action<int> actionFn)
        {
            for (int i = 0; i < times; i = (int)(i + 1))
            {
                actionFn(i);
            }
        }

        public static List<T> Times<T>(this int times, Func<T> actionFn)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < times; i = (int)i + 1)
            {
                list.Add(actionFn());
            }
            return list;
        }

        public static List<T> Times<T>(this int times, Func<int, T> actionFn)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < times; i = (int)i + 1)
            {
                list.Add(actionFn(i));
            }
            return list;
        }

        public static List<IAsyncResult> TimesAsync(this int times, Action actionFn)
        {
            List<IAsyncResult> list = new List<IAsyncResult>(times);
            for (int i = 0; i < times; i = (int)i + 1)
            {
                list.Add(actionFn.BeginInvoke(null,null));
            }
            return list;
        }

        public static List<IAsyncResult> TimesAsync(this int times, Action<int> actionFn)
        {
            List<IAsyncResult> list = new List<IAsyncResult>(times);
            for (int i = 0; i < times;i=(int)(i+1))
            {
                list.Add(actionFn.BeginInvoke(i,null,null));
            }
            return list;
        }
    }
}

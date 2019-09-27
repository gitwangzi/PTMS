/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 45360f52-ff36-4cb4-988f-6d86eecfd5ff      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Video
/////    Project Description:    
/////             Class Name: MethodHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/12/15 10:43:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/12/15 10:43:54
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Video
{
    public class MethodHelper
    {
        /// <summary>
        /// Get all the business methods
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MethodInfo> GetMethodInfo()
        {
            var messageProcessing = new MessageProcessing();
            var methodInfo = messageProcessing.GetType().GetMethods()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(BusinessAttribute)))
                .ToDictionary(x =>
                {
                    var att = x.GetCustomAttributes(typeof(BusinessAttribute), false)[0] as BusinessAttribute;
                    return att.TypeName;
                },
                    y => y
                );
            return methodInfo;
        }
    }
}

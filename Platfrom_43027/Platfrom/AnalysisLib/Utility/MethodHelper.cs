using Gsafety.Common.Logging;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 78501e64-fd16-4069-a6ee-611745ce902d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Analysis
/////    Project Description:    
/////             Class Name: BusinessMethodHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 14:52:41
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/5 14:52:41
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib
{
    public static class MethodHelper
    {
        /// <summary>
        /// get all business method
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MethodInfo> GetShareMethodInfo()
        {
            var businessProcess = new ShareBusinessProcess();
            var methodInfo = businessProcess.GetType().GetMethods()
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

        /// <summary>
        /// get all business method
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, MethodInfo> GetPrivateMethodInfo()
        {
            var businessProcess = new PrivateBusinessProcess();
            var methodInfo = businessProcess.GetType().GetMethods()
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

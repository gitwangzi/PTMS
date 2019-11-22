using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.CommandManagement
{
    public static class MethodHelper
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

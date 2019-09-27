using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.Common.Logging;
using System.Reflection;

namespace OnOffLineManagement
{
    public static class MethodHelper
    {
        public static Dictionary<string, MethodInfo> GetMethodInfo()
        {
            var businessProcess = new DealWithProcess();
            var methodInfo = businessProcess.GetType().GetMethods()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DealWithAttribute)))
                .ToDictionary(x =>
                {
                    var att = x.GetCustomAttributes(typeof(DealWithAttribute), false)[0] as DealWithAttribute;
                    return att.TypeName;
                },
                    y => y
                );
            return methodInfo;
        }
    }
}

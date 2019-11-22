using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    public static class RouteHelper
    {
        public static Hashtable GetAlertRouteInfo()
        {
            Hashtable h = new Hashtable();
            var route = new UserMessageRoute();
            route.GetType().GetFields()
                 .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(RouteAttribute))).ToList().ForEach(x =>
                 {
                     var att = x.GetCustomAttributes(typeof(RouteAttribute), false)[0] as RouteAttribute;
                     h.Add(att.TypeName, x.GetValue(route).ToString());
                 });
            return h;
        }
    }
}

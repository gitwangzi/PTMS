using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Repository
{
    public static class ExtendHelper
    {
        public static T ToConvert<T>(this object obj) where T : class,new()
        {
            var result = default(T);
            var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
            var str = ser.Serialize(obj);

            result = ser.Deserialize<T>(str);

            return result;
        }
         
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.Util
{
    public class JsonHelper
    {
        /// <summary>
        /// object to json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(object obj)
        {

            var result = "";
            try
            {
                var ser = new DataContractJsonSerializer(obj.GetType());
                using (var ms = new System.IO.MemoryStream())
                {
                    ser.WriteObject(ms, obj);
                    result = System.Text.Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
                }
            }
            catch (Exception exp)
            {
                //log

            }
            //  result = result.Trim('\"').Replace("\\", "");
            return result;
        }

        /// <summary>
        ///json to object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T FromJsonString<T>(string jsonStr)
        {
            T result = default(T);
            object temp = FromJsonString(typeof(T), jsonStr);
            if (temp != null)
            {
                result = (T)temp;
            }

            return result;
        }

        /// <summary>
        /// json to object
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static object FromJsonString(Type targetType, string jsonStr)
        {
            object result = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
                    var ser = new DataContractJsonSerializer(targetType);
                    var buffer = Encoding.UTF8.GetBytes(jsonStr);
                    using (var ms = new System.IO.MemoryStream(buffer))
                    {

                        result = ser.ReadObject(ms);
                    }
                }
            }
            catch (Exception exp)
            {

            }

            return result;
        }
    }
}

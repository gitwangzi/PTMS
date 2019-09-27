/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 91a757ff-d38c-4829-80b9-d2603cab5114      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Utilities
/////    Project Description:    
/////             Class Name: JsonHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 8/27/2013 3:40:18 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/27/2013 3:40:18 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Text;

namespace Gsafety.PTMS.Report.Repository
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

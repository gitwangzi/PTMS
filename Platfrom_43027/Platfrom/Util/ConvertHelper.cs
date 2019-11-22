/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9f3cd0e6-8e80-4313-a8f8-f9d1aeec61c7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.Common.Util
/////    Project Description:    
/////             Class Name: ConvertHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 10:37:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/31 15:40:29
/////            Modified by: TEST(guoh)
/////   Modified Description: 将十六进制字符串转换为二进制字符串
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Gsafety.Common.Util
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 将Object转换为byte[]数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 将byte[]数组转换为Object
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static object BytesToObject(byte[] bytes)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                IFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 字符串转换为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ConvertJsonToModel<T>(string jsonStr)
        {
            var jss = new JavaScriptSerializer();
            return jss.Deserialize<T>(jsonStr);
        }

        /// <summary>
        /// 实体转换为字符串JSON
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string ConvertModelToJson(object model)
        {
            var jss = new JavaScriptSerializer();
            return jss.Serialize(model);
        }

        /// <summary>
        /// List转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        private static DataTable ToDateTable<T>(IEnumerable<T> collection)
        {
            var pros = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(pros.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in pros)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? ConvertStrToDate(string dateStr, string format)
        {
            try
            {
                return DateTime.ParseExact(dateStr, format, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 将十六进制字符串转换为二进制字符串
        /// 2013-08-31 author by guoh
        /// </summary>
        /// <param name="originalString">十六进制字符串</param>
        /// <returns>二进制字符串</returns>
        public static string ConvertHexadecimalToBinary(string originalString)
        {
            string result = "";
            try
            {
                if (originalString != null && originalString.Trim().Length > 0)
                {
                    // 将十六进制字符串转换为二进制字符串
                    string strBinary = Convert.ToString(Convert.ToInt32(originalString, 16), 2);
                    //判断二进制字符串长度是否为十六进制字符串长度的4倍，
                    //若长度不够将在二进制字符串前端补"0"
                    if (strBinary != null && strBinary.Trim().Length > 0 && strBinary.Length < originalString.Length * 4)
                    {
                        int count = originalString.Length * 4 - strBinary.Length;
                        for (int i = 0; i < count; i++)
                        {
                            result += "0";
                        }
                        result += strBinary;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now + ":" + ex.ToString());
            }
            return result;
        }

        public static DateTime DateTimeToUTC(DateTime date)
        {
            switch (date.Kind)
            {
                case DateTimeKind.Utc:
                    return date;
                case DateTimeKind.Local:
                    return date.ToUniversalTime();
                default:
                    return date;
            }
        }

        public static DateTime DateTimeNow()
        {
            return DateTime.UtcNow;
        }
    }
}

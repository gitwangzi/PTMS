/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 111457e4-e61b-461d-8a41-85e4612723c5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Integration.Repository
/////    Project Description:    
/////             Class Name: JsonMessageSerializer
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-08-31 11:57:22
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-08-31 11:57:22
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Gsafety.PTMS.Integration.Repository
{
    public class JsonMessageSerializer
    {
        /// <summary>
        /// 编码格式
        /// </summary>
        public System.Text.Encoding Encoding { get; set; }

        private readonly JavaScriptSerializer _ser = new JavaScriptSerializer();

     

        /// <summary>
        /// 构造函数
        /// </summary>
        ///<param name="encod">编码格式</param>
        public JsonMessageSerializer(System.Text.Encoding encod)
        {
            this.Encoding = encod;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public JsonMessageSerializer()
            : this(System.Text.Encoding.UTF8)
        {
        }

        /// <summary>
        /// 反序列化消息
        /// </summary>
        /// <param name="serializedMessage">
        /// </param>
        /// <returns>
        ///  
        /// </returns>
        public List<string> Deserialize(List<byte> serializedMessage, out bool isPacket)
        {
            List<string> result = new List<string>();
            isPacket = serializedMessage.Count > 4 && Encoding.GetString(serializedMessage.ToArray(), 0, 4) == "99de";
            if (isPacket)
            {
                result = UnWarpMsg(serializedMessage);
            }
            return result;

        }


        private List<string> UnWarpMsg(List<byte> serializedMessage)
        {
            var result = new List<string>();

            do
            {
                if (serializedMessage.Count > 8)
                {                    
                    var length = int.Parse(this.Encoding.GetString(serializedMessage.ToArray(),4,4));
                    if (serializedMessage.Count < length + 8)
                    {
                        break;
                    }
                    result.Add(Encoding.GetString(serializedMessage.ToArray(), 8, length).Trim());
                    serializedMessage.RemoveRange(0, 8 + length);
                }
                else
                {
                    break;
                }


            } while (serializedMessage.Count > 0);



            return result;
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>
        ///  
        /// </returns>
        public byte[] Serialize(string message)
        {
            //var tow = new List<byte>();
            //tow.AddRange(this.Encoding.GetBytes("99de"));
            //tow.AddRange(BitConverter.GetBytes(message.Length));

            //var result = this.Encoding.GetBytes(message).ToList();
            //result.InsertRange(0, tow);
            //return result.ToArray();

            message = "99de" + message.Length.ToString().PadLeft(4, '0') + message;
            var result = this.Encoding.GetBytes(message).ToList();         
            return result.ToArray();
        }


        /// <summary>
        /// 将对象转换为Json格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string ToJsonString(object obj)
        {
            if (obj == null)
            {
                return "{}";
            }
            return _ser.Serialize(obj);
        }

        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public T FromJsonString<T>(string jsonStr)
        {
            var result = default(T);
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
                    result = _ser.Deserialize<T>(jsonStr);
                }
            }
            catch (Exception exp)
            {
                string msg=string.Format ("FromJsonString 【{0}】 to 【{1}】  is Error.", jsonStr, typeof(T));

                throw new Exception(msg);
              
            }
            return result;
        }

        /// <summary>
        /// 将Json字符串转换为通用字典格式
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public Dictionary<string, object> JsonToMap(string jsonStr)
        {
            var result = FromJsonString<Dictionary<string, object>>(jsonStr);

            return result ?? new Dictionary<string, object>();
        }
    }


}

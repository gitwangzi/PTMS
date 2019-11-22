/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ca47eb47-c004-44b1-9141-70d45a8c58d2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: QuetyHtmlArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-09-04 14:36:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-09-04 14:36:55
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.IO;
using System.Linq;

namespace Gsafety.PTMS.Video.Args
{
    public class QuetyHtmlHelp
    {
        public static string QueryName { get { return "qm"; } }

        public static string VideoTypePara { get { return "vt"; } }

        public static string GetQueryString(object obj)
        {
            if (obj == null || !obj.GetType().GetCustomAttributes(false).Any(x => x.GetType() == typeof(InvokeVideoTypeAttribute)))
            {
                throw new Exception("the para is not InvokeVideoTypeAttribute falg.");
            }
            string result = null;
            var ser = new System.Runtime.Serialization.DataContractSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, obj);
                result = Convert.ToBase64String(ms.ToArray());
            }           
            result = System.Uri.EscapeDataString(result);
            return result;
        }

        public static string GetVideoType(Type t)
        {
            var att =(InvokeVideoTypeAttribute)t.GetCustomAttributes(false).First(x => x.GetType() == typeof(InvokeVideoTypeAttribute));

            return att.VideoType.ToString();
        }

        public static ArgsBase FromQueryString(string queryString, VideoType type)
        {          
            queryString = System.Uri.UnescapeDataString(queryString);
            var buffer = Convert.FromBase64String(queryString);
            ArgsBase result = null;
            Type t = null;
            switch (type)
            {
                case VideoType.AlarmVideo:
                    t = typeof(AlarmVideoArgs);
                    break;
                case VideoType.AlarmVideo15:
                    t = typeof(AlarmVideo15Args);
                    break;
                case VideoType.AtonceVideo:
                    t = typeof(AtonceVideoArgs);
                    break;
                case VideoType.FileListVideo:
                    t = typeof(FileListVideoArgs);
                    break;

            }
            var ser = new System.Runtime.Serialization.DataContractSerializer(t);
            using (var ms = new MemoryStream(buffer))
            {
                result = (ArgsBase)ser.ReadObject(ms);
            }
            return result;
        }
    }
}

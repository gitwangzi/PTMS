
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4fb6e040-7c4c-4b36-8896-qwx564frdert   
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInfo
/////    Project Description:    
/////             Class Name: BaseService
/////          Class Version: v1.0.0.0
/////            Create Time: 10/12/2013 2:02:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 10/12/2013 2:02:48 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.Common.Logging;
using System;
using System.IO;
using System.ServiceModel;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Gsafety.PTMS.BaseInfo
{
    public class BaseService
    {
        private const string _UserInfo = "UserInfo";
        private const string _Ns = "http://www.ptms.com";

        protected UserInfoMessageHeader GetUserInfo()
        {

            if (OperationContext.Current.IncomingMessageHeaders.FindHeader(_UserInfo, _Ns) >= 0)
            {
                string text = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("UserInfo", _Ns);
                XmlSerializer deserializer = new XmlSerializer(typeof(UserInfoMessageHeader));
                TextReader textReader = new StringReader(text);
                UserInfoMessageHeader userInfo = (UserInfoMessageHeader)deserializer.Deserialize(textReader);
                textReader.Close();
                return userInfo;
            }
            else
            {
                UserInfoMessageHeader txt = new UserInfoMessageHeader();
                txt.Level = 0;
                txt.Region = "*";
                txt.UserName = "zs";
                txt.Group = "0";
                return txt;
            }

        }

        protected void Info(string info)
        {
            LoggerManager.Logger.Info(info);
        }

        protected void Info<T>(string para, List<T> info)
        {
            LoggerManager.Logger.Info(para);
            foreach (var item in info)
            {
                LoggerManager.Logger.Info(item);
            }
        }

        protected void Info(object info)
        {
            if (info != null)
            {
                LoggerManager.Logger.Info(info.ToString());
            }
        }

        protected void Error(Exception ex)
        {
            LoggerManager.Logger.Error(ex);
        }

        public void Log<T>(SingleMessage<T> result)
        {
            LoggerManager.Logger.Info("IsSucess:" + result.IsSuccess.ToString());
            if (result != null && result.Result != null)
            {
                LoggerManager.Logger.Info("Result:" + result.Result.ToString());
            }
            else
            {
                LoggerManager.Logger.Info("Result: null");
            }
        }

        public void Log<T>(MultiMessage<T> result)
        {
            LoggerManager.Logger.Info("IsSucess:" + result.IsSuccess.ToString());
            if (result.Result != null)
            {
                foreach (T item in result.Result)
                {
                    LoggerManager.Logger.Info("Result:" + item.ToString());
                }
            }
            else
            {
                LoggerManager.Logger.Info("Result: null");
            }
        }
    }
}

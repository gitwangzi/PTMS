using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;


namespace Gsafety.PTMS.Share
{
    public class ServiceClientFactory
    {

        private static Dictionary<string, string> _clientMap;
        private const string _UserInfo = "UserInfo";
        private const string _Ns = "http://www.ptms.com";

        static ServiceClientFactory()
        {
            InitClients();
        }
        public static T Create<T>() where T : class
        {
            InitClients();
            var result = default(T);
            if (!_clientMap.ContainsKey(typeof(T).Name))
            {
                throw new Exception(string.Format("Can not found {0} Config.", typeof(T)));
            }
            string url = _clientMap[typeof(T).Name];
            var ms = typeof(T).GetConstructors()
                .FirstOrDefault(x =>
                {
                    return x.GetParameters().Length == 2
                      && x.GetParameters().Any(y => y.ParameterType == typeof(System.ServiceModel.Channels.Binding))
                      && x.GetParameters().Any(z => z.ParameterType == typeof(System.ServiceModel.EndpointAddress))
                     ;
                });
            if (ms != null)
            {
                var bind = new System.ServiceModel.BasicHttpBinding();
                var address = new System.ServiceModel.EndpointAddress(url);
                bind.MaxReceivedMessageSize = int.MaxValue;
                bind.MaxBufferSize = int.MaxValue;
                bind.SendTimeout = System.TimeSpan.FromMinutes(5);  //need to config
                bind.ReceiveTimeout = System.TimeSpan.FromMinutes(5);//need to config
                result = ms.Invoke(new object[] { bind, address }) as T;
                IClientChannel clientChannel = (IClientChannel)result.GetType().InvokeMember("InnerChannel", System.Reflection.BindingFlags.GetProperty, null, result, null);
                OperationContext.Current = new OperationContext((IContextChannel)clientChannel);
                if (!string.IsNullOrEmpty(ApplicationContext.Instance.AuthenticationInfo.MessageHeader))
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(_UserInfo, _Ns, ApplicationContext.Instance.AuthenticationInfo.MessageHeader));
                }
            }
            else
            {
                throw new Exception(string.Format("The {0} is not Service Client.", typeof(T)));
            }
            return result;
        }

        public static void CreateMessageHeader(IClientChannel clientChannel)
        {
            OperationContext.Current = new OperationContext((IContextChannel)clientChannel);
            if (OperationContext.Current.OutgoingMessageHeaders != null && OperationContext.Current.OutgoingMessageHeaders.FindHeader(_UserInfo, _Ns) >= 0)
                return;
            if (!string.IsNullOrEmpty(ApplicationContext.Instance.AuthenticationInfo.MessageHeader))
            {
                OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(_UserInfo, _Ns, ApplicationContext.Instance.AuthenticationInfo.MessageHeader));
            }
        }


        private static void InitClients()
        {
            if (_clientMap != null)
            {
                return;
            }
            var doc = System.Xml.Linq.XElement.Parse(ApplicationContext.Instance.ServerConfig.ServiceUrlConfig);
            _clientMap = doc.Elements("add")
                .ToDictionary(x => x.Attribute("key").Value, y => y.Attribute("value").Value);
        }
        public static string ServiceConfig(string index)
        {
            if (_clientMap != null)
            {
                return _clientMap[index];
            }
            else
            {
                return null;
            }
        }
    }
}

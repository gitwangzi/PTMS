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
using System.ServiceModel;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.ServiceReference.VedioService;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using Gsafety.PTMS.Bases.Librarys;
using System.Threading.Tasks;
using System.Xml;
using Jounce.Core.Application;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;

namespace Gsafety.PTMS.Share.MessageManage
{
    public partial class MessageCenterClient
    {
        string _sessionid = string.Empty;

        public string SessionID
        {
            get { return _sessionid; }
            set { _sessionid = value; }
        }
        public MessageCenterClient()
        {
            _sessionid = Guid.NewGuid().ToString();
            _heartbeatMonitorInfo = new HeartbeatMonitorInfo();
            _callback = new MQCallBackExt();
            _instanceContext = new InstanceContext(_callback);

            Task.Factory.StartNew(() => HeartBeatTimer_Tick());
        }

        bool _initComplete = false;

        MQCallBackExt _callback = null;
        private HeartbeatMonitorInfo _heartbeatMonitorInfo;
        private readonly int MonitorHeartbeatTimespan = 10000;
        private MessageServiceExtClient _messageClient;
        private readonly int CmdSendTimeout = 2000;
        private readonly int CmdSetSendTimeout = 10000;
        private InstanceContext _instanceContext;
        bool _stop = false;

        public bool Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }
        private void HeartBeatTimer_Tick()
        {
            Thread.Sleep(1000);
            while (!_stop)
            {
                if (_heartbeatMonitorInfo.ServiceStatus == MessageServiceStatus.Connected)
                {
                    if (_heartbeatMonitorInfo.IsDisconnection)
                    {
                        _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
                        ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "Message Service Disconnected");
                    }
                }

                if (_heartbeatMonitorInfo.ServiceStatus == MessageServiceStatus.DisConnected)
                {
                    ApplicationContext.Instance.Logger.LogWaring(GetType().FullName, "ReConnection Message Service");
                    Init();
                }
                Thread.Sleep(MonitorHeartbeatTimespan);
            }
        }

        private string GetMessageServiceUrl(string serviceName)
        {
            var doc = XElement.Parse(ApplicationContext.Instance.ServerConfig.ServiceUrlConfig);
            return doc.Descendants("add").FirstOrDefault(x => x.Attribute("key").Value.Equals(serviceName)).Attribute("value").Value;

        }

        public void Init()
        {
            if (_heartbeatMonitorInfo.ServiceStatus == MessageServiceStatus.DisConnected)
            {
                if (_messageClient != null)
                {
                    _messageClient.CloseAsync();
                }
                string url = GetMessageServiceUrl(typeof(MessageServiceExtClient).Name);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    EndpointAddress address = new EndpointAddress(url);
                    BindingElementCollection elements = new BindingElementCollection();
                    elements.Add(new BinaryMessageEncodingBindingElement());
                    elements.Add(new TcpTransportBindingElement());
                    CustomBinding customBinding = new CustomBinding(elements);

                    customBinding.SendTimeout = System.TimeSpan.FromSeconds(50);
                    customBinding.ReceiveTimeout = System.TimeSpan.MaxValue;


                    customBinding.Elements.Find<TcpTransportBindingElement>().ConnectionBufferSize = 500000;
                    customBinding.Elements.Find<TcpTransportBindingElement>().MaxBufferSize = int.MaxValue;
                    customBinding.Elements.Find<TcpTransportBindingElement>().MaxReceivedMessageSize = int.MaxValue;

                    _messageClient = new MessageServiceExtClient(_instanceContext, customBinding, address);

                    UserModel model = new UserModel();
                    model.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                    model.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                    model.UserToken = _sessionid;
                    model.Vehicles = new System.Collections.ObjectModel.ObservableCollection<string>();
                    foreach (var item in _vehicles)
                    {
                        model.Vehicles.Add(item);
                    }

                    model.Organization = new System.Collections.ObjectModel.ObservableCollection<string>();
                    if (ApplicationContext.Instance.AuthenticationInfo.Organizations != null)
                    {
                        foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Organizations)
                        {
                            model.Organization.Add(item.ID);
                        }
                    }

                    model.AlarmProcessor = ApplicationContext.Instance.AuthenticationInfo.AlarmFunction;

                    _messageClient.RegisterCompleted += _messageClient_RegisterCompleted;
                    _messageClient.InnerChannel.Faulted += InnerChannel_Faulted;
                    _messageClient.RegisterAsync(model);

                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.RequestConnect;

                }
                else
                {
                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
                    Exception exception = new Exception("Can not found MessageServiceClient Config!");
                    ApplicationContext.Instance.Logger.LogException(GetType().FullName, exception);
                }
            }
        }

        void InnerChannel_Faulted(object sender, EventArgs e)
        {
            _initComplete = false;
            _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
        }

        void _messageClient_RegisterCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    _initComplete = true;
                    MessageActionCache.InvokeAction();
                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.Connected;
                }
                else
                {
                    _initComplete = false;
                    _heartbeatMonitorInfo.ServiceStatus = MessageServiceStatus.DisConnected;
                    ApplicationContext.Instance.Logger.LogInforMession(GetType().FullName, e.Error.ToString());
                }
                _heartbeatMonitorInfo.LastHeartBeatTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(this.GetType().FullName, ex);
            }
        }


    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6ea4f72b-0405-4eb0-9318-8763fa614662      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LIW
/////                 Author: TEST(liw)
/////======================================================================
/////           Project Name: Gsafety.MQ
/////    Project Description:    
/////             Class Name: MQConnectionHelper
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/7/31 11:17:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/7/31 11:17:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace LogisticsWebApi.MQHelper
{
    public class MQFactory
    {
        private IModel _model;

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <returns></returns>
        public IConnection CreateConnection()
        {
            try
            {
                ConnectionFactory cf = new ConnectionFactory();
                cf.UserName = MQConfigHelper.UserName;
                cf.Password = MQConfigHelper.Password;
                cf.Port = AmqpTcpEndpoint.UseDefaultPort;
                cf.Protocol = Protocols.DefaultProtocol;
                cf.HostName = MQConfigHelper.HostName;

                return cf.CreateConnection();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 建立Model
        /// </summary>
        /// <returns></returns>
        public IModel CreateModel()
        {
            _model = CreateConnection().CreateModel();
            return _model;
        }

        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="exchange">exchange名称</param>
        /// <param name="route">路由</param>
        /// <param name="msg">消息</param>
        public void PublishMessage(string exchange, string route, byte[] msg)
        {
            try
            {
                IBasicProperties bp = _model.CreateBasicProperties();

                bp.Type = "1";
                bp.Priority = 1;
                _model.BasicPublish(exchange, route, bp, msg);
            }
            catch (Exception ex)
            {
                //日志
                throw ex;
            }
        }
    }
}

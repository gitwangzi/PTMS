using Gsafety.Common.Util;
using Gsafety.MQ;
using Gsafety.PTMS.Common.Data;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTest
{
    public class MDVRTextTest
    {
        public static void Test()
        {
            IConnection _shareConn = null;
            while (_shareConn == null)
            {
                _shareConn = new MQFactory().CreateConnection();
            }
            IModel _shareCh = _shareConn.CreateModel();

            ////business queue
            _shareCh.QueueDeclare("PTMS.Business.Test", true, true, true, null);

            MDVRMessage message = new MDVRMessage();
            message.UID = "99999SXCA123";
            message.TextContent = "你好";
            message.TextFlag = new List<int>();
            message.TextFlag.Add(4);
            message.TextFlag.Add(3);
            string json = ConvertHelper.ConvertModelToJson(message);
            byte[] sendbytes = System.Text.UnicodeEncoding.UTF8.GetBytes(json);

            IBasicProperties bp = _shareCh.CreateBasicProperties();
            bp.Type = "1";
            bp.Priority = 1;
            _shareCh.BasicPublish(Constdefine.MDVREXCHANGE, "MDVR.SendText.99999SXCA123", bp, sendbytes);

            _shareCh.Close();
            _shareConn.Close();
        }
    }
}

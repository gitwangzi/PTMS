using Gsafety.PTMS.ServiceReference.OrderClientService;
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

namespace Gsafety.PTMS.Share.MessageManage
{
    public partial class MessageCenterClient
    {
        public void ClientChange(Gsafety.PTMS.ServiceReference.MessageServiceExt.OrderClient orderclient)
        {
            _messageClient.ClientChangeAsync(orderclient);
        }
    }
}

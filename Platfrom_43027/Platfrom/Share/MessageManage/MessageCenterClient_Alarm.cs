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
        public void SendCompleteAlarm(Gsafety.PTMS.ServiceReference.MessageServiceExt.CompleteAlarm completeAlarm)
        {
            _messageClient.CompleteAlarmAsync(completeAlarm);
        }

        public void TransferAlarm(Gsafety.PTMS.ServiceReference.MessageServiceExt.AlarmInfoEx alarm)
        {
            _messageClient.TransferAlarmAsync(alarm);
        }
    }
}

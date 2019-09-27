using Gsafety.PTMS.ServiceReference.MessageServiceExt;
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
        public void BeginInstallSuite(string mdvrcoresn)
        {
            _messageClient.BeginInstallSuiteAsync(mdvrcoresn);
        }

        public void CompleteInstallSuite(string mdvrcoresn, string organization, string vehicleid)
        {
            _messageClient.CompleteInstallSuiteAsync(mdvrcoresn, organization, vehicleid);
        }

        public void RemoveInstallSuite(string mdvrcoresn)
        {
            _messageClient.RemoveInstallSuiteAsync(mdvrcoresn);
        }

        public void BeginInstallGPS(string mdvrcoresn)
        {
            _messageClient.BeginInstallGPSAsync(mdvrcoresn);
        }

        public void CompleteInstallGPS(string mdvrcoresn, string organization, string vehicleid)
        {
            _messageClient.CompleteInstallGPSAsync(mdvrcoresn, organization, vehicleid);
        }

        public void RemoveInstallGPS(string mdvrcoresn)
        {
            _messageClient.RemoveInstallGPSAsync(mdvrcoresn);
        }

        public void SetAlarmParaCommand(SetAlarmPara param)
        {
            _messageClient.SetAlarmParaCommandAsync(param);
        }
    }
}

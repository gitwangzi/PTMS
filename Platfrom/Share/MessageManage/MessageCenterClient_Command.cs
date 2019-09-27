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
        public void SendGetVideoListCMD(Gsafety.PTMS.ServiceReference.MessageServiceExt.QueryMdvrFileList model)
        {
            _messageClient.SendGetVideoListCMDAsync(model);
        }

        public void SendDownloadMdvrFile(Gsafety.PTMS.ServiceReference.MessageServiceExt.DownloadFile model)
        {
            _messageClient.SendDownloadMdvrFileAsync(model);
        }

        public void SendTakePictureCMD(Gsafety.PTMS.ServiceReference.MessageServiceExt.TakePictureArgs model)
        {
            _messageClient.SendTakePictureCMDAsync(model);
        }
    }
}

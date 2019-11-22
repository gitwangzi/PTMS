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
using Gsafety.PTMS.ServiceReference.MessageService;

namespace Gsafety.Common.CommMessage
{
    public class AlertAddRemoveArgs:AlertBaseModel
    {
        //1 draw，0 delete
        public int Op { get; set; }
    }
}

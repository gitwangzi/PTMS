using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
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
using Gsafety.Common.CommMessage;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;
using Jounce.Framework.ViewModel;
using Gsafety.PTMS.BasicPage.ViewModels;

namespace Gsafety.PTMS.VideoDownloadManagement.ViewModels
{
    [ExportAsViewModel(VideoDownloadName.VideoDownLoadVm)]
    public class VideoDownLoadVm : BaseEntityViewModel
    {
        public HistoryVideoManageContentViewModel HistoryVideoManageContentViewModel { get; set; }

        public VideoDownLoadVm()
        {
            HistoryVideoManageContentViewModel = new HistoryVideoManageContentViewModel();
        }

        public string Test
        {
            get { return "ssss"; }
        }
    }
}

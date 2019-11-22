using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.VideoDownloadManagement.Views
{
    [ExportAsView(VideoDownloadName.VideoDownLoadV, Category = VideoDownloadName.CategoryName, MenuName = VideoDownloadName.MenuName, Url = "/VideoDownloadV")]
    [ExportViewToRegion(VideoDownloadName.VideoDownLoadV, VideoDownloadName.VideoDownloadContainer)]
    public partial class VideoDownloadView : UserControl
    {
        public VideoDownloadView()
        {
            InitializeComponent();

            this.Loaded += VideoDownloadView_Loaded;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        void VideoDownloadView_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

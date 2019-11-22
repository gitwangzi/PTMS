using Gsafety.Common.Controls;
using Jounce.Core.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.BasicPage.Models;

namespace Gsafety.PTMS.VideoManagement.Views
{
    public partial class VideoCtrl : UserControl, IEventSink<MediaInfoEx>
    {
        private MediaPlayerContainer _mediaPlayerContainer;

        public VideoCtrl()
        {
            InitializeComponent();

            this.Loaded += VideoCtrl_Loaded;

            try
            {
                ApplicationContext.Instance.EventAggregator.Subscribe<MediaInfoEx>(this);
            }
            catch (System.Exception ex)
            {
            }
        }

        void VideoCtrl_Loaded(object sender, RoutedEventArgs e)
        {
            SetVideoCount(2, 2);
        }

        private void SetVideoCount(int rowCount, int columnCount)
        {
            try
            {
                if (_mediaPlayerContainer == null)
                {
                    foreach (MediaPlayerContainer item in contentGrid.Children)
                    {
                        item.OnClosed();
                    }
                    contentGrid.Children.Clear();

                    _mediaPlayerContainer = new MediaPlayerContainer(rowCount, columnCount, true);
                    _mediaPlayerContainer.IsHideProgressControl = true;
                    _mediaPlayerContainer.Orientation = Orientation.Horizontal;
                    _mediaPlayerContainer.AutoPlay = true;
                    contentGrid.Children.Add(_mediaPlayerContainer);
                }
                else
                {
                    _mediaPlayerContainer.SetVideoCount(rowCount, columnCount);
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SetVideoCount(1, 1);
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            SetVideoCount(2, 2);
        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            SetVideoCount(3, 3);
        }

        private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            SetVideoCount(4, 4);
        }

        public void HandleEvent(MediaInfoEx publishedEvent)
        {
            if (publishedEvent == null)
            {
                return;
            }

            foreach (var item in publishedEvent.MediaInfoItems)
            {
                item.ShowRemoveBtn = true;
                _mediaPlayerContainer.Play(item);
            }
        }
    }
}

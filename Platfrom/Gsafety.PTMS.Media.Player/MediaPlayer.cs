using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.Player
{
    public partial class MediaPlayer
    {
        private bool _isDragProcessSlider = false;
        private Thumb _sliderProcessThumb;
        private TsMediaStreamSource _mediaSource;
        private MediaElementState _beforeDragState;

        public string InfoMessage
        {
            get { return txtInfoMessage.Text; }
            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    txtInfoMessage.Text = value;
                }
            }
        }

        public MediaElement MediaElement
        {
            get
            {
                return mediaElement1;
            }
        }

        private void btnVolumn_Click(object sender, RoutedEventArgs e)
        {
            if (btnVolumn.IsChecked == true)
            {
                mediaElement1.Volume = 0;
                volumnImage.Source = new BitmapImage(new Uri("Images/VideoVolumnScience.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                mediaElement1.Volume = sliderVolumn.Value;
                volumnImage.Source = new BitmapImage(new Uri("Images/VideoVolumn.png", UriKind.RelativeOrAbsolute));
            }
        }

        private void sliderVolumn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement1 == null || sliderVolumn == null || btnVolumn.IsChecked == true)
            {
                return;
            }
            mediaElement1.Volume = sliderVolumn.Value;
        }

        #region 进度条拖拽

        void MediaPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            InitProcessSliderThumbEvent();
        }

        private void InitProcessSliderThumbEvent()
        {
            _sliderProcessThumb = FindTemplateChild(sliderProcess, "HorizontalThumb") as Thumb;
            if (_sliderProcessThumb == null)
            {
                return;
            }

            _sliderProcessThumb.DragStarted += _sliderProcessThumb_DragStarted;
            _sliderProcessThumb.DragCompleted += _sliderProcessThumb_DragCompleted;
        }

        void _sliderProcessThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _beforeDragState = mediaElement1.CurrentState;

            mediaElement1.Pause();
            _isDragProcessSlider = true;
        }

        async void _sliderProcessThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _isDragProcessSlider = false;

            //mediaElement1.Position = TimeSpan.FromSeconds(sliderProcess.Value);

            //if (_beforeDragState == MediaElementState.Playing)
            //{
            //    mediaElement1.Play();
            //}

            //return;

            if (_beforeDragState == MediaElementState.Playing)
            {
                this.AutoPlay = true;
            }
            else if (_beforeDragState == MediaElementState.Paused || _beforeDragState == MediaElementState.Stopped)
            {
                this.AutoPlay = false;
            }

            await PlayFromTarget(TimeSpan.FromSeconds(sliderProcess.Value));
        }

        private FrameworkElement FindTemplateChild(DependencyObject obj, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;
                if (child.Name == name)
                {
                    return child;
                }

                child = FindTemplateChild(child, name);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }


        #endregion

        private async Task PlayFromTarget(TimeSpan target)
        {
            _seekTarget = target;

            //mediaElement1.Position = target;

            //return;


            await CloseMedia();

            await ConnectMedia();
        }

        private void ContentPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_removeBtnVisibility)
            {
                btnRemove.Visibility = Visibility.Collapsed;
            }
        }

        private void ContentPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_removeBtnVisibility)
            {
                btnRemove.Visibility = Visibility.Visible;
            }
        }
    }
}

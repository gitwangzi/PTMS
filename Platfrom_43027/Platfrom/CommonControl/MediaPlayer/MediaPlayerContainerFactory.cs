using Gsafety.Common.Controls;
using Jounce.Core.Event;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace Gsafety.Common.Controls
{
    public class MediaPlayerContainerFactory : IEventSink<MediaInfo>, IPartImportsSatisfiedNotification
    {
        [Import]
        private IEventAggregator EventAggregator { get; set; }
        private MediaPlayerWindow _currentWindow;

        public Grid Root { get; set; }
        public MediaPlayerContainerFactory()
        {
            CompositionInitializer.SatisfyImports(this);
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<MediaInfo>(this);
        }

        public void HandleEvent(MediaInfo publishedEvent)
        {
            if (publishedEvent == null)
            {
                return;
            }

            if (this.Root == null)
            {
                throw new Exception("MediaPlayerContainerFactory Root Cannot Be Null");
            }

            if (_currentWindow != null)
            {
                _currentWindow.Close();
                this.Root.Children.Remove(_currentWindow);
            }

            _currentWindow = new MediaPlayerWindow(publishedEvent);
            _currentWindow.ParentLayoutRoot = this.Root;
            _currentWindow.Show();
        }
    }
}

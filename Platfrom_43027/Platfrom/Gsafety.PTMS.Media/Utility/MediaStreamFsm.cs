using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Gsafety.PTMS.Media.Utility
{
    public struct MediaStreamFsm
    {
        #region MediaEvent enum

        public enum MediaEvent
        {
            MediaStreamSourceAssigned, // MediaStreamSource.SetSource(...)
            OpenMediaAsyncCalled,
            CallingReportOpenMediaCompleted,
            CallingReportOpenMediaCompletedLive,
            SeekAsyncCalled,
            CallingReportSeekCompleted,
            CallingReportSampleCompleted,
            GetSampleAsyncCalled,
            CloseMediaCalled,
            MediaStreamSourceCleared, // MediaStreamSource.Source = null;
            DisposeCalled,
            StreamsClosed
        }

        #endregion

        #region MediaState enum

        public enum MediaState
        {
            Invalid = 0,
            Idle = 1,
            Assigned = 2,
            Opening = 3,
            AwaitSeek = 4,
            Seeking = 5,
            AwaitPlaying = 6,
            Playing = 7,
            Closing = 8,
            Disposing = 9,
            Draining = 10
        }

        #endregion

        static readonly Dictionary<MediaState, Dictionary<MediaEvent, MediaState>> ValidTransitions =
            new Dictionary<MediaState, Dictionary<MediaEvent, MediaState>>
            {
                {
                    MediaState.Idle,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // A MediaStreamSource is assigned to the MediaElement
                        { MediaEvent.MediaStreamSourceAssigned, MediaState.Assigned },
                        // OpenMediaAsync() has been called
                        { MediaEvent.OpenMediaAsyncCalled, MediaState.Opening },
                        { MediaEvent.DisposeCalled, MediaState.Idle }
                    }
                },
                {
                    MediaState.Assigned,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // OpenMediaAsync() has been called
                        { MediaEvent.OpenMediaAsyncCalled, MediaState.Opening }
                    }
                },
                {
                    MediaState.Opening,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // ReportOpenMediaCompleted() has been called on a non-seekable stream
                        { MediaEvent.CallingReportOpenMediaCompleted, MediaState.AwaitSeek },
                        // ReportOpenMediaCompleted() has been called on a seekable stream
                        { MediaEvent.CallingReportOpenMediaCompletedLive, MediaState.Playing },
                        { MediaEvent.CloseMediaCalled, MediaState.Closing }
                    }
                },
                {
                    MediaState.AwaitSeek,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // SeekAsync() has been called
                        { MediaEvent.SeekAsyncCalled, MediaState.Seeking }
                    }
                },
                {
                    MediaState.Seeking,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // ReportSeekCompleted() has been called
                        { MediaEvent.CallingReportSeekCompleted, MediaState.Playing },
                        { MediaEvent.GetSampleAsyncCalled, MediaState.Seeking },
                        // Should we wait to ReportGetSampleCompleted() until after ReportSeekCompleted?
                        { MediaEvent.CallingReportSampleCompleted, MediaState.Seeking },
                        { MediaEvent.CloseMediaCalled, MediaState.Closing },
                    }
                },
                //{
                //    MediaState.AwaitPlaying,
                //    new Dictionary<MediaEvent, MediaState>
                //    {
                //        // We have seen a GetSampleAsync()
                //        { MediaEvent.GetSampleAsyncCalled, MediaState.Playing },
                //        // Since we we are getting GetSampleAsync()s while waiting for ReportSeekCompleted,
                //        // we need to deal with ReportSampleCompleted() as well.
                //        { MediaEvent.CallingReportSampleCompleted, MediaState.AwaitPlaying }
                //    }
                //},
                {
                    MediaState.Playing,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        { MediaEvent.GetSampleAsyncCalled, MediaState.Playing },
                        { MediaEvent.CloseMediaCalled, MediaState.Draining },
                        { MediaEvent.CallingReportSampleCompleted, MediaState.Playing },
                        { MediaEvent.SeekAsyncCalled, MediaState.Seeking }
                    }
                },
                {
                    MediaState.Draining,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        { MediaEvent.StreamsClosed, MediaState.Closing },
                        { MediaEvent.GetSampleAsyncCalled, MediaState.Draining },
                        { MediaEvent.CallingReportSampleCompleted, MediaState.Draining },
                        { MediaEvent.SeekAsyncCalled, MediaState.Draining },
                        { MediaEvent.CallingReportSeekCompleted, MediaState.Draining },
                    }
                },
                {
                    MediaState.Closing,
                    new Dictionary<MediaEvent, MediaState>
                    {
                        // The MediaStreamSource has been disposed (by MediaElement)
                        { MediaEvent.DisposeCalled, MediaState.Idle }
                    }
                },
            };

        static bool NoisyLogging = false;
        volatile int _mediaState;

        MediaState? Find(MediaState source, MediaEvent mediaEvent)
        {
            Dictionary<MediaEvent, MediaState> transitions;
            if (!ValidTransitions.TryGetValue(source, out transitions))
                return null;

            MediaState newState;
            if (!transitions.TryGetValue(mediaEvent, out newState))
                return null;

            return newState;
        }

        public void ValidateEvent(MediaEvent mediaEvent)
        {
            var expectedState = (MediaState)_mediaState;

            //LoggerInstance.Debug("MediaStreamFsm.ValidateEvent({0}) state {1}", mediaEvent, expectedState);

            for (; ; )
            {
                var newState = Find(expectedState, mediaEvent);

                if (!newState.HasValue)
                {
                    var message = string.Format("ValidateEvent Invalid state transition: state {0} event {1}", expectedState, mediaEvent);

                    LoggerInstance.Debug(message);

                    return;
                    //throw new InvalidOperationException(message);
                }

#pragma warning disable 0420
                var oldState = (MediaState)Interlocked.CompareExchange(ref _mediaState, (int)newState.Value, (int)expectedState);
#pragma warning restore 0420

                if (oldState == expectedState)
                {
                    if (NoisyLogging || oldState != newState.Value)
                        LoggerInstance.Debug("Media {0}: {1} -> {2} at {3}", mediaEvent, oldState, newState, DateTimeOffset.Now);

                    return;
                }

                expectedState = oldState;
            }
        }

        public void Reset()
        {
            //LoggerInstance.Debug("MediaStreamFsm.Reset()");

            _mediaState = (int)MediaState.Idle;
        }

        public override string ToString()
        {
            return string.Format("[Media {0}]", (MediaState)_mediaState);
        }
    }
}

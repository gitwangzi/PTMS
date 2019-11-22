using System;

namespace Gsafety.PTMS.Media.Buffering
{
    public class DefaultBufferingPolicy : IBufferingPolicy
    {
        int _bytesMaximum = 8192 * 1024;
        int _bytesMinimum = 10 * 1024;
        //int _bytesMinimum = 300 * 1024;
        int _bytesMinimumStarting = 10 * 1024;
        //TimeSpan _durationBufferingDone = TimeSpan.FromSeconds(9);
        TimeSpan _durationBufferingDone = TimeSpan.FromSeconds(2.5);
        TimeSpan _durationBufferingMax = TimeSpan.FromSeconds(25);
        TimeSpan _durationReadDisable = TimeSpan.FromSeconds(30);
        TimeSpan _durationReadEnable = TimeSpan.FromSeconds(15);
        TimeSpan _durationStartingDone = TimeSpan.FromSeconds(2.5);

        public int BytesMaximum
        {
            get { return _bytesMaximum; }
            set { _bytesMaximum = value; }
        }

        public int BytesMinimum
        {
            get { return _bytesMinimum; }
            set { _bytesMinimum = value; }
        }

        public int BytesMinimumStarting
        {
            get { return _bytesMinimumStarting; }
            set { _bytesMinimumStarting = value; }
        }

        public TimeSpan DurationReadEnable
        {
            get { return _durationReadEnable; }
            set { _durationReadEnable = value; }
        }

        public TimeSpan DurationBufferingDone
        {
            get { return _durationBufferingDone; }
            set { _durationBufferingDone = value; }
        }

        public TimeSpan DurationStartingDone
        {
            get { return _durationStartingDone; }
            set { _durationStartingDone = value; }
        }

        public TimeSpan DurationReadDisable
        {
            get { return _durationReadDisable; }
            set { _durationReadDisable = value; }
        }

        public TimeSpan DurationBufferingMax
        {
            get { return _durationBufferingMax; }
            set { _durationBufferingMax = value; }
        }

        public virtual bool ShouldBlockReads(bool isReadBlocked, TimeSpan durationBuffered, int bytesBuffered, bool isExhausted, bool isAllExhausted)
        {
            if (isAllExhausted)
                return false;

            if (bytesBuffered > BytesMaximum)
                return true;

            if (isExhausted)
                return false;

            if (durationBuffered < DurationReadEnable)
                return false;

            if (durationBuffered > DurationReadDisable)
                return true;

            return isReadBlocked;
        }

        public virtual bool IsDoneBuffering(TimeSpan bufferDuration, int bytesBuffered, int bytesBufferedWhenExhausted, bool isStarting)
        {
            var bufferSize = Math.Max(0, bytesBuffered - bytesBufferedWhenExhausted);

            var durationDone = isStarting ? DurationStartingDone : DurationBufferingDone;
            var bytesMinimum = isStarting ? BytesMinimumStarting : BytesMinimum;

            return (bufferDuration >= durationDone && bufferSize >= bytesMinimum) || bytesBuffered >= BytesMaximum || bufferDuration > DurationBufferingMax;
        }

        public virtual float GetProgress(TimeSpan bufferDuration, int bytesBuffered, int bytesBufferedWhenExhausted, bool isStarting)
        {
            var durationDone = isStarting ? DurationStartingDone : DurationBufferingDone;
            var bytesMinimum = isStarting ? BytesMinimumStarting : BytesMinimum;

            var bufferSize = Math.Max(0, bytesBuffered - bytesBufferedWhenExhausted);

            var bufferingStatus1 = Math.Max(0, bufferDuration.Ticks / (float)durationDone.Ticks);
            var bufferingStatus2 = bufferSize / (float)bytesMinimum;
            var bufferingStatus3 = bytesBuffered / (float)BytesMaximum;
            var bufferingStatus4 = Math.Max(0, bufferDuration.Ticks / (float)DurationBufferingMax.Ticks);

            var bufferingStatus = Math.Max(Math.Max(Math.Min(bufferingStatus1, bufferingStatus2), bufferingStatus3), bufferingStatus4);

            if (bufferingStatus > 1.0f)
                bufferingStatus = 1.0f;
            else if (bufferingStatus < 0.0f)
                bufferingStatus = 0.0f;

            return bufferingStatus;
        }
    }

    public static class DefaultBufferingPolicyExtensions
    {
        const int BytesMaximumLowerLimit = 512 * 1024;
        const int BytesMinimumLowerLimit = 1024;

        public static DefaultBufferingPolicy SetBandwidth(this DefaultBufferingPolicy policy, double bitsPerSecond)
        {
            if (null == policy)
                throw new ArgumentNullException("policy");
            if (bitsPerSecond < 100 || bitsPerSecond > 500 * 1024 * 1024)
                throw new ArgumentOutOfRangeException("bitsPerSecond");

            var bytesPerSecond = bitsPerSecond * (1.0 / 8);

            var starting = (int)Math.Round(policy.DurationStartingDone.TotalSeconds * bytesPerSecond);
            var minimum = (int)Math.Round(policy.DurationBufferingDone.TotalSeconds * bytesPerSecond);
            var maximum = (int)Math.Round(2 * policy.DurationBufferingMax.TotalSeconds * bytesPerSecond);

            if (starting < BytesMinimumLowerLimit)
                starting = BytesMinimumLowerLimit;

            if (minimum < BytesMinimumLowerLimit)
                minimum = BytesMinimumLowerLimit;

            if (maximum < BytesMaximumLowerLimit)
                maximum = BytesMaximumLowerLimit;

            if (minimum > maximum)
                minimum = maximum;

            if (starting > maximum)
                starting = maximum;

            policy.BytesMinimumStarting = starting;
            policy.BytesMinimum = minimum;
            policy.BytesMaximum = maximum;

            return policy;
        }
    }
}

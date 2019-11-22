using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Common;

namespace Gsafety.PTMS.Media.RTSP.Sdp
{
    /// <summary>
    /// Represents a TimeDescription with optional Repeat times.
    /// Parses and Creates.
    /// </summary>
    public class TimeDescription : SuppressedFinalizerDisposable, IEnumerable<SessionDescriptionLine>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the start time.
        /// If seto to 0 then the session is not bounded,  though it will not become active until after the <see cref="StartTime"/>.  
        /// </summary>
        /// <remarks>These values are the decimal representation of Network Time Protocol (NTP) time values in seconds since 1900 </remarks>
        public string StartTimeToken
        {
            get { return TimeDescriptionLine.StartTimeToken; }
            set { TimeDescriptionLine.StartTimeToken = value; }
        }

        public double StartTime
        {
            get { return TimeDescriptionLine.StartTime; }
            set { TimeDescriptionLine.StartTime = (long)value; }
        }

        /// <summary>
        /// Gets or sets the stop time.
        /// If set to 0 and the <see cref="StartTime"/> is also zero, the session is regarded as permanent.
        /// </summary>
        /// <remarks>These values are the decimal representation of Network Time Protocol (NTP) time values in seconds since 1900 </remarks>
        public double StopTime
        {
            get { return TimeDescriptionLine.StopTime; }
            set { TimeDescriptionLine.StopTime = (long)value; }
        }

        public string StopTimeToken
        {
            get { return TimeDescriptionLine.StopTimeToken; }
            set { TimeDescriptionLine.StopTimeToken = value; }
        }

        /// <summary>
        /// Gets or sets the DateTime representation of <see cref="StarTime"/>
        /// Throws an ArgumentOutOfRangeException if SessionStartTime was out of range.
        /// </summary>
        public DateTime NtpStartDateTime
        {
            get { return TimeDescriptionLine.NtpStartDateTime; }
            set { TimeDescriptionLine.NtpStartDateTime = value; }
        }

        /// <summary>
        /// Gets the DateTime representation of <see cref="StopTime"/>
        /// Throws an ArgumentOutOfRangeException if SessionStopTime was out of range.
        /// </summary>
        public DateTime NtpStopDateTime
        {
            get { return TimeDescriptionLine.NtpStopDateTime; }
            set { TimeDescriptionLine.NtpStopDateTime = value; }
        }

        /// <summary>
        /// If the <see cref="StopTime"/> is set to zero, then the session is not bounded,  though it will not become active until after the <see cref="StartTime"/>.  
        /// If the <see cref="StartTime"/> is also zero, the session is regarded as permanent.
        /// </summary>
        public bool IsPermanent { get { return TimeDescriptionLine.IsPermanent; } }

        //HasDefinedStartTime !=

        /// <summary>
        /// Indicates if the <see cref="StartTime"/> is 0
        /// </summary>
        public bool Unbounded { get { return TimeDescriptionLine.Unbounded; } }

        /// <summary>
        /// Gets or sets any repeat descriptions assoicated with the TimeDescription.
        /// </summary>
        //public List<string> RepeatTimes { get; private set; }

        public readonly List<Lines.SessionRepeatTimeLine> RepeatLines;

        /// <summary>
        /// Indicates if there are any repeat times.
        /// </summary>
        public bool HasRepeatTimes { get { return RepeatLines.Count > 0; } }

        /// <summary>
        /// Calculates the length in bytes of this TimeDescription.
        /// </summary>
        public int Length
        {
            get
            {
                return TimeDescriptionLine.Length + RepeatLines.Sum(l => l.Length);
            }
        }

        #endregion

        internal protected readonly Lines.SessionTimeDescriptionLine TimeDescriptionLine;

        #region Constructor

        public TimeDescription(bool shouldDispose = true)
            : base(shouldDispose)
        {
            TimeDescriptionLine = new Lines.SessionTimeDescriptionLine();

            RepeatLines = new List<Lines.SessionRepeatTimeLine>();
        }

        public TimeDescription(int startTime, int stopTime)
            : this()
        {
            StartTime = startTime;
            StopTime = stopTime;
        }

        public TimeDescription(string[] sdpLines, ref int index)
            : this()
        {

            TimeDescriptionLine = new Lines.SessionTimeDescriptionLine(sdpLines, ref index);

            string sdpLine;

            //Iterate remaining lines
            for (int e = sdpLines.Length; index < e; )
            {
                //Scope a line
                sdpLine = sdpLines[index];

                if (string.IsNullOrWhiteSpace(sdpLine))
                {
                    ++index;

                    continue;
                }

                //If we are not extracing repeat times then there is no more TimeDescription to parse
                if (sdpLine[0] != Gsafety.PTMS.Media.RTSP.Sdp.Lines.SessionRepeatTimeLine.RepeatType) break;

                //Parse and add the repeat time
                try
                {
                    //r=<repeat interval> <active duration> <offsets from start-time>
                    RepeatLines.Add(new Sdp.Lines.SessionRepeatTimeLine(sdpLines, ref index));
                }
                catch (Exception ex)
                {
                    Gsafety.PTMS.Media.RTSP.Common.TaggedExceptionExtensions.RaiseTaggedException(this, "Invalid Repeat Time", ex);
                    break;
                }
            }

        }

        public TimeDescription(TimeDescription other, bool referenceRepeatTimes = false, bool shouldDispose = true)
            : base(shouldDispose)
        {
            StartTime = other.StartTime;

            StopTime = other.StopTime;

            if (referenceRepeatTimes) RepeatLines = other.RepeatLines;
            else RepeatLines = new List<Lines.SessionRepeatTimeLine>(other.RepeatLines);
        }

        public TimeDescription(long sessionStart, long sessionStop)
            : this()
        {
            StartTime = sessionStart;

            StopTime = sessionStop;
        }

        public TimeDescription(DateTime sessionStartUtc, DateTime sessionStopUtc)
            : this()
        {
            NtpStartDateTime = sessionStartUtc;

            NtpStopDateTime = sessionStopUtc;
        }

        #endregion

        public string ToString(SessionDescription sdp = null)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(TimeDescriptionLine.ToString());

            foreach (Lines.SessionRepeatTimeLine repeatTime in RepeatLines)
            {
                builder.Append(repeatTime.ToString());
            }

            return builder.ToString();
        }

        public override string ToString()
        {
            return ToString(null);
        }

        public IEnumerator<SessionDescriptionLine> GetEnumerator()
        {
            yield return TimeDescriptionLine;

            foreach (Lines.SessionRepeatTimeLine repeatTime in RepeatLines)
            {
                yield return repeatTime;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<SessionDescriptionLine>)this).GetEnumerator();
        }
    }
}

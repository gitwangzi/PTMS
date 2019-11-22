using System;

namespace Gsafety.PTMS.Media.Metadata
{
    public interface ITrackMetadata
    {
        TimeSpan? TimeStamp { get; }

        string Title { get; }
        string Album { get; }
        string Artist { get; }
        int? Year { get; }
        string Genre { get; }
        Uri Website { get; }
    }

    public class TrackMetadata : ITrackMetadata
    {
        public TimeSpan? TimeStamp { get; set; }

        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public int? Year { get; set; }
        public string Genre { get; set; }
        public Uri Website { get; set; }

        public override string ToString()
        {
            var title = string.IsNullOrWhiteSpace(Title) ? "<null>" : '"' + Title + '"';
            var time = TimeStamp.HasValue ? TimeStamp.ToString() : "<null>";

            return "Track " + title + " @ " + time;
        }
    }
}

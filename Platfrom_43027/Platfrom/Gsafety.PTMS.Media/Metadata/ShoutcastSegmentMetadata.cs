namespace Gsafety.PTMS.Media.Metadata
{
    public interface IShoutcastSegmentMetadata : ISegmentMetadata
    {
        bool SupportsIcyMetadata { get; }
        int? IcyMetaInt { get; }
    }

    public class ShoutcastSegmentMetadata : SegmentMetadata, IShoutcastSegmentMetadata
    {
        public bool SupportsIcyMetadata { get; set; }
        public int? IcyMetaInt { get; set; }
    }
}

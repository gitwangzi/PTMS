namespace Gsafety.PTMS.Media.RTSP.Sdp
{
    /// <summary>
    /// Media Types used in SessionMediaDescription
    /// </summary>
    public enum MediaType : byte
    {
        unknown = 0,
        audio,
        video,
        image,//RFC6466 (T38) / rfc4145 / (rfc4751)
        text,
        timing,
        application,
        message,
        //
        data,
        control,
        //http://tools.ietf.org/html/rfc3840#section-10
        //automata,
        //Class, (business, personal, busipersonal)
        //Duplex,
        //Extensions,
        //mobility,
        //description,
        whiteboard //never specified in 4566 but referenced 3 total times
    }
}

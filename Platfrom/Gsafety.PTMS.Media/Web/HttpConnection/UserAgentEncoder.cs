namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public interface IUserAgentEncoder
    {
        string UsAsciiUserAgent { get; }
    }

    public class UserAgentEncoder : IUserAgentEncoder
    {
        readonly string _userAgent;

        public UserAgentEncoder(IUserAgent userAgent)
        {
            var name = userAgent.Name.Trim().Replace(' ', '_').Rfc2047Encode();
            var version = userAgent.Version.Trim().Replace(' ', '_').Rfc2047Encode();

            _userAgent = name + "/" + version;
        }

        public string UsAsciiUserAgent
        {
            get { return _userAgent; }
        }

    }
}

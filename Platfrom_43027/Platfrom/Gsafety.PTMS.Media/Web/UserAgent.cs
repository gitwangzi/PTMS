using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.Web
{
    public interface IUserAgent
    {
        string Name { get; }
        string Version { get; }
    }

    public class UserAgent : IUserAgent
    {
        readonly string _name;
        readonly string _version;

        public UserAgent(IApplicationInformation applicationInformation)
        {
            _name = applicationInformation.Title ?? "Unknown";
            _version = applicationInformation.Version ?? "0.0";
        }

        #region IUserAgent Members

        public string Name
        {
            get { return _name; }
        }

        public string Version
        {
            get { return _version; }
        }

        #endregion
    }
}

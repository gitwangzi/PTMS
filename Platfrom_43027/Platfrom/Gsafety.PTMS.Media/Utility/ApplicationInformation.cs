namespace Gsafety.PTMS.Media.Utility
{
    public class ApplicationInformation : IApplicationInformation
    {
        readonly string _title;
        readonly string _version;

        public ApplicationInformation(string title, string version)
        {
            _title = title ?? "Unknown";
            _version = version ?? "0.0";
        }

        public string Title
        {
            get { return _title; }
        }

        public string Version
        {
            get { return _version; }
        }
    }
}

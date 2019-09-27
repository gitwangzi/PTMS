using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.Content
{
    public class ContentType : IEquatable<ContentType>
    {
        readonly ICollection<string> _alternateMimeTypes;
        readonly ICollection<string> _fileExts;
        readonly ContentKind _kind;
        readonly string _mimeType;
        readonly string _name;

        public ContentType(string name, ContentKind kind, string mimeType, string fileExt, IEnumerable<string> alternateMimeTypes = null)
            : this(name, kind, mimeType, new[] { fileExt }, alternateMimeTypes)
        { }

        public ContentType(string name, ContentKind kind, string mimeType, IEnumerable<string> fileExts, IEnumerable<string> alternateMimeTypes = null)
        {
            if (null == name)
                throw new ArgumentNullException("name");
            if (mimeType == null)
                throw new ArgumentNullException("mimeType");
            if (null == fileExts)
                throw new ArgumentNullException("fileExts");

            _name = name;
            _kind = kind;
            _mimeType = mimeType;
            _alternateMimeTypes = null == alternateMimeTypes ? new List<string>() : alternateMimeTypes.ToList();
            _fileExts = fileExts.ToList();
        }

        public string Name
        {
            get { return _name; }
        }

        public ICollection<string> AlternateMimeTypes
        {
            get { return _alternateMimeTypes; }
        }

        public ICollection<string> FileExts
        {
            get { return _fileExts; }
        }

        public ContentKind Kind
        {
            get { return _kind; }
        }

        public string MimeType
        {
            get { return _mimeType; }
        }


        public bool Equals(ContentType other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (ReferenceEquals(null, other))
                return false;

            return string.Equals(_mimeType, other._mimeType, StringComparison.OrdinalIgnoreCase);
        }


        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_mimeType);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ContentType;

            if (ReferenceEquals(null, other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(ContentType a, ContentType b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(ContentType a, ContentType b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", _name, _mimeType);
        }
    }
}

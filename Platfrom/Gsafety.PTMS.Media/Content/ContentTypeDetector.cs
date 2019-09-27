using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.Media.Web;

namespace Gsafety.PTMS.Media.Content
{
    public interface IContentTypeDetector
    {
        ICollection<ContentType> GetContentType(Uri url, string mimeType = null, string fileName = null);
    }

    public class ContentTypeDetector : IContentTypeDetector
    {
        protected static readonly ContentType[] NoContent = new ContentType[0];
        protected readonly ContentType[] ContentTypes;
        protected readonly ILookup<string, ContentType> ExtensionLookup;
        protected readonly ILookup<string, ContentType> MimeLookup;

        public ContentTypeDetector(IEnumerable<ContentType> contentTypes)
        {
            if (null == contentTypes)
                throw new ArgumentNullException("contentTypes");

            ContentTypes = contentTypes.ToArray();

            ExtensionLookup = ContentTypes
                .SelectMany(ct => ct.FileExts, (ct, ext) => new
                {
                    ext,
                    ContentType = ct
                })
                .ToLookup(arg => arg.ext, x => x.ContentType, StringComparer.OrdinalIgnoreCase);

            var mimeTypes = ContentTypes
                .Select(ct => new
                {
                    ct.MimeType,
                    ContentType = ct
                });

            var alternateMimeTypes = ContentTypes
                .Where(ct => null != ct.AlternateMimeTypes)
                .SelectMany(ct => ct.AlternateMimeTypes, (ct, mime) => new
                {
                    MimeType = mime,
                    ContentType = ct
                });

            MimeLookup = alternateMimeTypes
                .Union(mimeTypes)
                .ToLookup(arg => arg.MimeType, x => x.ContentType, StringComparer.OrdinalIgnoreCase);
        }

        public virtual ICollection<ContentType> GetContentType(Uri url, string mimeType = null, string fileName = null)
        {
            if (null == url)
                throw new ArgumentNullException("url");

            var contentTypes = GetContentTypeByUrl(url);

            if (null != contentTypes && contentTypes.Any())
                return contentTypes;

            if (null != mimeType)
            {
                contentTypes = GetContentTypeByContentHeaders(mimeType);

                if (null != contentTypes)
                    return contentTypes;
            }

            if (string.IsNullOrWhiteSpace(fileName))
                return NoContent;

            contentTypes = GetContentTypeByFileName(fileName);

            return contentTypes ?? NoContent;
        }

        protected virtual ICollection<ContentType> GetContentTypeByUrl(Uri url)
        {
            var ext = url.GetExtension();

            if (null == ext)
                return null;

            return ExtensionLookup[ext].ToArray();
        }

        protected virtual ICollection<ContentType> GetContentTypeByContentHeaders(string mimeType)
        {
            if (null == mimeType)
                return null;

            return MimeLookup[mimeType].ToArray();
        }

        protected virtual ICollection<ContentType> GetContentTypeByFileName(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return null;

            var ext = UriExtensions.GetExtension(filename);

            return ExtensionLookup[ext].ToArray();
        }
    }
}

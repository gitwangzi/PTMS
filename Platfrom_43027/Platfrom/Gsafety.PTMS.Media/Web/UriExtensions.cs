using System;
using System.IO;

namespace Gsafety.PTMS.Media.Web
{
    public static class UriExtensions
    {
        static readonly char[] Slashes = { '/', '\\' };

        /// <summary>
        ///     Check if the URL contains a file extension that matches the argument.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static bool HasExtension(this Uri url, string extension)
        {
            if (!url.IsAbsoluteUri)
                return false;

            return url.LocalPath.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Get the file extension, including the period, of the URL's local path.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Extension or null</returns>
        public static string GetExtension(this Uri url)
        {
            if (!url.IsAbsoluteUri)
                return null;

            var path = url.LocalPath;

            var lastPeriod = path.LastIndexOf('.');

            if (lastPeriod <= 0 || lastPeriod + 1 == path.Length)
                return null;

            var lastSlash = path.LastIndexOfAny(Slashes);

            if (lastSlash >= lastPeriod)
                return null;

            return path.Substring(lastPeriod);
        }

        /// <summary>
        ///     Get the file extension, including the period, of the URL's local path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Extension or null</returns>
        public static string GetExtension(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            return Path.GetExtension(path);
        }
    }
}

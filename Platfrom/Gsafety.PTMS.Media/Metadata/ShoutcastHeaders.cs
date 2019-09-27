using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.Metadata
{
    class ShoutcastHeaders
    {
        readonly int? _bitrate;
        readonly string _description;
        readonly string _genre;
        readonly int? _metaInterval;
        readonly string _name;
        readonly bool _supportsIcyMetadata;
        readonly Uri _website;

        public ShoutcastHeaders(Uri streamUrl, IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
        {
            foreach (var header in headers)
            {
                switch (header.Key.ToLowerInvariant())
                {
                    case "icy-br":
                        foreach (var br in header.Value)
                        {
                            int bitrate;
                            if (int.TryParse(br, out bitrate))
                            {
                                if (bitrate > 0)
                                {
                                    _bitrate = bitrate * 1000;
                                    break;
                                }
                            }
                        }
                        break;
                    case "icy-description":
                        _description = header.Value.FirstOrDefault();
                        break;
                    case "icy-genre":
                        _genre = header.Value.FirstOrDefault();
                        break;
                    case "icy-metadata":
                        _supportsIcyMetadata = true;
                        break;
                    case "icy-metaint":
                        foreach (var metaint in header.Value)
                        {
                            int interval;
                            if (int.TryParse(metaint, out interval))
                            {
                                if (interval > 0)
                                {
                                    _metaInterval = interval;
                                    break;
                                }
                            }
                        }
                        break;
                    case "icy-name":
                        _name = header.Value.FirstOrDefault();
                        break;
                    case "icy-url":
                        foreach (var site in header.Value)
                        {
                            Uri url;
                            if (Uri.TryCreate(streamUrl, site, out url))
                            {
                                _website = url;
                                break;
                            }
                        }
                        break;
                }
            }
        }

        public int? Bitrate
        {
            get { return _bitrate; }
        }

        public string Description
        {
            get { return _description; }
        }

        public string Genre
        {
            get { return _genre; }
        }

        public int? MetaInterval
        {
            get { return _metaInterval; }
        }

        public string Name
        {
            get { return _name; }
        }

        public bool SupportsIcyMetadata
        {
            get { return _supportsIcyMetadata; }
        }

        public Uri Website
        {
            get { return _website; }
        }
    }
}

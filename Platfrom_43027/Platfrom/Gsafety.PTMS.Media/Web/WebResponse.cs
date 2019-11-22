using Gsafety.PTMS.Media.Content;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Media.Web
{
    public class WebResponse
    {
        public Uri RequestUri { get; set; }
        public long? ContentLength { get; set; }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }
        public ContentType ContentType { get; set; }
    }
}

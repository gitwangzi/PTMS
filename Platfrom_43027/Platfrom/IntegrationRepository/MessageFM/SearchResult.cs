using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gsafety.PTMS.Integration.Repository
{
    public class SearchResult
    {
        string uuid = string.Empty;

        public string Uuid
        {
            get { return uuid; }
            set { uuid = value; }
        }

        DateTime? createtime;

        public DateTime? CreateTime
        {
            get { return createtime; }
            set { createtime = value; }
        }
    }
}
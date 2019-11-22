using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Storage
{
    public class CacheManager
    {
        public static Dictionary<string, SuiteWorking> Suites = new Dictionary<string,SuiteWorking>();

        public static Dictionary<string, GPSWorking> GPSs = new Dictionary<string,GPSWorking>();

        public static Dictionary<string, MobileWorking> Mobiles = new Dictionary<string,MobileWorking>();

        public static Dictionary<string, string> District = new Dictionary<string, string>();
    }
}

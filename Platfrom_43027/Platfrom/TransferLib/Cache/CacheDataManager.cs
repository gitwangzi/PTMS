using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.TransferLib
{
    public class CacheDataManager
    {
        /// <summary>
        /// key district
        /// value url
        /// </summary>
        public static Dictionary<string, string> Districts = new Dictionary<string, string>();
        public static Dictionary<string, string> DistrictNames = new Dictionary<string, string>();
    }
}

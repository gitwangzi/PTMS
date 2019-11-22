using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.Util
{
    public static class SerialNoHelper
    {
        private static int hex = 65535;
        private static int ns = 1000000;
        public static int Create()
        {
            long current = DateTime.Now.Ticks;
            int i = Convert.ToUInt16(current % ns % hex);
            return i;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.TransferLib
{
    public class ReturnMessage
    {
        public string success { get; set; }

        public string data { get; set; }

        public ErrorDetail error { get; set; }

        public int dataCount { get; set; }
    }
}

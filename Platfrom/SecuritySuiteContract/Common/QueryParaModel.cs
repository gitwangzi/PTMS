using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    public class QueryParaModel : PageEntityBase
    {
        public string Experssion { get; set; }

        public string OrderBy { get; set; }

        public object[] Paras { get; set; }


        public QueryParaModel()
            : base()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Analysis.Helper
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RouteAttribute : Attribute
    {
        public string TypeName { get; private set; }

        public RouteAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}

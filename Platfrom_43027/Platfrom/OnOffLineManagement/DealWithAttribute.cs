using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnOffLineManagement
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DealWithAttribute : Attribute
    {
        public string TypeName { get; private set; }

        public DealWithAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}

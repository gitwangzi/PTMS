using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.SecuritySuite.Contract
{
    public abstract class EntityModelBase<T> : PageEntityBase where T : EntityItemBase
    {
        protected EntityModelBase()
        {
            this.Items = new List<T>();
        }

        public string Error { get; set; }

        public List<T> Items { get; set; }

    }
}

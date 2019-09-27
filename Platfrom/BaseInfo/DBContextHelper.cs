using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gsafety.PTMS.DBEntity;
using System.Data.Entity;

namespace Gsafety.PTMS.BaseInfo
{
    public static class DBContextHelper
    {
        public static SingleMessage<bool> Save(this DbContext context)
        {
            if (context.SaveChanges() > 0)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.SaveDBFailed);
            }
        }
    }
}

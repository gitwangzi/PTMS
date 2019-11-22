using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gsafety.PTMS.BaseInfo.Conditions.Interface
{
    /// <summary>
    /// Attrbute interface
    /// </summary>
   public interface IPropertyCondition
    {
       /// <summary>
       /// Get Value
       /// </summary>
       /// <returns>value</returns>
       List<string> ToValue();
       /// <summary>
       /// Convert to sql
       /// </summary>
       /// <returns>sql</returns>
       string ToSql();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.MakerContions.Enums;
using System.Linq.Expressions;

namespace Gsafety.PTMS.BaseInfo.MakerContions.Interface
{
    /// <summary>
    /// Interface conditions
    /// </summary>
    public interface IConditon
    {
        Expression ToFilterLinq<T>(ParameterExpression param);
    }
}

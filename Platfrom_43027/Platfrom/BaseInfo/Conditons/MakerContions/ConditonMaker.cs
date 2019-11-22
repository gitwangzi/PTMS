using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;

namespace Gsafety.PTMS.BaseInfo.Conditions
{
    /// <summary>
    /// Conditon Maker
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class ConditonMaker<T>
    {
        public Expression<Func<T, bool>> MakeCondtions(CondtionItem bpc, string Parameter)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), Parameter);
            Expression filter = bpc.ToFilterLinq<T>(param);

            // Expression pred = Expression.Lambda(filter, param);
            Expression<Func<T, bool>> condtion = Expression.Lambda<Func<T, bool>>(filter, new ParameterExpression[] { param });
            return condtion;
        }
        public Expression<Func<T, bool>> MakeCondtions(CondtionItem bpc)
        {
            return MakeCondtions(bpc, "A");
        }
    }
}

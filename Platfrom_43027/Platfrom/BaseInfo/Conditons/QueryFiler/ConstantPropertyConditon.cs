using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo.Conditons.QueryFiler
{

    public class ConstantPropertyConditon : BasePropertyCondition
    {
        private bool m_Request;
        public ConstantPropertyConditon(bool Request)
            : base(string.Empty, MakerContions.Enums.OperateEnum.equal)
        {
            m_Request = Request;

        }

        /// <summary>
        /// Create expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public override System.Linq.Expressions.Expression ToFilterLinq<T>(ParameterExpression param)
        {
            Expression filter = null;
            if (m_Request)
            {

                filter = Expression.Constant(true);
            }
            else
            {
                filter = Expression.Constant(false);
            }

            return filter;
        }
    }
}

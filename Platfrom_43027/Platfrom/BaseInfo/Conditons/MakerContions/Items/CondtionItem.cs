using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.MakerContions.Interface;
using Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums;
using System.Linq.Expressions;

namespace Gsafety.PTMS.BaseInfo.MakerContions.Items
{
    /// <summary>
    /// CondtionItem
    /// </summary>
    public abstract class CondtionItem : IConditon
    {

        private LogicSymbol m_logicSymbol = LogicSymbol.AND;

        public LogicSymbol LogicSymbol
        {
            get { return m_logicSymbol; }
            set { m_logicSymbol = value; }
        }

        /// <summary>
        /// Cenerate expression
        /// </summary>
        /// <param name="left"></param>
        /// <returns></returns>
        public virtual System.Linq.Expressions.Expression ToFilterLinq<T>(ParameterExpression param)
        {
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.MakerContions.Enums;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.BaseInfo.Conditions.QueryFiler
{
    public sealed class DateTimePropertyConditon : BasePropertyCondition
    {

        private DateTime m_propertyMinValue;

        private DateTime m_propertyMaxValue;

        /// <summary>
        /// max value
        /// </summary>

        public DateTime PropertyMaxValue
        {
            get { return m_propertyMaxValue; }
            set { m_propertyMaxValue = value; }
        }
        /// <summary>
        /// min value
        /// </summary>
        public DateTime PropertyMinValue
        {
            get { return m_propertyMinValue; }
            set { m_propertyMinValue = value; }
        }

        public DateTimePropertyConditon(string propertyName, OperateEnum operate, DateTime propertyMinValue, DateTime propertyMaxValue)
            : base(propertyName, operate)
        {

            m_propertyMinValue = propertyMinValue;
            m_propertyMaxValue = propertyMaxValue;

        }

        public override Expression ToFilterLinq<T>(ParameterExpression param)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(PropertyName));

            switch (Operate)
            {
                case OperateEnum.betweenness:
                    left = Expression.Property(left, "Value");

                    Expression rightMin = Expression.Constant(m_propertyMinValue, typeof(DateTime));

                    Expression millde = Expression.Call(left, typeof(DateTime).GetMethod("CompareTo", new Type[] { typeof(DateTime) }), rightMin);

                    Expression filterMin = Expression.GreaterThanOrEqual(millde, Expression.Constant(0));//, Expression.Constant(true), Expression.Constant(false));

                    Expression rightmax = Expression.Constant(m_propertyMaxValue, typeof(DateTime));

                    Expression otherMillde = Expression.Call(left, typeof(DateTime).GetMethod("CompareTo", new Type[] { typeof(DateTime) }), rightmax);

                    Expression filterMax = Expression.LessThanOrEqual(otherMillde, Expression.Constant(0));

                    return Expression.And(filterMin, filterMax);
                default:
                    break;
            }
            return null;
        }

     
    }
}

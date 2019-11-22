using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.MakerContions.Enums;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Gsafety.PTMS.BaseInfo.Conditions.QueryFiler
{
    public class StringPropertyConditon : BasePropertyCondition
    {
        private string m_propertyValue;

        public string PropertyValue
        {
            get { return m_propertyValue; }
            set { m_propertyValue = value; }
        }


        public StringPropertyConditon(string propertyName, OperateEnum operate, object propertyValue)
            : base(propertyName, operate)
        {

            m_propertyValue = propertyValue == null ? string.Empty : propertyValue.ToString();
   
        }

        public override System.Linq.Expressions.Expression ToFilterLinq<T>(ParameterExpression param)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(PropertyName));
            Expression right = Expression.Constant(m_propertyValue);
            Expression filter = null;
            switch (Operate)
            { 
                case OperateEnum.equal:
                     filter = Expression.Equal(left, right);
                     break;
                case OperateEnum.Contain:
                     filter = Expression.Call(left,typeof(string).GetMethod("Contains",new Type[]{typeof(string)}),right);
                     break;
                case OperateEnum.StartsWith:
                     filter = Expression.Call(left, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), right);
                     break;
                case OperateEnum.inValue:
                     filter = Expression.Call(right, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), left);
                     break;
                case OperateEnum.unequal:
                     filter = Expression.NotEqual(left, right);//, Expression.Constant(true), Expression.Constant(false));
                     break;
            
            }
        
            return filter;
        }
     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.Conditions.Interface;
using Gsafety.PTMS.BaseInfo.MakerContions.Enums;
using Gsafety.PTMS.BaseInfo.MakerContions.Interface;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums;


namespace Gsafety.PTMS.BaseInfo.Conditions.QueryFiler
{
    /// <summary>
    /// Attributes
    /// </summary>

    public  abstract class BasePropertyCondition :CondtionItem, IPropertyCondition
    {
        private string m_propertyName;
        /// <summary>
        /// Attributes name
        /// </summary>

        public string PropertyName
        {
            get { return m_propertyName; }
            set { m_propertyName = value; }
        }

        private OperateEnum m_operate;

        /// <summary>
        /// operation
        /// </summary>

        public OperateEnum Operate
        {
            get { return m_operate; }
            set { m_operate = value; }
        }
        /// <summary>
        /// cotr
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="operate"></param>
        /// <param name="propertyValue"></param>
        public BasePropertyCondition(string propertyName, OperateEnum operate)
        {
            m_operate = operate;

            m_propertyName = propertyName;
        }
        /// <summary>
        /// Convert to value
        /// </summary>
        /// <returns>value</returns>
        public virtual List<string> ToValue()
        {
            return null;
        }
        /// <summary>
        /// Convert to sql
        /// </summary>
        /// <returns>sql</returns>
        public virtual string ToSql()
        {
            return null;
        }
        public override System.Linq.Expressions.Expression ToFilterLinq<T>(ParameterExpression param)
        {
            return null;
        }

      
    }
}

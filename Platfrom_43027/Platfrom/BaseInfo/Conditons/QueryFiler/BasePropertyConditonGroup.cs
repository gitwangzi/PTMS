using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.BaseInfo.MakerContions.Interface;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using Gsafety.PTMS.BaseInfo.Conditions.MakerContions.Enums;
using System.Linq.Expressions;

namespace Gsafety.PTMS.BaseInfo.Conditions.QueryFiler
{
    /// <summary>
    /// Attribute query condition group
    /// </summary>
    public class BasePropertyConditonGroup : CondtionItem, IConditon
    {
        /// <summary>
        /// query condition group
        /// </summary>
        private List<CondtionItem> m_BasePropertyConditionList;

        /// <summary>
        /// logic
        /// </summary>
        private LogicSymbol m_logicSymbol = LogicSymbol.AND;

   
        public BasePropertyConditonGroup(LogicSymbol logicSymbol, params BasePropertyCondition[] basePropertyConditions)
        {
            m_logicSymbol = logicSymbol;
            m_BasePropertyConditionList = new List<CondtionItem>();
            if (null != basePropertyConditions && basePropertyConditions.Length > 0)
            {
                string groupID = Guid.NewGuid().ToString();

                for (int i = 0; i < basePropertyConditions.Length; i++)
                {

                    basePropertyConditions[i].LogicSymbol = m_logicSymbol;

                    m_BasePropertyConditionList.Add(basePropertyConditions[i]);
                }

            }

        }

        public BasePropertyConditonGroup(LogicSymbol logicSymbol, List<CondtionItem> basePropertyConditions)
        {
            m_logicSymbol = logicSymbol;
            m_BasePropertyConditionList = new List<CondtionItem>();
            if (null != basePropertyConditions && basePropertyConditions.Count > 0)
            {
                string groupID = Guid.NewGuid().ToString();

                for (int i = 0; i < basePropertyConditions.Count; i++)
                {

                    basePropertyConditions[i].LogicSymbol = m_logicSymbol;

                    m_BasePropertyConditionList.Add(basePropertyConditions[i]);
                }

            }

        }

        public BasePropertyConditonGroup(LogicSymbol logicSymbol, List<BasePropertyCondition> basePropertyConditions)
        {
            m_logicSymbol = logicSymbol;
            m_BasePropertyConditionList = new List<CondtionItem>();
            if (null != basePropertyConditions && basePropertyConditions.Count > 0)
            {

                string groupID = Guid.NewGuid().ToString();

                foreach (BasePropertyCondition bpc in basePropertyConditions)
                {

                    bpc.LogicSymbol = m_logicSymbol;



                    m_BasePropertyConditionList.Add(bpc);
                }

            }
        }
        public override System.Linq.Expressions.Expression ToFilterLinq<T>(ParameterExpression param)
        {
            Expression filer = null;
            if (null != m_BasePropertyConditionList && m_BasePropertyConditionList.Count > 0)
            {

                foreach (BasePropertyCondition bpc in m_BasePropertyConditionList)
                {
                    Expression falg = bpc.ToFilterLinq<T>(param);

                    if (null == filer)
                    {
                        filer = falg;
                        continue;
                    }
                    switch (m_logicSymbol)
                    {
                        case LogicSymbol.AND:
                            filer = Expression.And(filer, falg);
                            break;
                        case LogicSymbol.OR:
                            filer = Expression.Or(filer, falg);
                            break;

                    }


                }

            }

            return filer;
        }


    }
}

using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.Interface;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo.Conditons.ValueAnalys
{
    /// <summary>
    /// Safety verification
    /// </summary>
    public class AuthorityValue : IValueAnaly
    {
        protected short? m_userType;
        protected string m_regions;
        protected string m_province;
        protected string m_city;
        public AuthorityValue(short? userType, string regions, string province, string city)
        {

            m_userType = userType;
            m_regions = regions;
            m_province = province;
            m_city = city;
        }

 
        public virtual CondtionItem ToConditonItem()
        {

            string propertyName = string.Empty;
            switch (m_userType)
            {
                case -1:
                    propertyName = GlobalData.COMPANY_PROPERTY_NAME;
                    break;
                case 0:
                    propertyName = GlobalData.PROVINCE_PROPERTY_NAME;
                    break;
                case 1:
                case 2:
                    propertyName = GlobalData.CITY_PROPERTY_NAME;
                    break;
                default:
                    propertyName = string.Empty;
                    break;
            }
            return ToItemByPropertyName(propertyName);
        }

        protected CondtionItem ToItemByPropertyName(string propertyName)
        {
            CondtionItem ansItem = null;
            if (!string.IsNullOrEmpty(m_regions))
            {
                if (m_regions.Trim().Equals("*"))
                {
                    if (m_userType == 1)
                    {
                        ansItem = new StringPropertyConditon("CODE", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.StartsWith, m_province);
                    }
                    else if (m_userType == 2)
                    {
                        ansItem = new StringPropertyConditon("CODE", Gsafety.PTMS.BaseInfo.MakerContions.Enums.OperateEnum.StartsWith, m_city);
                    }
                    else
                    {
                        ansItem = new ConstantPropertyConditon(true);
                    }

                }
                else
                {
                    string[] separator = { ", " };
                    string[] regions = m_regions.Trim().Split(separator, StringSplitOptions.None);
                    List<CondtionItem> conditons = new List<CondtionItem>();
                    foreach (string regionItem in regions)
                    {
                        string[] regionParents = regionItem.Split('-');
                        if (regionParents.Length == 1)
                        {
                            ansItem = new StringPropertyConditon(propertyName, MakerContions.Enums.OperateEnum.StartsWith, regionItem.Trim());
                            conditons.Add(ansItem);
                        }
                        else
                        {
                            if (regionParents.Length == 2)
                            {
                                ansItem = new StringPropertyConditon(propertyName == GlobalData.DISTRICT_CODE ? propertyName : GlobalData.PROVINCE_PROPERTY_NAME, MakerContions.Enums.OperateEnum.equal, regionParents[0].Trim());
                                conditons.Add(ansItem);
                                ansItem = new StringPropertyConditon(propertyName, MakerContions.Enums.OperateEnum.StartsWith, regionItem.Trim());
                                conditons.Add(ansItem);
                            }
                        }
                    }
                    return new BasePropertyConditonGroup(Conditions.MakerContions.Enums.LogicSymbol.OR, conditons);
                }
            }
            else
            {
                ansItem = new ConstantPropertyConditon(false);
            }
            return ansItem;
        }
    }
}

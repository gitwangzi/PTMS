/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4fb6e040-7c4c-4b36-8896-12e34rfgty76     
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInfo
/////    Project Description:    
/////             Class Name: BaseRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 10/12/2013 2:02:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 10/12/2013 2:02:48 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================

using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo.Conditions;
using Gsafety.PTMS.BaseInfo.Conditions.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.Interface;
using Gsafety.PTMS.BaseInfo.Conditons.QueryFiler;
using Gsafety.PTMS.BaseInfo.Conditons.ValueAnalys;
using Gsafety.PTMS.BaseInfo.MakerContions.Items;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo
{
    public class BaseRepository
    {
        #region Fields

        UserInfoMessageHeader _UserInfo = new UserInfoMessageHeader();

        #endregion

        #region Attributes

        public UserInfoMessageHeader UserInfo
        {
            get { return _UserInfo; }
            set { _UserInfo = value; }
        }

        #endregion

        /// <summary>
        /// Data filtering condition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> MakeUserAuthorityCondtions<T>()
        {
            return MakeCondtions<T>(ConditonType.Other);
        }

        /// <summary>
        /// Data fitering for the administative region query
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Expression<Func<T, bool>> MakeDistrictCondtions<T>(UserInfoMessageHeader userInfo)
        {
            _UserInfo = userInfo;

            return MakeCondtions<T>(ConditonType.District);
        }

        public SingleMessage<bool> InsertLogError(PTMSEntities context,string id,DateTime createTime,string reason)
        {
            LOG_ERROR item = new LOG_ERROR();
            item.CREATE_TIME = createTime;
            if(reason.Length > 4000)
            {
                item.ERROR_REASON = reason.Substring(0, 3000);
            }
            else
            {
                item.ERROR_REASON = reason;
            }

            item.ID = id;
            context.LOG_ERROR.Add(item);
            int ret = context.SaveChanges();
            if (ret > 0)
            {
                return new SingleMessage<bool>(true);
            }
            else
            {
                return new SingleMessage<bool>(false, "");
            }
        }

        private Expression<Func<T, bool>> MakeCondtions<T>(ConditonType ConditonType)
        {
            string[] separator = { ", " };
            ConditonMaker<T> cdm = new ConditonMaker<T>();
            //USER_AUTHORITY  userAuthority;
            //using (PTMSEntities context = new PTMSEntities())
            //{
            //    var result = from x in context.USER_AUTHORITY
            //                 where x.USER_NAME == _UserInfo.UserName
            //                 select x;
            //    userAuthority = result.FirstOrDefault();//.ToList().FirstOrDefault;
            //}
            if (null != _UserInfo)
            {
                IValueAnaly avalue = null;
                if (ConditonType == BaseInfo.ConditonType.Other)
                {
                    avalue = new AuthorityValue((short)_UserInfo.Level, _UserInfo.Region, _UserInfo.Province,_UserInfo.City);
                }
                else
                {
                    //if (_UserInfo.Level == -1 && !string.IsNullOrEmpty(_UserInfo.Region))
                    //{

                    //    List<VEHICLE_COMPANY> vehicle_company;
                    //    string[] regions = _UserInfo.Region.Split(separator, StringSplitOptions.None);
                    //    using (PTMSEntities context = new PTMSEntities())
                    //    {
                    //        var result = from x in context.VEHICLE_COMPANY
                    //                     where regions.Contains(x.ID)
                    //                     select x;
                    //        vehicle_company = result.ToList();//.ToList().FirstOrDefault;
                    //    }
                    //    if (null != vehicle_company)
                    //    {
                    //        _UserInfo.Region = string.Empty;
                    //        for (int i = 0; i < vehicle_company.Count; i++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                _UserInfo.Region += vehicle_company[i].DISTRICT_CODE;
                    //                continue;
                    //            }
                    //            _UserInfo.Region += ", " + vehicle_company[i].DISTRICT_CODE;
                    //        }
                    //    }
                    //}
                    avalue = new DistrictValue((short)_UserInfo.Level, _UserInfo.Region, _UserInfo.Province,_UserInfo.City);
                }
                return cdm.MakeCondtions(avalue.ToConditonItem());
            }
            return cdm.MakeCondtions(new ConstantPropertyConditon(false));
        }

        public Expression<Func<DISTRICT_LEVEL_VIEW, bool>> GetDistrictExpression(UserInfoMessageHeader userInfo)
        {
            string[] separator = { ", " };
            Expression<Func<DISTRICT_LEVEL_VIEW, bool>> Exp = null;
            int intUserType = userInfo.Level;
            string strRegion = userInfo.Region;
            switch (intUserType)
            {
                //case (int)UserAuthorityType.CityLevel:
                case 2:
                    Exp = (x => x.CODE == strRegion);
                    break;
                //case (int)UserAuthorityType.ProvinceLevel:
                case 1:
                    if (strRegion == "*")
                    {
                        string strProvince = userInfo.Province;
                        Exp = (x => x.CODE.Length == 5 && x.CODE.Substring(0, 2) == strProvince);
                    }
                    else
                    {
                        string[] arrayCityCode = strRegion.Split(separator, StringSplitOptions.None);
                        Exp = (x => arrayCityCode.Contains(x.CODE));
                    }
                    break;
                //case (int)UserAuthorityType.CountryLevel:
                case 0:
                    if (strRegion == "*")
                    {
                        Exp = (x => x.CODE.Length == 5);
                    }
                    else
                    {
                        string[] arrayProvinceCode = strRegion.Split(separator, StringSplitOptions.None);
                        Exp = (x => x.CODE.Length == 5 && arrayProvinceCode.Contains(x.CODE.Substring(0, 2)));
                    }
                    break;
                default:
                    Exp = (x => x.CODE.Length == 5);
                    break;

            }
            return Exp;
        }

        //public Expression<Func<VEHICLE, bool>> GetDistrictExp(UserInfoMessageHeader userInfo)
        //{
        //    string[] separator = { ", " };
        //    Expression<Func<VEHICLE, bool>> Exp = null;
        //    int intUserType = userInfo.Level;
        //    string strRegion = userInfo.Region;
        //    switch (intUserType)
        //    {
        //        //case (int)UserAuthorityType.CityLevel:
        //        case 2:
        //            Exp = (x => x.DISTRICT_CODE == strRegion);
        //            break;
        //        //case (int)UserAuthorityType.ProvinceLevel:
        //        case 1:
        //            if (strRegion == "*")
        //            {
        //                string strProvince = userInfo.Province;
        //                Exp = (x => x.DISTRICT_CODE.Length == 5 && x.DISTRICT_CODE.Substring(0, 2) == strProvince);
        //            }
        //            else
        //            {
        //                string[] arrayCityCode = strRegion.Split(separator, StringSplitOptions.None);
        //                Exp = (x => arrayCityCode.Contains(x.DISTRICT_CODE));
        //            }
        //            break;
        //        //case (int)UserAuthorityType.CountryLevel:
        //        case 0:
        //            if (strRegion == "*")
        //            {
        //                Exp = (x => x.DISTRICT_CODE.Length == 5);
        //            }
        //            else
        //            {
        //                string[] arrayProvinceCode = strRegion.Split(separator, StringSplitOptions.None);
        //                Exp = (x => x.DISTRICT_CODE.Length == 5 && arrayProvinceCode.Contains(x.DISTRICT_CODE.Substring(0, 2)));
        //            }
        //            break;
        //        default:
        //            Exp = (x => x.DISTRICT_CODE.Length == 5);
        //            break;

        //    }
        //    return Exp;
        //}

        //public Expression<Func<VEHICLE_COMPANY, bool>> GetCompanyExpression(UserInfoMessageHeader userInfo)
        //{
        //    string strCompanyId = userInfo.CompanyId;
        //    Expression<Func<VEHICLE_COMPANY, bool>> Exp = (x => x.ID == strCompanyId && x.VALID == 1);
        //    return Exp;
        //}
    }

    public enum ConditonType
    {
        District,
        Other
    }
}

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a15bc7df-c952-4f39-9bb1-0a7e4e405e2f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation.Repository
/////    Project Description:    
/////             Class Name: DistrictRepository
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/19 17:58:10
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/19 17:58:10
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    public class DistrictRepository : Gsafety.PTMS.BaseInfo.BaseRepository
    {
        public DistrictRepository()
        {
        }

        public List<District> GetProvinceAndCity(PTMSEntities context)
        {
            var result = from x in context.BSC_DISTRICT
                         where x.CODE.Length == 2 || x.CODE.Length == 5
                         select new District
                         {
                             Code = x.CODE,
                             Name = x.NAME,
                         };
            return result.ToList();
        }



        public List<District> GetDistrict(PTMSEntities context)
        {

            var result = from x in context.BSC_DISTRICT
                         select new District
                         {
                             Code = x.CODE,
                             Name = x.NAME,
                         };
            return result.ToList();

        }

        /// <summary>
        /// Get District By Authority
        /// </summary>
        /// <returns></returns>
        public List<District> GetDistrictByAuthority(PTMSEntities context, UserInfoMessageHeader userInfo)
        {

            var result = from x in context.BSC_DISTRICT
                         join y in context.DISTRICT_LEVEL_VIEW.Where(base.MakeDistrictCondtions<DISTRICT_LEVEL_VIEW>(userInfo)) on x.CODE equals y.CODE
                         select new District
                         {
                             Code = x.CODE,
                             Name = x.NAME,
                         };
            return result.ToList();

        }

        public District GetDistricByCode(PTMSEntities context, string code)
        {

            if (code.Length == 2)
            {
                var result = (from x in context.BSC_DISTRICT
                              where x.CODE == code
                              select new District
                              {
                                  Code = x.CODE,
                                  Name = x.NAME,
                                  ParentCode = string.Empty,
                              }).FirstOrDefault();
                return result;
            }
            else
            {
                var result = (from x in context.BSC_DISTRICT
                              join y in context.BSC_DISTRICT on x.CODE.Substring(0, 2) equals y.CODE
                              where x.CODE == code
                              select new District
                         {
                             Code = x.CODE,
                             Name = x.NAME,
                             ParentCode = y.CODE,
                             ParentName = y.NAME,
                         }).FirstOrDefault();
                return result;
            }
        }

        public List<UserAuthority> GetUserAuthorityFuzzy(PTMSEntities _context, string loginName, PagingInfo page, out int totalRecord)
        {

            //var result = (from x in _context.USER_AUTHORITY
            //              where (string.IsNullOrEmpty(loginName) ? true : x.USER_NAME.Contains(loginName)) &&
            //                    (x.USER_TYPE == (short)UserAuthorityType.CountryLevel || x.USER_TYPE == (short)UserAuthorityType.ProvinceLevel) &&
            //                    (x.USER_GROUP != "SecurityAdmin")
            //              select new UserAuthority
            //              {
            //                  ID = x.ID,
            //                  LoginName = x.USER_NAME,
            //                  RegionsCode = x.REGIONS,
            //                  SecurityGroup = x.USER_GROUP,
            //                  UserType = (UserAuthorityType)x.USER_TYPE,
            //              });
            //List<UserAuthority> Result = new List<UserAuthority>();
            ////-1 get all
            //if (page == null || page.PageIndex == -1)
            //{
            //    totalRecord = result.Count();
            //    Result = result.ToList();
            //}
            //else
            //{
            //    totalRecord = result.Count();
            //    Result = result.OrderBy(x => x.LoginName).Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
            //}
            //string symbol = ", ";
            //string[] separator = { symbol };
            //List<District> ProvinceCityList = GetProvinceAndCity(_context);
            //Result.ForEach(item =>
            //{
            //    switch (item.RegionsCode)
            //    {
            //        case "*":
            //            item.RegionsName = "*";
            //            break;
            //        case null:
            //            item.RegionsName = string.Empty;
            //            break;
            //        default:
            //            string[] RegionCodeList = item.RegionsCode.Split(separator, StringSplitOptions.None);

            //            RegionCodeList.ToList().ForEach(code =>
            //            {
            //                var temp = ProvinceCityList.FirstOrDefault(district => district.Code == code);
            //                if (temp != null)
            //                {
            //                    item.RegionsName += temp.Name + symbol;
            //                }
            //            });
            //            if (!string.IsNullOrEmpty(item.RegionsName) && (item.RegionsName.Length > symbol.Length))
            //            {
            //                item.RegionsName = item.RegionsName.Substring(0, item.RegionsName.Length - symbol.Length);
            //            }
            //            break;
            //    }
            //});
            //return Result;
            totalRecord = 0;
            return null;
        }

        public bool DeleteUserAuthority(PTMSEntities context, string loginName)
        {

            //var result = context.USER_AUTHORITY.Where(x => x.USER_NAME == loginName).FirstOrDefault();
            //if (result != null)
            //{
            //    context.USER_AUTHORITY.Remove(result);
            //    context.SaveChanges();
            //}
            return true;

        }

        public bool AddUserAuthority(PTMSEntities context, UserAuthority userAuthority)
        {
            //USER_AUTHORITY item = new USER_AUTHORITY
            //{
            //    ID = Guid.NewGuid().ToString(),
            //    REGIONS = userAuthority.RegionsCode,
            //    USER_NAME = userAuthority.LoginName,
            //    USER_TYPE = (short)userAuthority.UserType,
            //    USER_GROUP = userAuthority.SecurityGroup,
            //};

            //context.USER_AUTHORITY.Add(item);
            //context.SaveChanges();
            return true;

        }

        public bool UpdateUserAuthority(PTMSEntities context, UserAuthority userAuthority)
        {

            //var result = context.USER_AUTHORITY.Where(x => x.USER_NAME == userAuthority.LoginName).FirstOrDefault();
            //if (result != null)
            //{
            //    result.REGIONS = userAuthority.RegionsCode;
            //    result.USER_TYPE = (short)userAuthority.UserType;
            //    result.USER_GROUP = userAuthority.SecurityGroup;
            //    context.SaveChanges();
            //}
            return true;

        }

        public UserAuthority GetUserAuthorityByName(PTMSEntities context, string name)
        {

            //var result = (from x in context.USER_AUTHORITY
            //              where x.USER_NAME == name
            //              select new UserAuthority
            //              {
            //                  ID = x.ID,
            //                  UserName = x.USER_NAME,
            //                  UserType = (UserAuthorityType)x.USER_TYPE,
            //                  RegionsCode = x.REGIONS,
            //                  SecurityGroup = x.USER_GROUP,
            //              }).FirstOrDefault();
            //return result;
            return null;

        }

        public List<FuncItem> GetFuncItem(PTMSEntities context)
        {

            //var result = (from x in context.FUNC_ITEM
            //              select new FuncItem
            //              {
            //                  Id = x.ID,
            //                  Name = x.NAME,
            //                  Note = x.NOTE,
            //                  ParendID = x.PARENDID
            //              }).ToList();
            //return result;
            return null;

        }

        public RoleFuncs GetFuncByRole(PTMSEntities context, string roleName)
        {

            //var result = (from x in context.ROLE_FUNCS
            //              where x.ROLE.Equals(roleName)
            //              select new RoleFuncs
            //              {
            //                  Id = x.ID,
            //                  RoleName = x.ROLE,
            //                  FuncId = x.FUNC_ID,
            //                  Creator = x.CREATOR,
            //                  CreateDate = x.CREATE_TIME
            //              }).FirstOrDefault();
            //return result;
            return null;

        }

        public bool UpdateRoleFunc(PTMSEntities context, RoleFuncs roleFunc)
        {

            //var result = (from x in context.ROLE_FUNCS
            //              where x.ROLE.Equals(roleFunc.RoleName)
            //              select x).FirstOrDefault();
            //if (result == null)
            //{
            //    ROLE_FUNCS item = new ROLE_FUNCS
            //    {
            //        ID = Guid.NewGuid().ToString(),
            //        ROLE = roleFunc.RoleName,
            //        FUNC_ID = roleFunc.FuncId,
            //        CREATOR = roleFunc.Creator,
            //        CREATE_TIME = roleFunc.CreateDate
            //    };

            //    context.ROLE_FUNCS.Add(item);
            //    context.SaveChanges();
            //}
            //else
            //{
            //    result.FUNC_ID = roleFunc.FuncId;
            //    result.CREATOR = roleFunc.Creator;
            //    result.CREATE_TIME = roleFunc.CreateDate;
            //    context.SaveChanges();
            //}

            return true;
        }

    }
}

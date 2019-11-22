using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.Common.AD;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Linq.Expressions;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gs.PTMS.Service
{
    public class GroupService : BaseService, IGroupService
    {
        ADUserMange usermanager = null;
        public GroupService()
        {
            string dominName = ConfigurationManager.AppSettings["DomainName"];
            string ADuserName = ConfigurationManager.AppSettings["ADAdministrator"];
            string ADpassword = ConfigurationManager.AppSettings["ADPassword"];
            string AdRootPath = ConfigurationManager.AppSettings["ADRootPath"];
            string ADRootPwd = ConfigurationManager.AppSettings["ADRootPwd"];
            string ADRootAdmin = ConfigurationManager.AppSettings["ADRootAdmin"];
            string ADServiceConStr = ConfigurationManager.ConnectionStrings["ADService"].ConnectionString;
            usermanager = new ADUserMange(dominName, ADuserName, ADpassword, AdRootPath, ADRootPwd, ADRootAdmin, ADServiceConStr);
        }
        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddGroup(string groupName)
        {

            try
            {
                Info("AddGroup");
                Info("groupName:" + Convert.ToString(groupName));
                var isSuc = usermanager.AddGroup(groupName);
                SingleMessage<bool> result = new SingleMessage<bool> { Result = isSuc };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { ExceptionMessage = ex, Result = false };
            }
        }
        /// <summary>
        /// Delete Group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public SingleMessage<bool> DeleteGroup(string groupName)
        {

            try
            {
                Info("DeleteGroup");
                Info("groupName:" + Convert.ToString(groupName));
                var isSuc = usermanager.DeleteAccount(groupName);
                SingleMessage<bool> result = new SingleMessage<bool> { Result = isSuc };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { Result = false, ExceptionMessage = ex };
            }

        }
        /// <summary>
        /// Get All Group Names
        /// </summary>
        /// <returns></returns>
        public MultiMessage<string> GetAllGroupNames()
        {

            try
            {
                Info("GetAllGroupNames");
                var list = usermanager.GetAllgroupnames();
                MultiMessage<string> result = new MultiMessage<string> { Result = list };
                Log<string>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<string> { ExceptionMessage = ex, Result = null };
            }
        }
        /// <summary>
        /// Get User By Group Name
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public MultiMessage<ADAccountInfo> GetAccountInfoByGroupName(string groupName)
        {
            List<ADAccountInfo> infolist = new List<ADAccountInfo>();
            List<ADAccountEntity> list = new List<ADAccountEntity>();
            try
            {
                Info("GetAccountInfoByGroupName");
                Info("groupName:" + Convert.ToString(groupName));
                if (groupName.Equals("SecurityManager"))
                {
                    list = usermanager.GetUserInfoBygroupname("SecurityMonitor");
                    foreach (var item in list)
                    {
                        if (item == null) continue;
                        item.SecurityGroup = "SecurityManager";
                        infolist.Add(GetRepackUserinfo(item));
                    }
                }
                list = usermanager.GetUserInfoBygroupname(groupName);
                foreach (var item in list)
                {
                    infolist.Add(GetRepackUserinfo(item));
                }
                //  infolist = infolist.OrderBy(o => o.UserLoginName);
                infolist = new List<ADAccountInfo>(infolist.OrderBy(o => o.UserLoginName));
                MultiMessage<ADAccountInfo> result = new MultiMessage<ADAccountInfo> { Result = infolist };
                Log<ADAccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ADAccountInfo>() { Result = null };
            }

        }
        public ADAccountInfo GetRepackUserinfo(ADAccountEntity info)
        {
            using (PTMSEntities context = new PTMSEntities())
            {

                DistrictRepository regionmanager = new DistrictRepository();
                InstallStationRepository SetupManager = new InstallStationRepository();
                ADAccountInfo resultmodel = new ADAccountInfo();
                if (info != null)
                {
                    resultmodel.UserName = info.DisplayName;
                    resultmodel.DisplayName = info.DisplayName;
                    resultmodel.UserLoginName = info.UserName;
                    resultmodel.Phone = info.Phone;
                    resultmodel.Email = info.Email;
                    resultmodel.Address = info.Address;
                    resultmodel.Description = info.Description;
                    if (string.IsNullOrEmpty(info.SecurityGroup))
                    {
                        return resultmodel;
                    }

                    resultmodel.SecurityGroup = info.SecurityGroup;
                    if (resultmodel.SecurityGroup.Equals("SecurityMonitor") || resultmodel.SecurityGroup.Equals("SecurityManager") || resultmodel.SecurityGroup.Equals("SecurityAdmin") || resultmodel.SecurityGroup.Equals("AlarmFilterCommissioner"))
                    {
                        resultmodel.OrgCode = info.Company;
                        if (regionmanager.GetUserAuthorityByName(context, info.UserName) != null)
                        {
                            resultmodel.ManagedRegionCode = regionmanager.GetUserAuthorityByName(context, info.UserName).RegionsCode;
                        }
                        if (info.Company != "0")
                        {
                            if (regionmanager.GetDistricByCode(context, info.Company) != null)
                            {
                                if (regionmanager.GetDistricByCode(context, info.Company).Code.Length == 2)
                                {
                                    resultmodel.Level = 1;
                                    resultmodel.CityCode = "";
                                    resultmodel.CityName = "";
                                    resultmodel.ProvinceCode = info.Company;
                                    resultmodel.ProvinceName = regionmanager.GetDistricByCode(context, info.Company).Name;
                                }
                                else
                                {
                                    resultmodel.Level = 2;
                                    resultmodel.CityCode = info.Company;
                                    resultmodel.CityName = regionmanager.GetDistricByCode(context, info.Company).Name;
                                    resultmodel.ProvinceCode = regionmanager.GetDistricByCode(context, info.Company).ParentCode;
                                    resultmodel.ProvinceName = regionmanager.GetDistricByCode(context, resultmodel.ProvinceCode).Name;
                                }
                            }
                        }

                    }
                    if (resultmodel.SecurityGroup.Equals("SiteManager") || resultmodel.SecurityGroup.Equals("SiteMonitor") || resultmodel.SecurityGroup.Equals("SysPhoneReceiver") || resultmodel.SecurityGroup.Equals("SysUpgradedUser") || resultmodel.SecurityGroup.Equals("SysMaintain"))
                    {
                        resultmodel.Level = -1;
                        resultmodel.OrgCode = info.Company;
                        if (info.Company != "0")
                        {
                            if (SetupManager.GetInstallStationByID(info.Company) != null)
                            {
                                //resultmodel.OrgName = SetupManager.GetInstallStationByID(info.Company).Name;
                                //resultmodel.CityCode = SetupManager.GetInstallStationByID(info.Company).CityCode;
                                //resultmodel.CityName = SetupManager.GetInstallStationByID(info.Company).CityName;
                                //resultmodel.ProvinceCode = SetupManager.GetInstallStationByID(info.Company).ProvinceCode;
                                //resultmodel.ProvinceName = SetupManager.GetInstallStationByID(info.Company).ProvinceName;
                            }
                        }
                    }
                    if (resultmodel.SecurityGroup.Equals("CompanyMonitor"))
                    {
                        resultmodel.Level = -1;
                        resultmodel.OrgCode = info.Company;
                        resultmodel.ManagedRegionCode = info.Company;
                    }
                }
                return resultmodel;
            }


        }
        public MultiMessage<ADAccountInfo> GetAccountInfoByGrouplist(List<string> list)
        {
            try
            {
                Info("GetAccountInfoByGrouplist");
                Info("list:" + Convert.ToString(list));
                List<ADAccountInfo> infolist = new List<ADAccountInfo>();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        infolist.AddRange(GetAccountInfoByGroupName(item).Result);
                    }
                }
                infolist = new List<ADAccountInfo>(infolist.OrderBy(o => o.UserLoginName));
                MultiMessage<ADAccountInfo> result = new MultiMessage<ADAccountInfo> { Result = infolist };
                Log<ADAccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ADAccountInfo>() { Result = null, ExceptionMessage = ex };
            }
        }
        public MultiMessage<ADAccountInfo> GetAccountInfoByGroupAndUserLoginName(string strGroupName, string strLoginName, string strUserName)
        {
            try
            {
                Info("GetAccountInfoByGroupAndUserLoginName");
                Info("strGroupName:" + Convert.ToString(strGroupName) + ";" + "strLoginName:" + Convert.ToString(strLoginName) + ";" + "strUserName:" + Convert.ToString(strUserName));
                Expression<Func<ADAccountInfo, bool>> expression = PredicateExtensions.True<ADAccountInfo>();

                if (!string.IsNullOrEmpty(strLoginName))
                {
                    expression = expression.And(c => !string.IsNullOrEmpty(c.UserLoginName) && c.UserLoginName.ToLower().Contains(strLoginName.Trim().ToLower()));
                }
                if (!string.IsNullOrEmpty(strUserName))
                {
                    //
                    //Fixed the third bug No. 14 By XiangboLiu 2015/06/11
                    //
                    expression = expression.And(c => !string.IsNullOrEmpty(c.UserName) && c.UserName.ToLower().Contains(strUserName.Trim().ToLower()));
                }

                List<ADAccountInfo> infolist = new List<ADAccountInfo>();
                infolist.AddRange(GetAccountInfoByGroupName(strGroupName).Result);

                IQueryable<ADAccountInfo> temp = infolist.AsQueryable().Where(expression);
                MultiMessage<ADAccountInfo> result = new MultiMessage<ADAccountInfo> { Result = temp.ToList(), IsSuccess = true };
                Log<ADAccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<ADAccountInfo>() { Result = null, ExceptionMessage = ex };
            }
        }
    }

    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;

        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression1, expression2.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body, invokedExpression), expression1.Parameters);
        }
    }
}

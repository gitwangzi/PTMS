using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Manager.Contract;
using Gsafety.PTMS.Manager.Contract.Data;
using System.Configuration;
using Gsafety.Common.AD;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.Manager.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ADUserInfoService : BaseService, IADAccountService
    {
        ADUserMange usermanager = null;
        public ADUserInfoService()
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
        /// <param name="model"></param>
        /// <returns></returns>
        public SingleMessage<bool> AddAccount(ADAccountInfo model)
        {
            bool isSuc = true;
            try
            {
                Info("AddAccount");
                Info("model:" + Convert.ToString(model));
                ADAccountEntity userentity = new ADAccountEntity();
                userentity.UserPrincipalName = model.UserName;
                userentity.UserName = model.UserName;
                userentity.UserPassword = model.UserPassword;
                userentity.Company = model.Company;
                userentity.Phone = model.Phone;
                userentity.Email = model.Email;
                userentity.Address = model.Address;
                userentity.Fax = model.Fax;
                userentity.SecurityGroup = model.SecurityGroup;
                userentity.DisplayName = model.DisplayName;
                userentity.Description = model.Description;
                isSuc = usermanager.AddUserAccountByModel(userentity);
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
        /// Update User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public SingleMessage<bool> UpdateAccount(ADAccountInfo user)
        {
            Info("UpdateAccount");
            Info("user:" + user.ToString());
            bool isSuc = true;
            try
            {
                Info("user:" + Convert.ToString(user));
                ADAccountEntity userentity = new ADAccountEntity();
                userentity.UserPrincipalName = user.UserLoginName;
                userentity.UserName = user.UserName;
                userentity.UserPassword = user.UserPassword;
                userentity.Phone = user.Phone;
                userentity.Fax = user.Fax;
                userentity.Company = user.Company;
                userentity.SecurityGroup = user.SecurityGroup;
                userentity.DisplayName = user.DisplayName;
                userentity.Description = user.Description;
                userentity.Email = user.Email;
                userentity.Address = user.Address;
                isSuc = usermanager.ChangeUserInfo(userentity);
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
        /// Delete User
        /// </summary>
        /// <param name="userAccountname"></param>
        /// <returns></returns>
        public SingleMessage<bool> DeleteAccount(string userAccountname)
        {

            try
            {
                Info("DeleteAccount");
                Info("userAccountname:" + Convert.ToString(userAccountname));
                var isSuc = usermanager.DeleteAccount(userAccountname);
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
        /// Get User Info
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SingleMessage<ADAccountInfo> GetAccount(String account)
        {
            ADAccountInfo accountinfo = new ADAccountInfo();
            ADAccountEntity entity = new ADAccountEntity();
            try
            {
                Info("GetAccount");
                Info("account:" + Convert.ToString(account));
                account = string.Format("CN={0},OU=National,DC=anttest,DC=com", account);
                entity = usermanager.GetUserModel(account);
                ADAccountInfo info = GetRepackUserinfo(entity);
                SingleMessage<ADAccountInfo> result = new SingleMessage<ADAccountInfo> { Result = info };
                Log<ADAccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<ADAccountInfo> { Result = null, ExceptionMessage = ex };
            }
        }
        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public SingleMessage<ADAccountInfo> ValidateUser(string account, string password)
        {
            ADAccountInfo accountinfo = null;
            ADAccountEntity entity = new ADAccountEntity();
            try
            {
                Info("ValidateUser");
                Info("account:" + Convert.ToString(account) + ";" + "password:" + Convert.ToString(password));
                entity = usermanager.ValidateUser(account, password);
                if (entity != null)
                {
                    accountinfo = GetRepackUserinfo(entity);
                }
                SingleMessage<ADAccountInfo> result = new SingleMessage<ADAccountInfo> { Result = accountinfo };
                Log<ADAccountInfo>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<ADAccountInfo> { Result = null, ExceptionMessage = ex };
            }
        }
        /// <summary>
        /// Get ADAccountInfo
        /// </summary>
        /// <remarks></remarks>
        /// <param name="entity"></param>
        private ADAccountInfo GetADAccountInfo(ADAccountEntity entity)
        {
            ADAccountInfo accountinfo = new ADAccountInfo();
            accountinfo.UserLoginName = entity.UserName; ;
            accountinfo.UserName = entity.DisplayName;
            accountinfo.SecurityGroup = entity.SecurityGroup;
            accountinfo.Description = entity.Description;
            accountinfo.Company = entity.Description;
            return accountinfo;
        }
        /// <summary>
        /// Verity that the User
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SingleMessage<bool> IsUserExits(string account)
        {
            try
            {
                Info("IsUserExits");
                Info("account:" + Convert.ToString(account));
                var isSuc = usermanager.IsUserExits(account);
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
        /// Active User
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SingleMessage<bool> EnableAccount(String account)
        {
            try
            {
                Info("EnableAccount");
                Info("account:" + Convert.ToString(account));
                var isSuc = usermanager.EnableUserAccount(account);
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
        /// Disable User
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SingleMessage<bool> DisableAccount(String account)
        {

            try
            {
                Info("DisableAccount");
                Info("account:" + Convert.ToString(account));
                var isSuc = usermanager.DisableAccount(account);
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
        /// Reset PassWord
        /// </summary>
        /// <param name="accountName">User</param>
        /// <param name="password">Pwd</param>
        /// <returns></returns>
        public SingleMessage<bool> ResetPassword(string accountName, string password)
        {
            try
            {
                Info("ResetPassword");
                Info("accountName:" + Convert.ToString(accountName) + ";" + "password:" + Convert.ToString(password));
                var isSuc = usermanager.ResetPassword(accountName, password);
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
        /// Get Repack Userinfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
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
                        //the company stands for the the district code
                        resultmodel.OrgCode = info.Company;
                        if (regionmanager.GetUserAuthorityByName(context, info.UserName) != null)
                        {
                            resultmodel.ManagedRegionCode = regionmanager.GetUserAuthorityByName(context, info.UserName).RegionsCode;
                        }
                        else
                        {
                            return null;
                        }
                        if (info.Company != "0")
                        {
                            //not the country user
                            District district = regionmanager.GetDistricByCode(context, info.Company);
                            if (district != null)
                            {
                                if (district.Code.Length == 2)
                                {
                                    resultmodel.Level = 1;
                                    resultmodel.CityCode = "";
                                    resultmodel.CityName = "";
                                    resultmodel.ProvinceCode = info.Company;
                                    resultmodel.ProvinceName = district.Name;
                                }
                                else
                                {
                                    resultmodel.Level = 2;
                                    resultmodel.CityCode = info.Company;
                                    resultmodel.CityName = district.Name;
                                    resultmodel.ProvinceCode = district.ParentCode;
                                    resultmodel.ProvinceName = regionmanager.GetDistricByCode(context, resultmodel.ProvinceCode).Name;
                                }
                            }
                        }

                    }
                    else if (resultmodel.SecurityGroup.Equals("SiteManager") || resultmodel.SecurityGroup.Equals("SiteMonitor") || resultmodel.SecurityGroup.Equals("SysPhoneReceiver") || resultmodel.SecurityGroup.Equals("SysUpgradedUser") || resultmodel.SecurityGroup.Equals("SysMaintain"))
                    {
                        //the setup user get the installation station from the company
                        resultmodel.Level = -1;
                        resultmodel.OrgCode = info.Company;
                        if (info.Company != "0")
                        {
                            InstallStation installstation = SetupManager.GetInstallStationByID(info.Company);
                            if (installstation != null)
                            {
                                resultmodel.OrgName = installstation.Name;
                                resultmodel.CityCode = installstation.CityCode;
                                resultmodel.CityName = installstation.CityName;
                                resultmodel.ProvinceCode = installstation.ProvinceCode;
                                resultmodel.ProvinceName = installstation.ProvinceName;
                            }
                        }

                    }
                    else if (resultmodel.SecurityGroup.Equals("CompanyMonitor"))
                    {
                        resultmodel.Level = -1;
                        resultmodel.OrgCode = info.Company;
                        resultmodel.ManagedRegionCode = info.Company;
                    }
                    else
                    {
                        return null;
                    }
                }
                return resultmodel;
            }

        }
    }
}

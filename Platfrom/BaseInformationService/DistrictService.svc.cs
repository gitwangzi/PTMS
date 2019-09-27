using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.DBEntity;

namespace Gsafety.PTMS.BaseInformation.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class DistrictService : Gsafety.PTMS.BaseInfo.BaseService, IDistrictService
    {
        private DistrictRepository Repository = new DistrictRepository();

        /// <summary>
        /// Get Province And City
        /// </summary>
        /// <returns></returns>
        public MultiMessage<District> GetProvinceAndCity()
        {
            try
            {
                Info("GetProvinceAndCity");
                var temp = new List<District>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetProvinceAndCity(context);
                }

                MultiMessage<District> result = new MultiMessage<District>() { IsSuccess = true, Result = temp };
                //Log<District>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<District>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Get All District
        /// </summary>
        /// <returns></returns>
        public MultiMessage<District> GetDistrict()
        {
            try
            {
                Info("GetDistrict");
                GetUserInfo();
                var temp = new List<District>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetDistrict(context);
                }

                MultiMessage<District> result = new MultiMessage<District>() { IsSuccess = true, Result = temp };
                //Log<District>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<District>() { ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// Get District By Authority
        /// </summary>
        /// <returns></returns>
        public MultiMessage<District> GetDistrictByAuthority()
        {
            try
            {
                Info("GetDistrictByAuthority");
                UserInfoMessageHeader userInfo = GetUserInfo();
                var temp = new List<District>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetDistrictByAuthority(context, userInfo);
                }


                MultiMessage<District> result = new MultiMessage<District>() { IsSuccess = true, Result = temp };
                Log<District>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<District>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<UserAuthority> GetUserAuthorityFuzzy(string loginName, PagingInfo page)
        {
            try
            {
                Info("GetUserAuthorityFuzzy");
                Info("loginName:" + Convert.ToString(loginName) + ";" + "page:" + Convert.ToString(page));
                int totalRecord;
                var temp = new List<UserAuthority>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetUserAuthorityFuzzy(context, loginName, page, out totalRecord);
                }

                MultiMessage<UserAuthority> result = new MultiMessage<UserAuthority>() { Result = temp, TotalRecord = totalRecord };
                Log<UserAuthority>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<UserAuthority>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> AddUserAuthority(UserAuthority userAuthority)
        {
            try
            {
                Info("AddUserAuthority");
                Info(userAuthority.ToString());
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.AddUserAuthority(context, userAuthority);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> DeleteUserAuthority(string loginName)
        {
            try
            {
                Info("DeleteUserAuthority");
                Info("loginName:" + Convert.ToString(loginName));
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                  temp  = Repository.DeleteUserAuthority(context, loginName);
                }


                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> UpdateUserAuthority(UserAuthority userAuthority)
        {
            try
            {
                Info("UpdateUserAuthority");
                Info(userAuthority.ToString());
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateUserAuthority(context, userAuthority);
                }


                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }

        public MultiMessage<FuncItem> GetFuncItem()
        {
            try
            {
                Info("GetFuncItem");
                var temp = new List<FuncItem>();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetFuncItem(context);
                }

                MultiMessage<FuncItem> result = new MultiMessage<FuncItem>() { IsSuccess = true, Result = temp };
                Log<FuncItem>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<FuncItem> { ExceptionMessage = ex };
            }
        }

        public SingleMessage<RoleFuncs> GetFuncByRole(string roleName)
        {
            try
            {
                Info("GetFuncByRole");
                Info("roleName:" + Convert.ToString(roleName));
                var temp = new RoleFuncs();
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.GetFuncByRole(context, roleName);
                }

                SingleMessage<RoleFuncs> result = new SingleMessage<RoleFuncs>() { IsSuccess = true, Result = temp };
                Log<RoleFuncs>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<RoleFuncs> { ExceptionMessage = ex };
            }
        }

        public SingleMessage<bool> UpdateRoleFunc(RoleFuncs roleFunc)
        {
            try
            {
                Info("UpdateRoleFunc");
                Info(roleFunc.ToString());
                var temp = false;
                using (PTMSEntities context = new PTMSEntities())
                {
                    temp = Repository.UpdateRoleFunc(context, roleFunc);
                }

                SingleMessage<bool> result = new SingleMessage<bool>() { IsSuccess = true, Result = temp };
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>() { ExceptionMessage = ex };
            }
        }
    }
}

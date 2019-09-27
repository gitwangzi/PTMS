using Gsafety.Common.Util;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;
using System;
using System.Transactions;

namespace Gs.PTMS.Service
{
    public class OrganizationService : BaseService, IOrganization
    {

        /// <summary>
        /// 添加组织机构表
        /// </summary>
        /// <param name="model">组织机构表</param>
        public SingleMessage<bool> InsertOrganization(Organization model, string userID)
        {
            Info("InsertOrganization");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = new SingleMessage<bool>();
                model.CreateTime = DateTime.UtcNow;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        model.CreateTime = ConvertHelper.DateTimeNow();
                        result.Result = OrganizationRepository.InsertOrganization(context, model, userID);
                        result.IsSuccess = true;
                        if (result.Result)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// 修改组织机构表
        /// </summary>
        public SingleMessage<bool> UpdateOrganization(Organization model)
        {
            Info("UpdateOrganization");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = new SingleMessage<bool>();
                using (var context = new PTMSEntities())
                {
                    result.Result = OrganizationRepository.UpdateOrganization(context, model);
                    result.IsSuccess = true;
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// 删除组织机构表
        /// </summary>
        public SingleMessage<bool> DeleteOrganization(string Id, string userid)
        {
            Info("DeleteOrganization");
            Info(Id);
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    TransactionOptions optons = new TransactionOptions();
                    optons.IsolationLevel = IsolationLevel.ReadCommitted;
                    var scope = new TransactionScope(TransactionScopeOption.Required, optons);
                    try
                    {
                        result = OrganizationRepository.DeleteOrganization(context, Id, userid); 
                        
                        if (result.Result)
                        {
                            scope.Complete();
                        }
                    }
                    finally
                    {
                        scope.Dispose();
                    }
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        public MultiMessage<Organization> GetOrganizationByUser(string userid)
        {
            Info("GetOrganizationByUser");
            try
            {
                MultiMessage<Organization> result = new MultiMessage<Organization>();
                using (var context = new PTMSEntities())
                {
                    result.Result = OrganizationRepository.GetOrganizationByUser(context, userid);
                    result.IsSuccess = true;
                }
                Log<Organization>(result);

                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Organization> { IsSuccess = false, ExceptionMessage = ex };
            }
        }

        /// <summary>
        /// 是否可以删除组织机构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MultiMessage<Organization> GetAllOrganization(string clientID)
        {
            Info("GetAllOrganization");
            Info(clientID);
            try
            {
                MultiMessage<Organization> result = new MultiMessage<Organization>();
                using (var context = new PTMSEntities())
                {
                    result.Result = OrganizationRepository.GetAllOrganization(context, clientID);
                    result.IsSuccess = true;
                }
                Log<Organization>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<Organization> { IsSuccess = false, ExceptionMessage = ex };
            }
        }
    }
}

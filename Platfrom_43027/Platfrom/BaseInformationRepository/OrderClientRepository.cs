using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Gsafety.PTMS.Manager.Repository;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.Manager.Contract;
using System.Data.Entity.Validation;
using Gsafety.PTMS.Manager.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Repository.UserManage.Utilities;

namespace Gsafety.PTMS.BaseInformation.Repository
{
    ///<summary>
    ///客户
    ///</summary>
    public class OrderClientRepository
    {
        internal const string _orderClientRoleName = "OrderClient";
        internal const string _superAdminRoleName = "SuperAdmin";
        internal const string _orderClientDefaultPwd = "96e79218965eb72c92a549dd5a330112";

        //internal const string _orderClientDefaultPwd = "q123456.";
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="model">客户</param>
        public static SingleMessage<bool> InsertOrderClient(PTMSEntities context, OrderClientEx model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            var role = context.USR_ROLE.FirstOrDefault(t => t.ROLE_CATEGORY == (short)RoleCategory.ClientAdmin);
            if (role == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientRoleNameNoExist);
            }

            var orderClient = new BSC_ORDER_CLIENT();
            OrderClientUtility.UpdateEntity(orderClient, model, true);
            context.BSC_ORDER_CLIENT.Add(orderClient);

            var gUser = new GUser()
            {
                ID = Guid.NewGuid().ToString(),
                Account = model.UserName,
                UserName = model.Contact,
                Password = _orderClientDefaultPwd,
                Phone = model.Phone,
                Mobile = model.Mobile,
                Email = model.Email,
                Address = model.Address,
                RoleID = role.ID,
                Creator = _superAdminRoleName,
                IsClientCreate = false,
                Department = string.Empty,
                ClientID = orderClient.ID,
                Description = string.Empty
            };
            var result = GUserRepository.Insert(context, gUser);

            if (result.IsSuccess == false || result.Result == false)
            {
                return result;
            }
            return context.Save();
        }

        /// <summary>
        /// 修改客户
        /// </summary>
        public static SingleMessage<bool> UpdateOrderClient(PTMSEntities context, OrderClientEx model)
        {
            if (string.IsNullOrWhiteSpace(model.ID))
            {
                throw new ArgumentNullException("model.ID");
            }

            var entity = context.BSC_ORDER_CLIENT.FirstOrDefault(t => t.ID == model.ID);
            if (entity == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientNotExist);
            }

            OrderClientUtility.UpdateEntity(entity, model, false);

            var gUserEntity = GetGUSERByOrderClientID(context, model.ID);
            if (gUserEntity == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientNotExist);
            }

            var gUserModel = GUserUtility.GetModel(gUserEntity);
            gUserModel.UserName = model.Contact;
            gUserModel.Phone = model.Phone;
            gUserModel.Mobile = model.Mobile;
            gUserModel.Email = model.Email;
            gUserModel.Address = model.Address;

            var result = GUserRepository.Update(context, gUserModel);

            if (!result.Result)
            {
                return result;
            }

            context.BSC_ORDER_CLIENT.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return context.Save();
        }

        public static MultiMessage<OrderClientEx> GetOrderClientExList(PTMSEntities context, OrderClientManagerQueryModel obj)
        {
            int totalCount;

            List<BSC_ORDER_CLIENT> orderList = null;
            if (string.IsNullOrEmpty(obj.Name))
            {
                orderList = context.BSC_ORDER_CLIENT.
                    Where(x => ((obj.Status.HasValue) ? x.STATUS == (short)obj.Status.Value : true))
                   .Page(out totalCount, obj.PageIndex, obj.PageSize, true, n => n.CREATE_TIME, false).ToList();
            }
            else
            {
                orderList = context.BSC_ORDER_CLIENT.
                   Where(x => ((obj.Status.HasValue) ? x.STATUS == (short)obj.Status.Value : true) && x.NAME.Contains(obj.Name)).OrderByDescending(n => n.CREATE_TIME)
                   .Page(out totalCount, obj.PageIndex, obj.PageSize, true, n => n.CREATE_TIME, false).ToList();
            }

            var orderIDs = orderList.Select(t => t.ID).ToList();

            var deviceCountDic = context.BSC_DEV_SUITE.Where(n => orderIDs.Contains(n.CLIENT_ID)).GroupBy(o => o.CLIENT_ID).ToDictionary(t => t.Key, t => t.Count());
            var userCountDic = context.USR_GUSER.Where(n => orderIDs.Contains(n.CLIENT_ID) && n.IS_CLIENT_CREATE == 1).GroupBy(o => o.CLIENT_ID).ToDictionary(t => t.Key, t => t.Count());
            var role = context.USR_ROLE.FirstOrDefault(t => t.ROLE_CATEGORY == (short)RoleCategory.ClientAdmin);

            var users = context.USR_GUSER.Where(n => n.ROLE_ID == role.ID).ToDictionary(t => t.CLIENT_ID, t => t.ACCOUNT);

            var list = orderList.Select(t => new OrderClientEx()
               {
                   ID = t.ID,
                   Name = t.NAME,
                   Address = t.ADDRESS,
                   DeviceCount = (int)t.DEVICE_COUNT,
                   UserCount = (int)t.USER_COUNT,
                   Status = (StatusEnum)t.STATUS,
                   TansferMode = (TansferModeEnum)t.TANSFER_MODE,
                   ActualUserCount = userCountDic.ContainsKey(t.ID) ? userCountDic[t.ID] : 0,
                   ActualDeviceCount = deviceCountDic.ContainsKey(t.ID) ? deviceCountDic[t.ID] : 0,
                   Mobile = t.MOBILE,
                   Phone = t.PHONE,
                   BeginTime = t.BEGIN_TIME,
                   EndTime = t.END_TIME,
                   Contact = t.CONTACT,
                   Email = t.EMAIL,
                   PlatformVersion = t.VERSION,
                   UserName = users.ContainsKey(t.ID) ? users[t.ID] : string.Empty
               }).ToList();

            return new MultiMessage<OrderClientEx>(list, totalCount);
        }

        public static SingleMessage<bool> ResetPassword(PTMSEntities context, string orderClientID, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(orderClientID))
            {
                throw new ArgumentNullException("orderClientID");
            }

            var gUSEREntity = GetGUSERByOrderClientID(context, orderClientID);
            if (gUSEREntity == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientNotExist);
            }

            return GUserRepository.ModifyPassword(context, gUSEREntity.ID, newPassword);
        }

        private static USR_GUSER GetGUSERByOrderClientID(PTMSEntities context, string orderClientID)
        {
            var gUserEntity = (from u in context.USR_GUSER
                               join r in context.USR_ROLE on u.ROLE_ID equals r.ID
                               where u.CLIENT_ID == orderClientID && r.ROLE_CATEGORY == (int)RoleCategory.ClientAdmin
                               select u).FirstOrDefault();
            return gUserEntity;
        }

        public static SingleMessage<bool> SetOrderClientStatus(PTMSEntities context, string orderClientID, bool enable)
        {
            if (string.IsNullOrWhiteSpace(orderClientID))
            {
                throw new ArgumentNullException("orderClientID");
            }

            var orderClient = context.BSC_ORDER_CLIENT.FirstOrDefault(t => t.ID == orderClientID);

            if (orderClient == null)
            {
                return new SingleMessage<bool>(false, CommonErrorMessage.OrderClientNotExist);
            }

            orderClient.STATUS = (short)(enable ? 0 : 1);
            context.Entry(orderClient).State = EntityState.Modified;

            return context.Save();
        }

        public static SingleMessage<OrderClient> GetOrderClient(PTMSEntities context, string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                throw new ArgumentNullException("ID");
            }
            BSC_ORDER_CLIENT entity = context.BSC_ORDER_CLIENT.SingleOrDefault(n => n.ID == ID);
            if (entity != null)
            {
                OrderClient model = OrderClientUtility.GetModel(entity);
                return new SingleMessage<OrderClient>(model);
            }
            else
            {
                return new SingleMessage<OrderClient>(false, CommonErrorMessage.OrderClientNotExist);
            }
        }
    }
}


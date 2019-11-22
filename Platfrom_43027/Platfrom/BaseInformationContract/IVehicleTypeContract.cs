using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;
using Gsafety.PTMS.Common.Data;
namespace Gsafety.PTMS.BaseInformation.Contract.Data
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface IVehicleType
    {
        [OperationContract]
        SingleMessage<bool> InsertVehicleTypeColor(List<VehicleTypeColor> color);

        [OperationContract]
        SingleMessage<bool> UpdateVehicleTypeColor(VehicleTypeColor model);

        [OperationContract]
        SingleMessage<bool> DeleteVehicleTypeColor(string id);

        [OperationContract]
        MultiMessage<VehicleTypeColor> GetVehicleTypeColorList(string typeid);

        [OperationContract]
        MultiMessage<VehicleTypeColor> GetAllVehicleTypeColorList(string clientID);


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> InsertVehicleType(VehicleType model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> UpdateVehicleType(VehicleType model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        [OperationContract]
        SingleMessage<bool> DeleteVehicleTypeByID(string ID);

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <returns>获取</returns>
        //[OperationContract]
        //SingleMessage<VehicleType> GetVehicleType(string ID);

        /// <summary>
        /// 按名称查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<VehicleType> GetByNameVehicleTypeList(int pageIndex, int pageSize, string clientID, string name);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="SearchVehicleId"></param>
        /// <param name="SearchOwner"></param>
        /// <param name="SearchVehicleType"></param>
        /// <returns></returns>
        [OperationContract]
        MultiMessage<Vehicle> GetBscVehicleList(int pageIndex, int pageSize, string SearchVehicleId, string SearchOwner, string SearchVehicleType, string orgId, string vehicletypeid);

        [OperationContract]
        MultiMessage<VehicleType> GetVehicleTypeList(string clientID);
    }
}


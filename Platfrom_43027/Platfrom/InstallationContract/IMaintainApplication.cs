using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Installation.Contract
{
	///<summary>
	///维修申请
	///</summary>
	[ServiceContract]
	public interface IMaintainApplication
	{

		/// <summary>
		/// 添加维修申请
		/// </summary>
		/// <param name="model">维修申请</param>
		[OperationContract]
		SingleMessage<bool> InsertMaintainApplication(MaintainApplication model);

		/// <summary>
		/// 修改维修申请
		/// </summary>
		/// <param name="model">维修申请</param>
		[OperationContract]
		SingleMessage<bool> UpdateMaintainApplication(MaintainApplication model);

		/// <summary>
		/// 删除维修申请
		/// </summary>
		/// <param name="model">维修申请</param>
		[OperationContract]
		SingleMessage<bool> DeleteMaintainApplicationByID(string ID);

		/// <summary>
		/// 获取维修申请
		/// </summary>
		/// <returns>获取维修申请</returns>
		[OperationContract]
        SingleMessage<MaintainApplication> GetMaintainApplication(string clientID, string ID);
		/// <summary>
		/// 获取维修申请列表
		/// </summary>
		/// <returns>获取维修申请</returns>
		[OperationContract]
		MultiMessage<MaintainApplication> GetMaintainApplicationList(string clientID,int pageIndex, int pageSize);

        /// <summary>
        /// 获取维修申请列表
        /// </summary>
        /// <returns>获取维修申请</returns>
        [OperationContract]
        MultiMessage<MaintainApplication> GetMaintainApplicationByCondition(string clientID,string name,int pageIndex, int pageSize);


	}
}


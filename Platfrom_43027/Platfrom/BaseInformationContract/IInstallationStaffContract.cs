using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Models;

namespace Gsafety.PTMS.BaseInformation.Contract
{
	///<summary>
	///
	///</summary>
	[ServiceContract]
	public interface IInstallationStaff
	{

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="model"></param>
		[OperationContract]
		SingleMessage<bool> InsertInstallationStaff(InstallationStaff model);

		/// <summary>
		/// 修改
		/// </summary>
		/// <param name="model"></param>
		[OperationContract]
		SingleMessage<bool> UpdateInstallationStaff(InstallationStaff model);

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="model"></param>
		[OperationContract]
		SingleMessage<bool> DeleteInstallationStaffByID(string ID);

		/// <summary>
		/// 获取
		/// </summary>
		/// <returns>获取</returns>
		[OperationContract]
		SingleMessage<InstallationStaff> GetInstallationStaff(string ID);
		/// <summary>
		/// 获取列表
		/// </summary>
		/// <returns>获取</returns>
		[OperationContract]
		MultiMessage<InstallationStaff> GetInstallationStaffList(int pageIndex, int pageSize);

	}
}


using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
namespace Gsafety.PTMS.BaseInformation.Contract
{
	///<summary>
	///行政区域
	///</summary>
	[ServiceContract]
	public interface IBscDistrict
	{

		/// <summary>
		/// 添加行政区域
		/// </summary>
		/// <param name="model">行政区域</param>
		[OperationContract]
		SingleMessage<bool> InsertBscDistrict(BscDistrict model);

		/// <summary>
		/// 修改行政区域
		/// </summary>
		/// <param name="model">行政区域</param>
		[OperationContract]
		SingleMessage<bool> UpdateBscDistrict(BscDistrict model);

		/// <summary>
		/// 删除行政区域
		/// </summary>
		/// <param name="model">行政区域</param>
		[OperationContract]
		SingleMessage<bool> DeleteBscDistrictByID(string Code);

		/// <summary>
		/// 获取行政区域
		/// </summary>
		/// <returns>获取行政区域</returns>
		[OperationContract]
		SingleMessage<BscDistrict> GetBscDistrict(string Code);
		/// <summary>
		/// 获取行政区域列表
		/// </summary>
		/// <returns>获取行政区域</returns>
		[OperationContract]
		MultiMessage<BscDistrict> GetBscDistrictList(int pageIndex, int pageSize);

	}
}


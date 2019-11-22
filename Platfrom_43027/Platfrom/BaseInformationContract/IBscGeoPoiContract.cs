using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
namespace Gsafety.PTMS.BaseInformation.Contract
{
	///<summary>
	///POI
	///</summary>
	[ServiceContract]
	public interface IBscGeoPoiService
	{

		/// <summary>
		/// 添加POI
		/// </summary>
		/// <param name="model">POI</param>
		[OperationContract]
		SingleMessage<bool> InsertBscGeoPoi(BscGeoPoi model);

		/// <summary>
		/// 修改POI
		/// </summary>
		/// <param name="model">POI</param>
		[OperationContract]
		SingleMessage<bool> UpdateBscGeoPoi(BscGeoPoi model);

		/// <summary>
		/// 删除POI
		/// </summary>
		/// <param name="model">POI</param>
		[OperationContract]
		SingleMessage<bool> DeleteBscGeoPoiByID(decimal ID);

		/// <summary>
		/// 获取POI
		/// </summary>
		/// <returns>获取POI</returns>
		[OperationContract]
		SingleMessage<BscGeoPoi> GetBscGeoPoi(decimal ID);
		/// <summary>
		/// 获取POI列表
		/// </summary>
		/// <returns>获取POI</returns>
		[OperationContract]
        MultiMessage<BscGeoPoi> GetBscGeoPoiList(int pageIndex, int pageSize, string searchContent, decimal property);

	}
}


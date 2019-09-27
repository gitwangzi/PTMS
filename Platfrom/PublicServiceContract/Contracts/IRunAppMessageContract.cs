using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ComponentModel.DataAnnotations;
using Gsafety.PTMS.Base.Contract.Data;
namespace Gsafety.PTMS.PublicService.Contract
{
	///<summary>
	///App消息
	///</summary>
	[ServiceContract]
	public interface IRunAppMessage
	{

		/// <summary>
		/// 添加App消息
		/// </summary>
		/// <param name="model">App消息</param>
		[OperationContract]
		SingleMessage<bool> InsertRunAppMessage(RunAppMessage model);

		/// <summary>
		/// 修改App消息
		/// </summary>
		/// <param name="model">App消息</param>
		[OperationContract]
		SingleMessage<bool> UpdateRunAppMessage(RunAppMessage model);

		/// <summary>
		/// 删除App消息
		/// </summary>
		/// <param name="model">App消息</param>
		[OperationContract]
		SingleMessage<bool> DeleteRunAppMessageByID(string ID);

		/// <summary>
		/// 获取App消息
		/// </summary>
		/// <returns>获取App消息</returns>
		[OperationContract]
		SingleMessage<RunAppMessage> GetRunAppMessage(string ID);
		/// <summary>
		/// 获取App消息列表
		/// </summary>
		/// <returns>获取App消息</returns>
		[OperationContract]
		MultiMessage<RunAppMessage> GetRunAppMessageList(string clientID,string title,int pageIndex, int pageSize);

        /// <summary>
        /// 发送App消息
        /// </summary>
        /// <param name="model">App消息</param>
        [OperationContract]
        SingleMessage<bool> SendRunAppMessage(RunAppMessage model,string vehicleId);
	}
}


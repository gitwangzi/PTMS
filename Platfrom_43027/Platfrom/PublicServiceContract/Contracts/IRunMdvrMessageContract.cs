using Gsafety.PTMS.Base.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Gsafety.PTMS.Common.Data;

namespace Gsafety.PTMS.PublicService.Contract
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IMessageManageService”。
    [ServiceContract]
    public interface IRunMdvrMessage
    {
        /// <summary>
        /// 添加LED消息
        /// </summary>
        /// <param name="model">LED消息</param>
        [OperationContract]
        SingleMessage<bool> InsertRunMdvrMessage(RunMdvrMessage model);

        /// <summary>
        /// 修改LED消息
        /// </summary>
        /// <param name="model">LED消息</param>
        [OperationContract]
        SingleMessage<bool> UpdateRunMdvrMessage(RunMdvrMessage model);

        /// <summary>
        /// 删除LED消息
        /// </summary>
        /// <param name="model">LED消息</param>
        [OperationContract]
        SingleMessage<bool> DeleteRunMdvrMessageByID(string ID);

        /// <summary>
        /// 获取LED消息
        /// </summary>
        /// <returns>获取LED消息</returns>
        [OperationContract]
        SingleMessage<RunMdvrMessage> GetRunMdvrMessage(string ID);
        /// <summary>
        /// 获取LED消息列表
        /// </summary>
        /// <returns>获取LED消息</returns>
        [OperationContract]
        MultiMessage<RunMdvrMessage> GetRunMdvrMessageList(string clientID, string title, int type, string name, int pageIndex, int pageSize);
    }
}

using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///
    ///</summary>
    [ServiceContract]
    public interface IUserOnline
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>获取</returns>
        [OperationContract]
        MultiMessage<UserOnline> GetUserOnlineList(string clientID, string userName, int pageIndex, int pageSize);

    }
}


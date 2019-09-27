using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Manager.Contract.Data.CommandManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Gsafety.PTMS.Manager.Contract
{
    ///<summary>
    ///视频设置
    ///</summary>
    [ServiceContract]
    public interface IVideoRule
    {
        /// <summary>
        /// 添加视频设置
        /// </summary>
        /// <param name="model">视频设置</param>
        [OperationContract]
        SingleMessage<bool> InsertVideoRule(VideoRule model);

        /// <summary>
        /// 修改视频设置
        /// </summary>
        /// <param name="model">视频设置</param>
        [OperationContract]
        SingleMessage<bool> UpdateVideoRule(VideoRule model);

        /// <summary>
        /// 删除视频设置
        /// </summary>
        /// <param name="model">视频设置</param>
        [OperationContract]
        SingleMessage<bool> DeleteVideoRuleByID(string ID);


        [OperationContract]
        MultiMessage<VideoRule> GetVideoRuleListByName(PagingInfo page, string clientID, string name);

    }
}


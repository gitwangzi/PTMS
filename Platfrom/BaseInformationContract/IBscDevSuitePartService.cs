using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInformation.Contract
{
    [ServiceContract]
    public interface IBscDevSuitePartService
    {
        /// <summary>
        /// 添加安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> InsertBscDevSuitePart(DevSuitePart model);

        /// <summary>
        /// 修改安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> UpdateBscDevSuitePart(DevSuitePart model);

        /// <summary>
        /// 删除安全套件
        /// </summary>
        /// <param name="model">安全套件</param>
        [OperationContract]
        SingleMessage<bool> DeleteBscDevSuitePartByID(string SuiteInfoID);

        /// <summary>
        /// 查询安全套件摄像头列表
        /// </summary>
        /// <param name="model">安全套件InfoID</param>
        [OperationContract]
        MultiMessage<DevSuitePart> GetCameraListBySuiteInfoID(string suitInfoID);
    }
}

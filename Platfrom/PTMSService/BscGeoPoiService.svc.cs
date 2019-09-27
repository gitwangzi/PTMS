using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“BscGeoPoiService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 BscGeoPoiService.svc 或 BscGeoPoiService.svc.cs，然后开始调试。
    ///<summary>
    ///POI
    ///</summary>
    public class BscGeoPoiService : BaseService, IBscGeoPoiService
    {

        /// <summary>
        /// 添加POI
        /// </summary>
        /// <param name="model">POI</param>
        public SingleMessage<bool> InsertBscGeoPoi(BscGeoPoi model)
        {
            Info("InsertBscGeoPoi");
            Info(model.ToString());
            
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscGeoPoiRepository.InsertBscGeoPoi(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 修改POI
        /// </summary>
        public SingleMessage<bool> UpdateBscGeoPoi(BscGeoPoi model)
        {
            Info("UpdateBscGeoPoi");
            Info(model.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscGeoPoiRepository.UpdateBscGeoPoi(context, model);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 删除POI
        /// </summary>
        public SingleMessage<bool> DeleteBscGeoPoiByID(decimal ID)
        {
            Info("DeleteBscGeoPoiByID");
            Info(ID.ToString());
            try
            {
                SingleMessage<bool> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscGeoPoiRepository.DeleteBscGeoPoiByID(context, ID);
                }
                Log<bool>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<bool>(false, ex);
            }
        }

        /// <summary>
        /// 获取POI
        /// </summary>
        public SingleMessage<BscGeoPoi> GetBscGeoPoi(decimal ID)
        {
            Info("GetBscGeoPoi");
            Info(ID.ToString());
            try
            {
                SingleMessage<BscGeoPoi> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscGeoPoiRepository.GetBscGeoPoi(context, ID);
                }
                Log<BscGeoPoi>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new SingleMessage<BscGeoPoi>(false, ex);
            }
        }
        /// <summary>
        /// 获取POI
        /// </summary>
        public MultiMessage<BscGeoPoi> GetBscGeoPoiList(int pageIndex, int pageSize, string searchContent, decimal property)
        {
            Info("GetBscGeoPoiList");
            Info(pageIndex.ToString());
            Info(pageSize.ToString());
            try
            {
                MultiMessage<BscGeoPoi> result = null;
                using (var context = new PTMSEntities())
                {
                    result = BscGeoPoiRepository.GetBscGeoPoiList(context, pageIndex, pageSize, searchContent.ToLower(), property);
                }
                Log<BscGeoPoi>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<BscGeoPoi>(false, ex);
            }
        }

    }
}

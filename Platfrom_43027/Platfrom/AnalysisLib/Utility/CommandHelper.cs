using Gsafety.Common.Logging;
using Gsafety.Common.Util;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Message.Contract.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.AnalysisLib
{
    public class CommandHelper
    {
        public static ReturnInfo QueryParaResponse(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();
            string ruleKey = null;
            //byte[] -> json
            string str = System.Text.UnicodeEncoding.UTF8.GetString(bytes);
            LoggerManager.Logger.Info(string.Format("Get Query Param Response Info:{0}", str));
            //json -> entity
            QueryParaResponseEx model = JsonHelper.FromJsonString<QueryParaResponseEx>(str);

            if (model != null)
            {
                //fill model 
                model.MdvrCoreId = model.UID;
                if (FillQueryResponseModel(model, out ruleKey))
                {
                    returnInfo.RuleKey = ruleKey;
                    returnInfo.Message = ConvertHelper.ObjectToBytes(model);
                }
            }
            else
            {
                LoggerManager.Logger.Warn(string.Format("Converted QueryParaResponseEx to entity is null,string:{0}", str));
            }
            return returnInfo;
        }

        private static bool FillQueryResponseModel(QueryParaResponseEx model, out string ruleKey)
        {
            if (CacheDataManager.Suites.ContainsKey(model.MdvrCoreId))
            {
                var suiteInfo = CacheDataManager.Suites[model.MdvrCoreId];
                if (suiteInfo != null)
                {
                    model.VehicleId = suiteInfo.VEHICLE_ID;
                    model.SuiteInfoID = suiteInfo.SUITE_INFO_ID;
                    model.ClientId = suiteInfo.CLIENT_ID;
                    ruleKey = string.Format(".{0}.{1}.{2}", model.ClientId, suiteInfo.ORGNIZATION_ID, suiteInfo.VEHICLE_ID);
                    return true;
                }
                else
                {
                    LoggerManager.Logger.Warn(string.Format("Fill OnOffline data from cache when the entity is emply,MdvrCoreSN:{0}", model.UID));
                    ruleKey = null;
                    return false;
                }
            }
            ruleKey = null;
            return false;
        }

        public static ReturnInfo QueryPartParam(PTMSEntities context, byte[] bytes)
        {
            ReturnInfo returnInfo = new ReturnInfo();

            var model = ConvertHelper.BytesToObject(bytes) as QueryPartParamEx;

            QueryPartParam response = model as QueryPartParam;

            if (response != null)
            {
                //entity -> json
                string str = JsonHelper.ToJsonString(response);
                //json -> byte[]
                var msg = System.Text.UTF8Encoding.UTF8.GetBytes(str);

                if (msg != null && msg.Length > 0)
                {
                    returnInfo.Message = msg;
                }
            }
            else
            {
                //LoggerManager.Logger.Warn(string.Format("Converted register to entity is null,string:{0}", str));
            }
            return returnInfo;
        }
    }
}

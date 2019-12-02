using Gsafety.Common.Logging;
using Gsafety.PTMS.Base.Contract.Data;
using Gsafety.PTMS.BaseInfo;
using Gsafety.PTMS.BaseInformation.Contract;
using Gsafety.PTMS.BaseInformation.Contract.Data;
using Gsafety.PTMS.BaseInformation.Repository;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.DBEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Transactions;

namespace Gs.PTMS.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“BscGeoPoiService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 BscGeoPoiService.svc 或 BscGeoPoiService.svc.cs，然后开始调试。
    ///<summary>
    ///POI
    ///</summary>
    public class BscGeoPoiService : BaseService, IBscGeoPoiService
    {

        public string GetDataAsync(string url)
        {
            var result = string.Empty;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                var httpwebrequest = (HttpWebRequest)WebRequest.Create(url);
                httpwebrequest.ContentType = "application/json";
                httpwebrequest.Method = "Get";


                var httpResponse = (HttpWebResponse)httpwebrequest.GetResponse();
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return null;

            }
        }

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

        /// <summary>
        /// 获取POI
        /// </summary>
        public MultiMessage<BscGeoPoiArgGis> GetBscGeoPoiList_ArgGis(string searchContent)
        {
            Info("GetBscGeoPoiList_ArgGis");
            
            try
            {
                MultiMessage<BscGeoPoiArgGis> result = null;

                string GeoUrl = System.Configuration.ConfigurationManager.AppSettings["GisGeocodeServiceUrl"];

                if (GeoUrl != string.Empty)
                {

                    if (!GeoUrl.EndsWith("/"))
                    {
                        GeoUrl += "/";
                    }
                }

                string PoiList = GetDataAsync(GeoUrl + "findAddressCandidates?Address=" + searchContent + "&outFields=*&maxLocations=50&f=pjson");

                if (!string.IsNullOrEmpty(PoiList))
                {
                    List<BscGeoPoiArgGis> GisData = new List<BscGeoPoiArgGis>();
                    ArgGisRoot data = JsonConvert.DeserializeObject<ArgGisRoot>(PoiList);
                    foreach (var item in data.candidates)
                    {
                        BscGeoPoiArgGis single = new BscGeoPoiArgGis();
                        string StrAddress = item.attributes.Place_addr;
                        char[] cSplit = {';'};
                        List<string> ListAddress = StrAddress.Split(cSplit, StringSplitOptions.RemoveEmptyEntries).ToList();

                        single.Address = "";
                        if (ListAddress.Count > 0)
                        {
                            single.Address = ListAddress.Last();
                        }
                        single.Name = item.attributes.PlaceName;
                        single.Longitude = item.attributes.DisplayX;
                        single.Latitude = item.attributes.DisplayY;
                        single.Property = item.attributes.Type;
                        
                        GisData.Add(single);
                    }
                    result = new MultiMessage<BscGeoPoiArgGis>(GisData, GisData.Count());

                }

                Log<BscGeoPoiArgGis>(result);
                return result;
            }
            catch (Exception ex)
            {
                Error(ex);
                return new MultiMessage<BscGeoPoiArgGis>(false, ex);
            }
        }

    }
}

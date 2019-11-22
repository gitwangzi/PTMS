using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Gs.CitySafety.AccessInContracts;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Gsafety.Ant.Analysis.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HttpClient client = new HttpClient();
            PublicTrafficInfo pti = GetPTInfo();

            client.BaseAddress = new Uri("http://172.16.11.113:8700/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string httpUrl = "api/Alarm/PublicTrafficAlarm?Verify=";

            var response = client.PostAsJsonAsync(httpUrl, pti);
            HttpResponseMessage httpResponseMessage = response.Result.EnsureSuccessStatusCode();
            HttpContent httpContent = httpResponseMessage.Content;
            var result = httpContent.ReadAsAsync<Gs.CitySafety.AccessInContracts.Data.ReturnStatus>();

            string IncidentId = result.Result.IncidentAppealId;
            if (result.Result.status == 1)
            {
                //IncidentId = 
                //returnResult = true;
                //LoggerManager.Logger.Info("Send alarm message to ECU911 success！IncidentAppealId：" + result.IncidentAppealId);
            }
            else
            {
                //////转发失败
                //disposeItem.ForwardedFlag = 2;
                //LoggerManager.Logger.Error(string.Format("Send alarm message to ECU911 failure,error message : {0}！", result.ErrorMsg));
            }
        }

        private PublicTrafficInfo GetPTInfo()
        {
            PublicTrafficInfo ptinfo = new PublicTrafficInfo 
            {
                BusNumber = "GPD0045",
                mdvr_core_sn = new Guid().ToString(),
                GPSTime =  DateTime.Now,
                AssigedArea = "AlarmAddressCode",//model.DistrictCode
                Mobile = "13982920393",
                AlarmOrignalTypeId = 14,
                ReportTime = DateTime.Now,
                VehicleType = 2,
                AccidentTime = DateTime.Now,
                Longitude = Convert.ToDouble(0.00),
                Latitude = Convert.ToDouble(0.00),
            };
            return ptinfo;
        }
    }
}

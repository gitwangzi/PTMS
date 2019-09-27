using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Bing;
using Gsafety.PTMS.Share;
using System.Json;

namespace Gsafety.Common.Controls
{
    public class BingMapLoad
    {
        public static void InitBingMap(ESRI.ArcGIS.Client.Map myMap)
        {
            WebClient webClient = new WebClient();
            string uri = string.Format("http://dev.virtualearth.net/REST/v1/Imagery/Metadata/Aerial?supressStatus=true&Key={0}", ApplicationContext.Instance.ServerConfig.BingKey);

            webClient.OpenReadCompleted += (s, a) =>
            {
                if (a.Error == null)
                {
                    JsonValue jsonResponse = JsonObject.Load(a.Result);
                    string authenticationResult = jsonResponse["authenticationResultCode"];
                    a.Result.Close();

                    if (authenticationResult == "ValidCredentials")
                    {
                        ESRI.ArcGIS.Client.Bing.TileLayer tileLayer = new ESRI.ArcGIS.Client.Bing.TileLayer()
                        {
                            ID = "BingMap",
                            LayerStyle = TileLayer.LayerType.Road,
                            ServerType = ServerType.Production,
                            Token = ApplicationContext.Instance.ServerConfig.BingKey
                        };
                        myMap.Layers.Insert(0, tileLayer);
                    }
                }
            };
            webClient.OpenReadAsync(new System.Uri(uri));
        }
    }
}

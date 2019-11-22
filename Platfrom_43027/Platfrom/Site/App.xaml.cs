using GisManagement.ViewModels;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.Event;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;
using System.Threading;
using System.Windows.Threading;
using System.Net.Browser;
using Gsafety.PTMS.Enums;
using Gsafety.Common.Controls;

namespace PTMS
{
    public partial class App : Application
        , IEventSink<DeleteUser>
        , IEventSink<ChangeUser>
    {
        const string DefaultLanguage = "zh-CN";
        string DefalutDateFormat = "yyyy-MM-dd";
        [Import]
        public IEventAggregator EventAggregator { get; set; }
        public App()
        {
            //必要的
#if DEBUG
            var result = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            result = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
#endif
            SetLanguage();
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitParams(e);
            if (!string.IsNullOrEmpty(ApplicationContext.Instance.ServerConfig.DateFormat))
            {
                DefalutDateFormat = ApplicationContext.Instance.ServerConfig.DateFormat;
            }
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = DefalutDateFormat;
            Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = DefalutDateFormat;
            if (!string.IsNullOrEmpty(ApplicationContext.Instance.ServerConfig.LongDateFormat))
            {
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongDatePattern = ApplicationContext.Instance.ServerConfig.LongDateFormat;
                Thread.CurrentThread.CurrentUICulture.DateTimeFormat.LongDatePattern = ApplicationContext.Instance.ServerConfig.LongDateFormat;

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
                Thread.CurrentThread.CurrentUICulture.DateTimeFormat.LongTimePattern = "HH:mm:ss";

                Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
                Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            }
            new VechileMemDataOperate();
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, e.ExceptionObject);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(GetType().FullName, ex);
            }
            finally
            {
                e.Handled = true;
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }


        private void InitParams(StartupEventArgs e)
        {
            Dictionary<string, string> configParams = e.InitParams as Dictionary<string, string>;
            foreach (var param in configParams)
            {
                if (string.IsNullOrEmpty(param.Value))
                    continue;

                switch (param.Key)
                {
                    case "GisBaseMapUrl":
                        ApplicationContext.Instance.ServerConfig.GisBaseMapUrl = param.Value;
                        break;
                    case "DomGisBaseMapUrl":
                        ApplicationContext.Instance.ServerConfig.DomGisBaseMapUrl = param.Value;
                        break;
                    case "MapInitExtent":
                        ApplicationContext.Instance.ServerConfig.MapInitExtent = param.Value;
                        break;
                    case "OverMapMaximumExtent":
                        ApplicationContext.Instance.ServerConfig.OverMapMaximumExtent = param.Value;
                        break;
                    case "ServiceUrlConfig":
                        ApplicationContext.Instance.ServerConfig.ServiceUrlConfig = param.Value;
                        break;
                    case "LayersSearchParams":
                        ApplicationContext.Instance.ServerConfig.LayersSearchParams = param.Value;
                        break;
                    case "FacilitySpeed":
                        ApplicationContext.Instance.ServerConfig.FacilitySpeed = param.Value;
                        break;
                    case "AutoLocateResolution":
                        ApplicationContext.Instance.ServerConfig.AutoLocateResolution = param.Value;
                        break;
                    case "MinResolution":
                        ApplicationContext.Instance.ServerConfig.MinResolution = param.Value;
                        break;
                    case "MaxResolution":
                        ApplicationContext.Instance.ServerConfig.MaxResolution = param.Value;
                        break;
                    case "GisGemetryServiceUrl":
                        ApplicationContext.Instance.ServerConfig.GisGeometryServiceUrl = param.Value;
                        break;
                    case "DistQueryGisLayerID":
                        ApplicationContext.Instance.ServerConfig.DistQueryGisID = param.Value;
                        break;
                    case "DateFormat":
                        ApplicationContext.Instance.ServerConfig.DateFormat = param.Value;
                        break;
                    case "videoServiceFileServerIp":
                        ApplicationContext.Instance.ServerConfig.VideoServiceFileServerIP = param.Value;
                        break;
                    case "videoServiceFileServerPort":
                        ApplicationContext.Instance.ServerConfig.VideoServiceFileServerPort = param.Value;
                        break;
                    case "RTSPServiceIP":
                        ApplicationContext.Instance.ServerConfig.RTSPServiceIP = param.Value;
                        break;
                    case "RTSPServicePort":
                        ApplicationContext.Instance.ServerConfig.RTSPServicePort = param.Value;
                        break;
                    case "RTSPStreamChannel":
                        ApplicationContext.Instance.ServerConfig.RTSPStreamChannel = param.Value;
                        break;
                    case "AlarmParamAlarmBeforeTime":
                        ApplicationContext.Instance.ServerConfig.AlarmParamAlarmBeforeTime = Convert.ToInt32(param.Value);
                        break;
                    case "AlarmParamAlarmEndTime":
                        ApplicationContext.Instance.ServerConfig.AlarmParamAlarmEndTime = Convert.ToInt32(param.Value);
                        break;
                    case "AlarmParamRelatedData":
                        ApplicationContext.Instance.ServerConfig.AlarmParamRelatedData = Convert.ToInt32(param.Value);
                        break;
                    case "Map":
                        switch (param.Value)
                        {
                            case "ArcGisMap":
                                ApplicationContext.Instance.ServerConfig.BaseMapType = BaseMapTypeEnum.ArcGisMap;
                                break;
                            case "BingMap":
                                ApplicationContext.Instance.ServerConfig.BaseMapType = BaseMapTypeEnum.BingMap;
                                ApplicationContext.Instance.ServerConfig.BingKey = configParams.Single(x => x.Key == "BingKey").Value;
                                break;
                            case "GoogleMap":
                                ApplicationContext.Instance.ServerConfig.BaseMapType = BaseMapTypeEnum.GoogleMap;
                                break;
                            case "BaiduMap":
                                ApplicationContext.Instance.ServerConfig.BaseMapType = BaseMapTypeEnum.BaiduMap;
                                break;
                            case "TsMap":
                                ApplicationContext.Instance.ServerConfig.BaseMapType = BaseMapTypeEnum.TsMap;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "MapSubType":
                        ApplicationContext.Instance.ServerConfig.MapSubType = param.Value;
                        break;
                    case "Bias":
                        ApplicationContext.Instance.ServerConfig.Bias = Convert.ToBoolean(param.Value);
                        break;
                    case "LongDateFormat":
                        ApplicationContext.Instance.ServerConfig.LongDateFormat = param.Value;
                        break;
                    case "DefaultVideoConnectTimeOut":
                        ApplicationContext.Instance.ServerConfig.DefaultVideoConnectTimeOut = int.Parse(param.Value);
                        break;
                    case "DisplayParameterMode":
                        ApplicationContext.Instance.ServerConfig.DisplayParameter.DisplayParameterMode = bool.Parse(param.Value);
                        break;
                    case "DisplayParameter":
                        ApplicationContext.Instance.ServerConfig.DisplayParameter.ParseVideoFileParameter(param.Value);
                        break;
                    case "CultureInfo":
                        ApplicationContext.Instance.ServerConfig.Culture = param.Value;
                        break;
                    case "GoogleAddress":
                        ApplicationContext.Instance.ServerConfig.GoogleAddress = param.Value;
                        break;
                    case "MapLanguage":
                        ApplicationContext.Instance.ServerConfig.MapLanguage = param.Value;
                        break;
                    case "EnglishHelpUrl":
                        ApplicationContext.Instance.ServerConfig.EnglishHelpUrl = param.Value;
                        break;
                    case "ChineseHelpUrl":
                        ApplicationContext.Instance.ServerConfig.ChineseHelpUrl = param.Value;
                        break;
                    case "SpanishHelpUrl":
                        ApplicationContext.Instance.ServerConfig.SpanishHelpUrl = param.Value;
                        break;
                    case "BiasType":
                        ApplicationContext.Instance.ServerConfig.BiasType = param.Value;
                        break;
                    case "BiasX":
                        ApplicationContext.Instance.ServerConfig.BiasX = double.Parse(param.Value);
                        break;
                    case "BiasY":
                        ApplicationContext.Instance.ServerConfig.BiasY = double.Parse(param.Value);
                        break;
                    case "Layers":
                        ApplicationContext.Instance.ServerConfig.Layers = param.Value;
                        break;
                    case "MatrixSet":
                        ApplicationContext.Instance.ServerConfig.MatrixSet = param.Value;
                        break;
                    case "TileMatrixSet":
                        ApplicationContext.Instance.ServerConfig.TileMatrixSet = param.Value;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetLanguage()
        {
            try
            {
                string language = HtmlPage.Window.Invoke("GetLanguage", new object[] { }).ToString();
                if (string.IsNullOrEmpty(language))
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(DefaultLanguage);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DefaultLanguage);
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
                }
            }
            catch (Exception ex)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(DefaultLanguage);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DefaultLanguage);
                ApplicationContext.Instance.Logger.LogException("GetLanguge", ex);
            }
        }

        public void HandleEvent(DeleteUser publishedEvent)
        {
            if (ApplicationContext.Instance.AuthenticationInfo.UserName.Equals(publishedEvent.UserName))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("UserInfoChange"));
            }
        }

        public void HandleEvent(ChangeUser publishedEvent)
        {
            if (ApplicationContext.Instance.AuthenticationInfo.UserName.Equals(publishedEvent.UserName))
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("UserInfoDelete"));
            }
        }
    }
}

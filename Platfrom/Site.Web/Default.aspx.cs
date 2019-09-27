using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace PTMS.Web
{
    public partial class Default : System.Web.UI.Page
    {
        private const string LanguageConfig = "CultureInfo";
        public string ServerConfigInfor = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ConfigurationManager.AppSettings.AllKeys.Contains(LanguageConfig))
                {
                    this.language.Value = ConfigurationManager.AppSettings[LanguageConfig].ToString();
                }
                else
                {
                    this.language.Value = "zh-CN";
                }

               

                GetServerConfigInfo();
            }
        }

        private void GetServerConfigInfo()
        {
            NameValueCollection serverConfigs = ConfigurationManager.AppSettings;

            StringBuilder sb = new StringBuilder();
            int settingCount = serverConfigs.Count;
            for (int i = 0; i < settingCount; i++)
            {
                sb.Append(serverConfigs.GetKey(i));
                sb.Append("=");
                sb.Append(serverConfigs[i]);
                sb.Append(",");
            }

            //SrvConfig
            sb.Append(",ServiceUrlConfig=");
            sb.Append(GetServiceConfig());
        
            //search layer config
            sb.Append(",LayersSearchParams");
            sb.Append("=");
            sb.Append(GetLayersInfoParams());

            ServerConfigInfor = sb.ToString();
        }

        private string GetServiceConfig()
        {
            var doc = new XmlDocument();
            doc.Load(Server.MapPath("./ServiceConfig.xml"));
            return doc.OuterXml;
        }
        /// <summary>
        /// 得到地址查询配置
        /// </summary>
        /// <returns></returns>
        private string GetLayersInfoParams()
        {
            var doc = new XmlDocument();
            doc.Load(Server.MapPath("./GISLayerSearch.xml"));
            return doc.OuterXml;
        }
    }
}
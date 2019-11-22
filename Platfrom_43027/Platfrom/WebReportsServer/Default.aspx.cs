using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebReportsServer
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MsgInfo.InnerHtml = ex.Message;
            }
        }

        private string GetConfig(string appConfigKeyName)
        {
            string strValue = System.Configuration.ConfigurationManager.AppSettings[appConfigKeyName];
            if (string.IsNullOrWhiteSpace(strValue) || strValue.Trim() == "")
            {
                string errMsg = "config error!&nbsp;&nbsp;key:" + appConfigKeyName + "<br/>"
                    + "Example:<br/>"
                    + HttpUtility.HtmlEncode("<appSettings>") + "<br/>"
                    + HttpUtility.HtmlEncode("<add key=\"" + appConfigKeyName + "\" value=\"xxxxx\"/>") + "<br>"
                    + "......";
                throw new Exception(errMsg);
            }
            return strValue;
        }
    }
}
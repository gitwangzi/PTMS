using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebReports
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string iDocID = Request["iDocID"];
                //string token=string.Empty;
                //string tokenEncode = string.Empty;
                //if (Session["token"] == null || Session["token"].ToString().Trim() == "")
                //{
                //    SessionMgr mySessionMgr = new SessionMgr();
                //    EnterpriseSession myEnSession;
                //    string username = "administrator";
                //    string pwd = "wang_123";
                //    string apsAuthType = "secEnterprise";
                //    myEnSession = mySessionMgr.Logon(username, pwd, "WIN-BUOLGRGCPTR:6400", apsAuthType);
                //    token = myEnSession.LogonTokenMgr.CreateLogonTokenEx("", 120, 100);
                //    tokenEncode = System.Web.HttpUtility.UrlEncode(token, new System.Text.UTF8Encoding());
                //    myEnSession.Logoff();
                //    Session["token"] = token;
                //}

                //Response.Redirect("http://172.16.20.89:8080/BOE/OpenDocument/opendoc/openDocument.jsp?token=" + tokenEncode + "&sIDType=CUID&iDocID=" + iDocID);
            }
            catch (Exception ex)
            {
                MsgInfo.InnerHtml = ex.Message;
            }
        }
    }
}
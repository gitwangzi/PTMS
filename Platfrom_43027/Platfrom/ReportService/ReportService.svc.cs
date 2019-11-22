using DevExpress.Data.Utils.ServiceModel;
using Gsafety.Common.Logging;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Report.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService" in code, svc and config file together.
    [SilverlightFaultBehavior]
    public class ReportService : DevExpress.XtraReports.Service.ReportService
    {
        const string DefaultLanguage = "zh-CN";
        public ReportService()
        {
            try
            {
                string language = "";
                if (ConfigurationManager.AppSettings.AllKeys.Contains("CultureInfo"))
                {
                    language = ConfigurationManager.AppSettings["CultureInfo"].ToString();
                }
                else
                {
                    language = "zh-CN";
                }
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(DefaultLanguage);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DefaultLanguage);
            }
        }
    }
}

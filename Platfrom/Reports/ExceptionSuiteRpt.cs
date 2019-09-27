using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using System.Collections.Generic;
using Gsafety.Common.Logging;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Linq;
namespace Gsafety.PTMS.Reports
{
    public partial class ExceptionSuiteRpt : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";

        public ExceptionSuiteRpt()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrEventStyle.BackColor = Color.FromArgb(233, 238, 244);
                //if (ConfigurationManager.AppSettings.AllKeys.Contains("DateFormat"))
                //{
                //    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DateFormat"].ToString()))
                //    {
                //        strDateFormat = ConfigurationManager.AppSettings["DateFormat"].ToString();
                //    }
                //}
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("ExceptionSuiteRpt()", ex);
            }
        }

        private void ExceptionSuiteRpt_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());
                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = rpTitle.Text = reader.GetString("Rpt_ExceptionSuite_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";

                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);

                tCPHM.Text = reader.GetString("Rpt_ExceptionSuite_CelCPHM");
                tYCYY.Text = reader.GetString("Rpt_ExceptionSuite_CelYCYY");
                tSN.Text = reader.GetString("Rpt_ExceptionSuite_CelTJSN");
                //tID.Text = reader.GetString("Rpt_ExceptionSuite_ID");
                //celID.DataBindings.Add("Text", DataSource, "SUITE_INFO_ID");
                colSN.DataBindings.Add("Text", DataSource, "MDVR_CORE_SN");
                colYCYY.DataBindings.Add("Text", DataSource, "ABNORMAL_CAUSE");
                colCPHM.DataBindings.Add("Text", DataSource, "VEHICLE_ID");
                DataTable dt = repository.GetDataForExceptionSuiteRpt();
                if (dt.Rows.Count > 0)
                {
                    this.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("ExceptionSuiteRpt_DataSourceDemanded", ex);
            }
        }

        private void colYCYY_EvaluateBinding(object sender, BindingEventArgs e)
        {

        }

    }
}

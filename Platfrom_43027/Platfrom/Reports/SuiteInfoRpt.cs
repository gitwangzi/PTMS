using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Text;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Report.Repository;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using System.Reflection;
using System.Configuration;

namespace Gsafety.PTMS.Reports
{
    public partial class SuiteInfoRpt : DevExpress.XtraReports.UI.XtraReport
    {
        ReportStatisticsRepository repository = null;
        StringResourceReader reader = new StringResourceReader();
        //string strDateFormat = "yyyy-MM-dd";
        public SuiteInfoRpt()
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
                LoggerManager.Logger.Error("SuiteInfoRpt()", ex);
            }
        }
        private void SuiteInfoRpt_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                xrChart1.Titles[0].Text = reader.GetString("Report_SuiteInfo_Title");
                this.DisplayName = rpTitle.Text = reader.GetString("Report_SuiteInfo_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);

                tBFB.Text = reader.GetString("Report_SuiteInfo_Percentage");
                tGS.Text = reader.GetString("Report_SuiteInfo_Toatl");
                tZT.Text = reader.GetString("Report_SuiteInfo_Status");
                this.xrChart1.Series[0].LegendText = reader.GetString("Rpt_Public_Count");


                colZT.DataBindings.Add("Text", DataSource, "statustype");
                colGS.DataBindings.Add("Text", DataSource, "toatl");
                colBFB.DataBindings.Add("Text", DataSource, "percentage");

                DataTable dtTemp = repository.GetDataForSuiteInfoRpt();
                this.DataSource = dtTemp;
                DataTable dtPie = dtTemp.Copy();

                dtPie.Rows.RemoveAt(dtTemp.Rows.Count - 1);
                if (dtPie.Rows.Count < 1)
                {
                    this.xrChart1.Visible = false;
                }
                else
                {
                    this.xrChart1.DataSource = dtPie;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("SuiteInfoRpt_DataSourceDemanded", ex);
            }
        }
    }
}

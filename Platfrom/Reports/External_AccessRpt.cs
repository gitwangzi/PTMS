using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Gsafety.Common.Logging;
using System.Configuration;
using System.Reflection;

namespace Gsafety.PTMS.Reports
{
    public partial class External_AccessRpt : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";
        public External_AccessRpt()
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
                LoggerManager.Logger.Error("External_AccessRpt()", ex);
            }
        }

        private void External_AccessRpt_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";
                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);
                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = rpTitle.Text = reader.GetString("Rpt_External_Access_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);

                tTime.Text = reader.GetString("Rpt_External_Access_COLTIME");
                tSSSP.Text = reader.GetString("E8_VEN911");
                tYJBJGPS.Text = reader.GetString("Rpt_External_Access_COLYJBJGPS");
                tLSGPS.Text = reader.GetString("Rpt_External_Access_COLLSGPS");
                tRCGPS.Text = reader.GetString("Rpt_External_Access_COLRCGPS");
                tLSSP.Text = reader.GetString("E8_ECU911");
                tZS.Text = reader.GetString("Rpt_External_Access_COLZS");
                this.xrChart1.Series[0].LegendText = reader.GetString("Rpt_Public_Count");
                this.xrChart1.Titles[0].Text = reader.GetString("Rpt_External_Access_Title");

                colTime.DataBindings.Add("Text", DataSource, "COLTIME");
                colLSGPSSJ.DataBindings.Add("Text", DataSource, "COLLSGPS");
                colSSSP.DataBindings.Add("Text", DataSource, "COLSSSP");
                colRCGPSSJ.DataBindings.Add("Text", DataSource, "COLRCGPS");
                colYJBJGPSSJ.DataBindings.Add("Text", DataSource, "COLYJBJGPS");
                colLSSP.DataBindings.Add("Text", DataSource, "COLLSSP");
                colZS.DataBindings.Add("Text", DataSource, "COLZS");


                DataTable dtSource = repository.GetDataForExternal_AccessRpt();
                if (dtSource == null || dtSource.Rows.Count < 1)
                {
                    xrChart1.Visible = false;
                    return;
                }
                List<PieData> list = repository.CreatePieDataForExternal_AccessRpt();
                if (list.Count < 1)
                {
                    xrChart1.Visible = false;
                }
                else
                {
                    this.xrChart1.Series[0].DataSource = list;
                }
                this.DataSource = dtSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("External_AccessRpt_DataSourceDemanded", ex);
            }

        }

        private void colTime_EvaluateBinding(object sender, BindingEventArgs e)
        {
            try
            {
                DateTime dt;
                if (DateTime.TryParse(e.Value.ToString(), out dt))
                {
                    e.Value = dt.ToString(repository.DateFormat);
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("colTime_EvaluateBinding", ex);
            }
        }

    }
}

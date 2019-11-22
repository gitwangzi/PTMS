using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using Gsafety.PTMS.Report.Repository;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Gsafety.PTMS.Reports
{
    public partial class VehicleOnTimeRpt : DevExpress.XtraReports.UI.XtraReport
    {
        ReportStatisticsRepository repository = null;
        StringResourceReader reader = new StringResourceReader();
        //string strDateFormat = "yyyy-MM-dd";
        public VehicleOnTimeRpt()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrEventStyle.BackColor = Color.FromArgb(233, 238, 244);
                this.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);
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
                LoggerManager.Logger.Error("VehicleOnTimeRpt()", ex);
            }
        }

        private void VehicleOnTimeRpt_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";
                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);
                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = rpTitle.Text = this.xrChart1.Titles[0].Text = reader.GetString("Rpt_Vehicle_OnTime_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);

                xrtOnLine.Text = reader.GetString("Rpt_Vehicle_OnTime_OnLine");
                xrtTime.Text = reader.GetString("Rpt_Vehicle_OnTime_Time");
                xrtAvgTime.Text = reader.GetString("Rpt_Vehicle_OnTime_AvgTime");
                tDISTANCE.Text = reader.GetString("Rpt_Vehicle_OnTime_Distance");

                this.xrChart1.Series[0].LegendText = xrtOnLine.Text;
                this.xrChart1.Series[1].LegendText = xrtAvgTime.Text;
                this.xrChart1.Series[2].LegendText = tDISTANCE.Text;

                colTime.DataBindings.Add("Text", DataSource, "COLTIME");
                colOnline.DataBindings.Add("Text", DataSource, "COLONLINECOUNT");
                colAvgTimespan.DataBindings.Add("Text", DataSource, "COLAVGTIMESPAN");
                celDISTANCE.DataBindings.Add("Text", DataSource, "DISTANCE");
                DataTable dt = repository.GetDataForVehicleOnTimeRpt();

                if (dt == null)
                {
                    this.xrChart1.Visible = false;
                    return;
                }

                DataTable dtPie = dt.Copy();

                this.xrChart1.DataSource = dtPie;
                this.DataSource = dt;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("VehicleOnTimeRpt_DataSourceDemanded", ex);
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

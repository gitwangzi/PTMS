using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using System.Data;
using System.Collections.Generic;
using Gsafety.Common.Logging;
using System.Configuration;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class VehicleAlertRpt : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";
        public VehicleAlertRpt()
        {
            try
            {
                InitializeComponent();
                this.PageSize = new Size(1000, 1000);
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
                LoggerManager.Logger.Error("VehicleAlertRpt()", ex);
            }
        }

        private void VehicleAlertRpt_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";
                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);
                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = rpTitle.Text = reader.GetString("Rpt_Vehicle_Alert_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);

                tTime.Text = reader.GetString("Rpt_Vehicle_Alert_COLTIME");
                tJDZWL.Text = reader.GetString("Rpt_Vehicle_Alert_COLJDZWL");
                tCDZWL.Text = reader.GetString("Rpt_Vehicle_Alert_COLCDZWL");
                tPTCS.Text = reader.GetString("Rpt_Vehicle_Alert_COLPTCS");

                tWLNCS.Text = reader.GetString("Rpt_Vehicle_Alert_COLWLNCS");
                tWLNDS.Text = reader.GetString("Rpt_Vehicle_Alert_COLWLNDS");
                //tYCKM.Text = reader.GetString("Rpt_Vehicle_Alert_COLYCKM");
                tZS.Text = reader.GetString("Rpt_Vehicle_Alert_COLZS");
                this.xrChart1.Series[0].LegendText = reader.GetString("Rpt_Public_Count");
                this.xrChart1.Titles[0].Text = reader.GetString("Rpt_Vehicle_Alert_Title");


                colTime.DataBindings.Add("Text", DataSource, "COLTIME");
                colJDZWL.DataBindings.Add("Text", DataSource, "COLJDZWL");
                colCDZWL.DataBindings.Add("Text", DataSource, "COLCDZWL");
                colWLNCS.DataBindings.Add("Text", DataSource, "COLWLNCS");
                colWLNDS.DataBindings.Add("Text", DataSource, "COLWLNDS");
                //colYCKM.DataBindings.Add("Text", DataSource, "COLYCKM");
                colPTCS.DataBindings.Add("Text", DataSource, "COLPTCS");
                colZS.DataBindings.Add("Text", DataSource, "CELZS");

                DataTable dt = repository.GetDataForVehicleAlertRpt();
                if (dt == null || dt.Rows.Count < 1)
                {
                    xrChart1.Visible = false;
                    return;
                }
                List<PieData> list = repository.CreatePieDataForVehicleAlertRpt();
                if (list.Count < 1)
                {
                    xrChart1.Visible = false;
                }
                else
                {
                    this.xrChart1.Series[0].DataSource = list;
                }
                this.DataSource = dt;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
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

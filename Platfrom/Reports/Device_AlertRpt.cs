using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using Gsafety.Common.Logging;
using System.Data;
using System.Linq;
using System.Configuration;
using System.Reflection;

namespace Gsafety.PTMS.Reports
{
    public partial class Device_AlertRpt : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";
        public Device_AlertRpt()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrEventStyle.BackColor = Color.FromArgb(233, 238, 244);
                xrChart1.Titles[0].TextColor = Color.FromArgb(110, 110, 110);

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
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }

        private void DeviceAlertRpt_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";
                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);
                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = title.Text = reader.GetString("Rpt_Device_Alert_Title");
                this.xrChart1.Titles[0].Text = reader.GetString("Rpt_Device_Alert_Title");
                title.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);              
                colSXTWXH.Text = reader.GetString("Rpt_Device_Alert_COLCW");
                tGPSJSJGZ.Text = reader.GetString("Rpt_Device_Alert_COLGPSJSJGZ");
                tSXTZD.Text = reader.GetString("LowTemperature");
                tMDVRSDKCW.Text = reader.GetString("Rpt_Device_Alert_COLMDVRSDCW");
                tSXTWXH.Text = reader.GetString("Rpt_Device_Alert_COLSXTWXH");
                tSCMMCW.Text = reader.GetString("Rpt_Device_Alert_COLSCMMCW");
                tDYYC.Text = reader.GetString("Rpt_Device_Alert_COLDYYC");
                tZS.Text = reader.GetString("Rpt_Device_Alert_COLZS");
                tTime.Text = reader.GetString("Rpt_Vehicle_Alert_COLTIME");
                tCW.Text = reader.GetString("MAINTAIN_HightTemperature");
                this.xrChart1.Series[0].LegendText = reader.GetString("Rpt_Public_Count");

                colTime.DataBindings.Add("Text", DataSource, "COLTIME");
                colCW.DataBindings.Add("Text", DataSource, "COLCW");
                colGpsJSJGZ.DataBindings.Add("Text", DataSource, "COLGPSJSJGZ");
                colSXTZD.DataBindings.Add("Text", DataSource, "COLSXTZD");
                colSXTWXH.DataBindings.Add("Text", DataSource, "COLSXTWXH");
                colSDCW.DataBindings.Add("Text", DataSource, "COLMDVRSDCW");
                colSCMMCW.DataBindings.Add("Text", DataSource, "COLSCMMCW");
                colDYYC.DataBindings.Add("Text", DataSource, "COLDYYC");
                colZS.DataBindings.Add("Text", DataSource, "COLZS");


                DataTable dtsource = repository.GetDataForDeviceAlertRpt();
                if (dtsource == null || dtsource.Rows.Count < 1)
                {
                    xrChart1.Visible = false;
                    return;
                }

                List<PieData> list = repository.CreatePieDataForDeviceAlertRpt();

                if (list.Count < 1)
                {
                    xrChart1.Visible = false;
                    return;
                }
                this.xrChart1.Series[0].DataSource = list;
                this.DataSource = dtsource;
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
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }
    }
}

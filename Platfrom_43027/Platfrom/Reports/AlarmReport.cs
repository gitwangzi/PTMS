using DevExpress.XtraPivotGrid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UI.PivotGrid;
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Text;
using Gsafety.PTMS.DBEntity;
using Gsafety.PTMS.Report.Repository;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using DevExpress.XtraPrinting;
using System.Configuration;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class AlarmReport : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";
        public AlarmReport()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrControlStyle1.BackColor = Color.FromArgb(233, 238, 244);
                xrChart1.Titles[0].TextColor = Color.FromArgb(110, 110, 110);
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }

        private void AlarmReport_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";


                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);

                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));
                xrtToatl.Text = reader.GetString("Rpt_Alarm_Toatl");
                xrtTrueTotal.Text = reader.GetString("Rpt_Alarm_True");

                xrt911Result.Text = reader.GetString("Rpt_Alarm_911_Result");
                xrtPolice.Text = reader.GetString("Rpt_Alarm_911_Police");
                xrtFieControl.Text = reader.GetString("Rpt_Alarm_911_FireControl");
                xrtTraffic.Text = reader.GetString("Rpt_Alarm_911_Traffic");
                xrtRisk.Text = reader.GetString("Rpt_Alarm_911_Risk");
                xrtTime.Text = reader.GetString("Rpt_Alarm_Time");
                xrtMedical.Text = reader.GetString("Rpt_Alarm_911_Medical");
                xrtArmy.Text = reader.GetString("Rpt_Alarm_911_Army");
                xrtMunicipal.Text = reader.GetString("Rpt_Alarm_911_Municipal");
                
                this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_Alarm_Title");
                xrChart1.Titles[0].Text = reader.GetString("Rpt_Alarm_Title");
                xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                colCreateTime.DataBindings.Add("Text", DataSource, "CREATETIME");
                colAlarmZS.DataBindings.Add("Text", DataSource, "ZONGSHUGG");
                colTrueZS.DataBindings.Add("Text", DataSource, "T");

                JC1.DataBindings.Add("Text", DataSource, "JC1");
                XF2.DataBindings.Add("Text", DataSource, "XF2");
                FX3.DataBindings.Add("Text", DataSource, "FX3");
                JT4.DataBindings.Add("Text", DataSource, "JT4");
                YL5.DataBindings.Add("Text", DataSource, "YL5");
                JD6.DataBindings.Add("Text", DataSource, "JD6");
                SZ7.DataBindings.Add("Text", DataSource, "SZ7");

                DataTable dtSource = repository.GetDataForAlarmReport();
                if (dtSource == null)
                {
                    xrChart1.Visible = false;
                    return;
                }
                List<PieData> list = repository.CreatePieDataForAlarmRpt();
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
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }
        private void colCreateTime_EvaluateBinding(object sender, BindingEventArgs e)
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

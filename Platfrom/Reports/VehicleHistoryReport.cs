using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Report.Repository;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class VehicleHistoryReport : ReportBase
    {
        public VehicleHistoryReport()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrControlStyle1.BackColor = Color.FromArgb(233, 238, 244);
                ChangeFont();
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }

        protected override void ChangeFont()
        {
            this.xrTable1.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xrPageInfo1.Font = new System.Drawing.Font(ReportBase.FontName, 10F);
            this.xrLTime.Font = new System.Drawing.Font(ReportBase.FontName, 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rpt_StartTime.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cell_Rpt_StartTime.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Rpt_EndTime.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cell_Rpt_EndTime.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xrtTitle.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Bold);
            this.xrTable2.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Font = new System.Drawing.Font(ReportBase.FontName, 9.75F);
        }

        private void AlarmReport_DataSourceDemanded(object sender, EventArgs e)
        {

            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";

                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTimeLocal.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTimeLocal.ToString(repository.DateFormat);

                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));
                xrtVehicle.Text = reader.GetString("VehicleNumber");
                xrtOrganization.Text = reader.GetString("Rpt_Organization");
                xrtVehicleType.Text = reader.GetString("Rpt_VehicleType");
                xrtDistrict.Text = reader.GetString("Rpt_District");
                xrtLon.Text = reader.GetString("ALARM_Longitude");
                xrtLat.Text = reader.GetString("ALARM_Latitude");
                xrtDir.Text = reader.GetString("Dir");
                xrtSpeed.Text = reader.GetString("ALERT_Speed");
                xrtTime.Text = reader.GetString("GIS_GpsTime");

                this.DisplayName = xrtTitle.Text = reader.GetString("GIS_OpenHistoryTrace");
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
                {
                    this.DisplayName = "History Trace Statistics";
                }

                //xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                colVehicle.DataBindings.Add("Text", DataSource, "vehicle_id");
                colOrganization.DataBindings.Add("Text", DataSource, "OrganizationName");
                colDistrict.DataBindings.Add("Text", DataSource, "districtname");
                colVehicleType.DataBindings.Add("Text", DataSource, "vehicletype");
                colLon.DataBindings.Add("Text", DataSource, "lon");
                colLat.DataBindings.Add("Text", DataSource, "lat");
                colDir.DataBindings.Add("Text", DataSource, "dir");
                colSpeed.DataBindings.Add("Text", DataSource, "speed");
                colTime.DataBindings.Add("Text", DataSource, "gpstime");

                DataTable dtSource = repository.GetHistoryTraceStatisticsReport();


                foreach (DataRow row in dtSource.Rows)
                {
                    DateTime gpstime = DateTime.Parse(row["gpstime"].ToString());
                    row["gpstime"] = gpstime.AddHours(repository.WhereInfo.TimeZone);

                  
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

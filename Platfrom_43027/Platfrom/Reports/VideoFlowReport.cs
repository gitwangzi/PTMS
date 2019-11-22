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
    public partial class VideoFlowReport : ReportBase
    {
        public VideoFlowReport()
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
            this.colVehicleType.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
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
                xrtLength.Text = reader.GetString("Rpt_VideoFlow_TimeSpan");
                xrtFlow.Text = reader.GetString("Rpt_VideoFlow_VideoSize");

                this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_VideoFlow_Title");

                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
                {
                    this.DisplayName = "Video Flow Statistics";
                }

                //xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                colVehicle.DataBindings.Add("Text", DataSource, "vehicle_id");
                colOrganization.DataBindings.Add("Text", DataSource, "OrganizationName");
                colDistrict.DataBindings.Add("Text", DataSource, "districtname");
                colVehicleType.DataBindings.Add("Text", DataSource, "vehicletype");
                colLength.DataBindings.Add("Text", DataSource, "TimeSpanString");
                colFlow.DataBindings.Add("Text", DataSource, "VideoSizeValue");

                DataTable dtSource = repository.GetDataForVideoFlowStatisticsReport();
                DataColumn dcTimeSpanString = new DataColumn();
                dcTimeSpanString.DataType = typeof(string);
                dcTimeSpanString.ColumnName = "TimeSpanString";
                dtSource.Columns.Add(dcTimeSpanString);

                DataColumn dcVideoSizeValue = new DataColumn();
                dcVideoSizeValue.DataType = typeof(string);
                dcVideoSizeValue.ColumnName = "VideoSizeValue";
                dtSource.Columns.Add(dcVideoSizeValue);

                foreach (DataRow row in dtSource.Rows)
                {
                    if (row["TimeSpan"] == DBNull.Value)
                    {
                        row["TimeSpanString"] = "00:00:00";
                    }
                    else
                    {
                        TimeSpan span = TimeSpan.FromSeconds(Convert.ToInt64(row["TimeSpan"].ToString()));
                        row["TimeSpanString"] = (span.Days * 24 + span.Hours).ToString() + ":" + span.Minutes.ToString("d2") + ":" + span.Seconds.ToString("d2");
                    }

                    if (row["VideoSize"] != DBNull.Value)
                    {
                        double videosize = Convert.ToDouble(row["VideoSize"].ToString());
                        videosize = videosize / (1024 * 1024);
                        row["VideoSizeValue"] = videosize.ToString("F1");
                    }
                    else
                    {
                        row["VideoSizeValue"] = "0";
                    }
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

using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Report.Repository;
using System;
using System.Data;
using System.Drawing;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class AlarmStatisticsReport : ReportBase
    {
        public AlarmStatisticsReport()
        {
            try
            {
                InitializeComponent();
                this.ForeColor = Color.FromArgb(110, 110, 110);
                xrControlStyle1.BackColor = Color.FromArgb(233, 238, 244);
                xrChart1.Titles[0].TextColor = Color.FromArgb(110, 110, 110);
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
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font(ReportBase.FontName, 8F);
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font(ReportBase.FontName, 8F);
            this.xrChart1.Legend.Font = new System.Drawing.Font(ReportBase.FontName, 8F);
            chartTitle1.Font = new System.Drawing.Font(ReportBase.FontName, 14F, System.Drawing.FontStyle.Bold);
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
                if (repository.WhereInfo.GroupMode == 1)
                {
                    xrtGroup.Text = reader.GetString("Year");
                }
                else if (repository.WhereInfo.GroupMode == 2)
                {
                    xrtGroup.Text = reader.GetString("Month");
                }
                else if (repository.WhereInfo.GroupMode == 3)
                {
                    xrtGroup.Text = reader.GetString("Week");
                }
                else if (repository.WhereInfo.GroupMode == 4)
                {
                    xrtGroup.Text = reader.GetString("Day");
                }
                else if (repository.WhereInfo.GroupMode == 5)
                {
                    xrtGroup.Text = reader.GetString("Organization");
                }
                else if (repository.WhereInfo.GroupMode == 6)
                {
                    xrtGroup.Text = reader.GetString("Province");
                }
                else if (repository.WhereInfo.GroupMode == 7)
                {
                    xrtGroup.Text = reader.GetString("City");
                }
                else if (repository.WhereInfo.GroupMode == 8)
                {
                    xrtGroup.Text = reader.GetString("BASEINFO_Vehicle");
                }
                xrtTotal.Text = reader.GetString("Rpt_Alarm_Toatl");

                xrtHandled.Text = reader.GetString("Rpt_Alarm_Handled");
                //xrtGroup.Text = reader.GetString("Rpt_Alarm_Time");
                xrtTrue.Text = reader.GetString("Rpt_Alarm_True");
                xrtFalse.Text = reader.GetString("Rpt_Alarm_False");
                xrtTransfer.Text = reader.GetString("Rpt_Transfer");
                this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_Alarm_Title");

                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
                {
                    this.DisplayName = "Alarm Statistics";
                }
                xrChart1.Titles[0].Text = reader.GetString("Rpt_Alarm_Title");
                //xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);
                this.xrChart1.Series[0].Name = reader.GetString("TotalAlarm");
                this.xrChart1.Series[1].Name = reader.GetString("TrueAlarm");
                this.xrChart1.Series[2].Name = reader.GetString("FalseAlarm");
                this.xrChart1.Series[3].Name = reader.GetString("TransferAlarm");

                colGroup.DataBindings.Add("Text", DataSource, "PROJECT");
                colTotal.DataBindings.Add("Text", DataSource, "ALARMNUM");
                colTrue.DataBindings.Add("Text", DataSource, "TRUEALARM");
                colFalse.DataBindings.Add("Text", DataSource, "FALSEALARM");
                colTransfer.DataBindings.Add("Text", DataSource, "IS_TRANSFER");

                DataTable dtSource = repository.GetDataForAlarmStatisticsReport();
                if (dtSource == null || dtSource.Rows.Count == 0)
                {
                    xrChart1.Visible = false;
                    return;
                }

                this.xrChart1.DataSource = dtSource.Copy();

                this.DataSource = dtSource;
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            }
        }

    }
}

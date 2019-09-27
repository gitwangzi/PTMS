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
using Gsafety.PTMS.Report.Repository;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using DevExpress.XtraPrinting;
using System.Configuration;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class UserOnlineReport : ReportBase
    {
        public UserOnlineReport()
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
                xrtOnlineTime.Text = reader.GetString("Rpt_Online_OnlineTime");
                xrtTimeSpan.Text = reader.GetString("Rpt_Online_TimeSpan");
                xrtOffline.Text = reader.GetString("Rpt_Online_OfflineTime");
                xrtAcountName.Text = reader.GetString("Rpt_Online_AcountName");

                this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_Online_Title");


                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
                {
                    this.DisplayName = "User Online Statistics";
                }
                //xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                colVehicle.DataBindings.Add("Text", DataSource, "LOGIN_USER");
                colOnlineTime.DataBindings.Add("Text", DataSource, "LOGIN_TIME");
                colOfflineTime.DataBindings.Add("Text", DataSource, "LOGOUT_TIME");
                colTimeSpan.DataBindings.Add("Text", DataSource, "TimeSpan");

                DataTable dtSource = repository.GetDataForUserOnlineStatisticsReport();
                DataColumn column = new DataColumn();
                column.ColumnName = "TimeSpan";
                column.DataType = typeof(string);
                dtSource.Columns.Add(column);

                foreach (DataRow row in dtSource.Rows)
                {
                    DateTime logintime = DateTime.Parse(row["LOGIN_TIME"].ToString());
                    row["LOGIN_TIME"] = logintime.AddHours(repository.WhereInfo.TimeZone);

                    if (row["LOGOUT_TIME"] != DBNull.Value)
                    {
                        DateTime logouttime = DateTime.Parse(row["LOGOUT_TIME"].ToString());
                        row["LOGOUT_TIME"] = logouttime.AddHours(repository.WhereInfo.TimeZone);

                        row["TimeSpan"] = logouttime.Subtract(logintime).ToString();
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

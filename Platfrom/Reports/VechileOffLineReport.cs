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
    public partial class VechileOffLineReport : ReportBase
    {
        public VechileOffLineReport()
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

                Rpt_EndTime.Text = reader.GetString("Duration") + ":";
                Rpt_StartTime.Text = reader.GetString("Time") + ":";

                TimeSpan span = repository.WhereInfo.BeginTime.Subtract(repository.WhereInfo.EndTime);

                cell_Rpt_EndTime.Text = span.TotalHours.ToString() + reader.GetString("Hour");
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTimeLocal.ToString(repository.DateFormat);

                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));
                xrtVehicle.Text = reader.GetString("VehicleNumber");
                xrtOrganization.Text = reader.GetString("Rpt_Organization");
                xrtVehicleType.Text = reader.GetString("Rpt_VehicleType");
                xrtDistrict.Text = reader.GetString("Rpt_District");
                xrtOwner.Text = reader.GetString("Rpt_Offline_Owner");
                xrtPhone.Text = reader.GetString("Rpt_Offline_Phone");

                this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_Offline_Title");
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "zh-CN")
                {
                    this.DisplayName = "User Offline Statistics";
                }

                //xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);


                colVehicle.DataBindings.Add("Text", DataSource, "vehicle_id");
                colOrganization.DataBindings.Add("Text", DataSource, "OrganizationName");
                colDistrict.DataBindings.Add("Text", DataSource, "districtname");
                colVehicleType.DataBindings.Add("Text", DataSource, "vehicletype");
                colOwner.DataBindings.Add("Text", DataSource, "owner");
                colPhone.DataBindings.Add("Text", DataSource, "contact_phone");

                DataTable dtSource = repository.GetDataForOffLineStatisticsReport();

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

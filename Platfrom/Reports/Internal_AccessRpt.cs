using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.Common.Logging;
using Gsafety.PTMS.Report.Repository;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Reports
{
    public partial class Internal_AccessRpt : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        //string strDateFormat = "yyyy-MM-dd";
        public Internal_AccessRpt()
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
                LoggerManager.Logger.Error("Internal_AccessRpt()", ex);
            }
        }

        private void Internal_AccessRpt_DataSourceDemanded(object sender, EventArgs e)
        {
            try
            {
                repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());

                Rpt_EndTime.Text = reader.GetString("Rpt_EndTime") + ":";
                Rpt_StartTime.Text = reader.GetString("Rpt_StartTime") + ":";
                cell_Rpt_EndTime.Text = repository.WhereInfo.EndTime.ToString(repository.DateFormat);
                cell_Rpt_StartTime.Text = repository.WhereInfo.BeginTime.ToString(repository.DateFormat);

                xrLTime.Text = string.Format("{0}: {1}", reader.GetString("Rpt_Time"), repository.WhereInfo.LocalTime.ToString(repository.DateFormat + " HH:mm:ss"));

                this.DisplayName = rpTitle.Text = reader.GetString("Rpt_Internal_Access_Title");
                rpTitle.Font = new System.Drawing.Font("Segoe UI", 14, FontStyle.Bold);

                tDate.Text = reader.GetString("Rpt_Internal_Access_COLDATE");
                tYHDLCS.Text = reader.GetString("Rpt_Internal_Access_COLDLCS");
                tAvgTime.Text = reader.GetString("Rpt_Internal_Access_COLAVGTIME");
                tYJBJCLCS.Text = reader.GetString("Rpt_Internal_Access_COLYJBJCLS");

                colDate.DataBindings.Add("Text", DataSource, "COLDATE");
                colDLCS.DataBindings.Add("Text", DataSource, "COLDLCS");
                colYJBJCLS.DataBindings.Add("Text", DataSource, "COLYJBJCLS");
                colAvgTime.DataBindings.Add("Text", DataSource, "COLAVGTIMETEMP");

                DataTable dt = repository.GetDataForInternal_AccessRpt();

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.Logger.Error("Internal_AccessRpt_DataSourceDemanded", ex);

            }
        }

        private void colDate_EvaluateBinding(object sender, BindingEventArgs e)
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
                LoggerManager.Logger.Error("colDate_EvaluateBinding", ex);
            }
        }

    }
}

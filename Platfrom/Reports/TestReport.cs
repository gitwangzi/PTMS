using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using System.Configuration;
using Gsafety.Common.Logging;
using System.Reflection;
using System.Linq;
namespace Gsafety.PTMS.Reports
{
    public partial class TestReport : DevExpress.XtraReports.UI.XtraReport
    {
        StringResourceReader reader = new StringResourceReader();
        ReportStatisticsRepository repository = null;
        const string DefaultLanguage = "zh-CN";
        public TestReport()
        {
            this.ForeColor = Color.FromArgb(110, 110, 110);
            //xrControlStyle1.BackColor = Color.FromArgb(233, 238, 244);
            //xrChart1.Titles[0].TextColor = Color.FromArgb(110, 110, 110);
            InitializeComponent();
            Console.WriteLine("XtraReport1--------" + this.xrtTitle.Font.ToString());



            //try
            //{
            //    string language = "";
            //    if (ConfigurationManager.AppSettings.AllKeys.Contains("CultureInfo"))
            //    {
            //        language = ConfigurationManager.AppSettings["CultureInfo"].ToString();
            //    }
            //    else
            //    {
            //        language = "zh-CN";
            //    }
            //    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            //    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            //}
            //catch (Exception ex)
            //{
            //    LoggerManager.Logger.Error(MethodInfo.GetCurrentMethod(), ex);
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(DefaultLanguage);
            //    //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DefaultLanguage);
            //}

        }

        private void XtraReport1_DataSourceDemanded(object sender, EventArgs e)
        {
            repository = new ReportStatisticsRepository(this.Parameters["whereInfo"].Value.ToString());
            this.DisplayName = xrtTitle.Text = reader.GetString("Rpt_Alarm_Title");
            Console.WriteLine("XtraReport1_DataSourceDemanded--------" + this.xrtTitle.Font.ToString());

        }

        private void XtraReport1_AfterPrint(object sender, EventArgs e)
        {
            Console.WriteLine("XtraReport1_AfterPrint--------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_BandHeightChanged(object sender, BandEventArgs e)
        {
            Console.WriteLine("XtraReport1_BandHeightChanged---------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Console.WriteLine("XtraReport1_BeforePrint-------" + this.xrtTitle.Font.ToString());

        }

        private void XtraReport1_DesignerLoaded(object sender, DevExpress.XtraReports.UserDesigner.DesignerLoadedEventArgs e)
        {
            Console.WriteLine("XtraReport1_DesignerLoaded----------" + this.xrtTitle.Font.ToString());

        }

        private void XtraReport1_DataSourceRowChanged(object sender, DataSourceRowEventArgs e)
        {

            Console.WriteLine("XtraReport1_DataSourceRowChanged-------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_FillEmptySpace(object sender, BandEventArgs e)
        {
            Console.WriteLine("XtraReport1_FillEmptySpace-------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_ParametersRequestBeforeShow(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            Console.WriteLine("XtraReport1_ParametersRequestBeforeShow-------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
            Console.WriteLine("XtraReport1_ParametersRequestSubmit-------" + this.xrtTitle.Font.ToString());

        }

        private void XtraReport1_ParametersRequestValueChanged(object sender, DevExpress.XtraReports.Parameters.ParametersRequestValueChangedEventArgs e)
        {
            Console.WriteLine("XtraReport1_ParametersRequestValueChanged-------" + this.xrtTitle.Font.ToString());

        }

        private void XtraReport1_PrintProgress(object sender, DevExpress.XtraPrinting.PrintProgressEventArgs e)
        {
            Console.WriteLine("XtraReport1_ParametersRequestValueChanged-------" + this.xrtTitle.Font.ToString());
        }

        private void XtraReport1_SaveComponents(object sender, SaveComponentsEventArgs e)
        {
            Console.WriteLine("XtraReport1_SaveComponents-------" + this.xrtTitle.Font.ToString());
        }

        private void xrtTitle_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            Console.WriteLine("XtraReport1_SaveComponents-------" + this.xrtTitle.Font.ToString());
        }

    }
}

namespace Gsafety.PTMS.Reports
{
    partial class TestReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrtTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrtTitle
            // 
            this.xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.xrtTitle.ForeColor = System.Drawing.Color.Black;
            this.xrtTitle.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.xrtTitle.Name = "xrtTitle";
            this.xrtTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrtTitle.SizeF = new System.Drawing.SizeF(640F, 40.625F);
            this.xrtTitle.StylePriority.UseForeColor = false;
            this.xrtTitle.Text = "一键报警统计报表";
            this.xrtTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrtTitle.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrtTitle_PrintOnPage);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrtTitle});
            this.ReportHeader.Name = "ReportHeader";
            // 
            // whereInfo
            // 
            this.whereInfo.Name = "whereInfo";
            this.whereInfo.Visible = false;
            // 
            // XtraReport1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.ForeColor = System.Drawing.Color.OrangeRed;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.whereInfo});
            this.Version = "13.2";
            this.SaveComponents += new System.EventHandler<DevExpress.XtraReports.UI.SaveComponentsEventArgs>(this.XtraReport1_SaveComponents);
            this.PrintProgress += new DevExpress.XtraPrinting.PrintProgressEventHandler(this.XtraReport1_PrintProgress);
            this.FillEmptySpace += new DevExpress.XtraReports.UI.BandEventHandler(this.XtraReport1_FillEmptySpace);
            this.DesignerLoaded += new DevExpress.XtraReports.UserDesigner.DesignerLoadedEventHandler(this.XtraReport1_DesignerLoaded);
            this.ParametersRequestBeforeShow += new System.EventHandler<DevExpress.XtraReports.Parameters.ParametersRequestEventArgs>(this.XtraReport1_ParametersRequestBeforeShow);
            this.ParametersRequestValueChanged += new System.EventHandler<DevExpress.XtraReports.Parameters.ParametersRequestValueChangedEventArgs>(this.XtraReport1_ParametersRequestValueChanged);
            this.ParametersRequestSubmit += new System.EventHandler<DevExpress.XtraReports.Parameters.ParametersRequestEventArgs>(this.XtraReport1_ParametersRequestSubmit);
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.XtraReport1_DataSourceDemanded);
            this.BandHeightChanged += new DevExpress.XtraReports.UI.BandEventHandler(this.XtraReport1_BandHeightChanged);
            this.DataSourceRowChanged += new DevExpress.XtraReports.UI.DataSourceRowEventHandler(this.XtraReport1_DataSourceRowChanged);
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.XtraReport1_BeforePrint);
            this.AfterPrint += new System.EventHandler(this.XtraReport1_AfterPrint);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrtTitle;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.Parameters.Parameter whereInfo;
    }
}

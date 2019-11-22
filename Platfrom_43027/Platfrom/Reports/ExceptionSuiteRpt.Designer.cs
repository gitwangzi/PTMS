namespace Gsafety.PTMS.Reports
{
    partial class ExceptionSuiteRpt
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
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colCPHM = new DevExpress.XtraReports.UI.XRTableCell();
            this.colSN = new DevExpress.XtraReports.UI.XRTableCell();
            this.colYCYY = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.rpTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tCPHM = new DevExpress.XtraReports.UI.XRTableCell();
            this.tSN = new DevExpress.XtraReports.UI.XRTableCell();
            this.tYCYY = new DevExpress.XtraReports.UI.XRTableCell();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrEventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 30F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.EvenStyleName = "xrEventStyle";
            this.xrTable2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(798F, 30F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colCPHM,
            this.colSN,
            this.colYCYY});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // colCPHM
            // 
            this.colCPHM.Name = "colCPHM";
            this.colCPHM.Text = "colCPHM";
            this.colCPHM.Weight = 0.64039419089632088D;
            // 
            // colSN
            // 
            this.colSN.Name = "colSN";
            this.colSN.Text = "colSN";
            this.colSN.Weight = 0.78817733388816191D;
            // 
            // colYCYY
            // 
            this.colYCYY.Name = "colYCYY";
            this.colYCYY.Text = "colYCYY";
            this.colYCYY.Weight = 2.3452215786637929D;
            this.colYCYY.EvaluateBinding += new DevExpress.XtraReports.UI.BindingEventHandler(this.colYCYY_EvaluateBinding);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // rpTitle
            // 
            this.rpTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.rpTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.rpTitle.Name = "rpTitle";
            this.rpTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.rpTitle.SizeF = new System.Drawing.SizeF(798F, 40.62498F);
            this.rpTitle.StylePriority.UseFont = false;
            this.rpTitle.StylePriority.UseTextAlignment = false;
            this.rpTitle.Text = "异常套件原因统计";
            this.rpTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.xrLTime});
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(325.9165F, 35.45834F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(99.99998F, 25F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLTime
            // 
            this.xrLTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.xrLTime.LocationFloat = new DevExpress.Utils.PointFloat(425.9165F, 35.45834F);
            this.xrLTime.Name = "xrLTime";
            this.xrLTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLTime.SizeF = new System.Drawing.SizeF(372.0835F, 25F);
            this.xrLTime.StylePriority.UseFont = false;
            this.xrLTime.StylePriority.UseTextAlignment = false;
            this.xrLTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 112.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(798F, 30F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tCPHM,
            this.tSN,
            this.tYCYY});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tCPHM
            // 
            this.tCPHM.Name = "tCPHM";
            this.tCPHM.Text = "车牌号";
            this.tCPHM.Weight = 0.64039411873653029D;
            // 
            // tSN
            // 
            this.tSN.Name = "tSN";
            this.tSN.Text = "套件芯片号";
            this.tSN.Weight = 0.78817726172837121D;
            // 
            // tYCYY
            // 
            this.tYCYY.Name = "tYCYY";
            this.tYCYY.Text = "异常原因";
            this.tYCYY.Weight = 2.3452217229833741D;
            // 
            // whereInfo
            // 
            this.whereInfo.Name = "whereInfo";
            this.whereInfo.Visible = false;
            // 
            // xrEventStyle
            // 
            this.xrEventStyle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.xrEventStyle.Name = "xrEventStyle";
            this.xrEventStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.rpTitle,
            this.xrTable1});
            this.ReportHeader.HeightF = 142.5F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52.875F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4});
            this.xrTable3.SizeF = new System.Drawing.SizeF(798F, 49.20832F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.Rpt_StartTime,
            this.cell_Rpt_StartTime,
            this.Rpt_EndTime,
            this.cell_Rpt_EndTime});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // Rpt_StartTime
            // 
            this.Rpt_StartTime.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Rpt_StartTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Rpt_StartTime.Name = "Rpt_StartTime";
            this.Rpt_StartTime.StylePriority.UseBorders = false;
            this.Rpt_StartTime.StylePriority.UseFont = false;
            this.Rpt_StartTime.StylePriority.UseTextAlignment = false;
            this.Rpt_StartTime.Text = "Rpt_StartTime";
            this.Rpt_StartTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Rpt_StartTime.Weight = 0.60000018075801376D;
            // 
            // cell_Rpt_StartTime
            // 
            this.cell_Rpt_StartTime.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.cell_Rpt_StartTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cell_Rpt_StartTime.Name = "cell_Rpt_StartTime";
            this.cell_Rpt_StartTime.StylePriority.UseBorders = false;
            this.cell_Rpt_StartTime.StylePriority.UseFont = false;
            this.cell_Rpt_StartTime.StylePriority.UseTextAlignment = false;
            this.cell_Rpt_StartTime.Text = "cell_Rpt_StartTime";
            this.cell_Rpt_StartTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.cell_Rpt_StartTime.Weight = 0.91057608799656942D;
            // 
            // Rpt_EndTime
            // 
            this.Rpt_EndTime.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Rpt_EndTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.Rpt_EndTime.Name = "Rpt_EndTime";
            this.Rpt_EndTime.StylePriority.UseBorders = false;
            this.Rpt_EndTime.StylePriority.UseFont = false;
            this.Rpt_EndTime.StylePriority.UseTextAlignment = false;
            this.Rpt_EndTime.Text = "Rpt_EndTime";
            this.Rpt_EndTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Rpt_EndTime.Weight = 0.60000019249554692D;
            // 
            // cell_Rpt_EndTime
            // 
            this.cell_Rpt_EndTime.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.cell_Rpt_EndTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cell_Rpt_EndTime.Name = "cell_Rpt_EndTime";
            this.cell_Rpt_EndTime.StylePriority.UseBorders = false;
            this.cell_Rpt_EndTime.StylePriority.UseFont = false;
            this.cell_Rpt_EndTime.StylePriority.UseTextAlignment = false;
            this.cell_Rpt_EndTime.Text = "cell_Rpt_EndTime";
            this.cell_Rpt_EndTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.cell_Rpt_EndTime.Weight = 0.88942353874987012D;
            // 
            // ExceptionSuiteRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(25, 25, 50, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.whereInfo});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrEventStyle});
            this.Version = "13.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.ExceptionSuiteRpt_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel rpTitle;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tCPHM;
        private DevExpress.XtraReports.UI.XRTableCell tSN;
        private DevExpress.XtraReports.UI.XRTableCell tYCYY;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell colCPHM;
        private DevExpress.XtraReports.UI.XRTableCell colSN;
        private DevExpress.XtraReports.UI.XRTableCell colYCYY;
        private DevExpress.XtraReports.Parameters.Parameter whereInfo;
        private DevExpress.XtraReports.UI.XRControlStyle xrEventStyle;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLTime;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_EndTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_EndTime;
    }
}

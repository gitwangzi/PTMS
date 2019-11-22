namespace Gsafety.PTMS.Reports
{
    partial class Internal_AccessRpt
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
            this.colDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDLCS = new DevExpress.XtraReports.UI.XRTableCell();
            this.colAvgTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.colYJBJCLS = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.tYHDLCS = new DevExpress.XtraReports.UI.XRTableCell();
            this.tAvgTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.tYJBJCLCS = new DevExpress.XtraReports.UI.XRTableCell();
            this.rpTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrEventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
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
            this.xrTable2.SizeF = new System.Drawing.SizeF(650F, 30F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colDate,
            this.colDLCS,
            this.colAvgTime,
            this.colYJBJCLS});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // colDate
            // 
            this.colDate.Name = "colDate";
            this.colDate.Text = "colDate";
            this.colDate.Weight = 0.55288458017202524D;
            this.colDate.EvaluateBinding += new DevExpress.XtraReports.UI.BindingEventHandler(this.colDate_EvaluateBinding);
            // 
            // colDLCS
            // 
            this.colDLCS.Name = "colDLCS";
            this.colDLCS.Text = "colDLCS";
            this.colDLCS.Weight = 0.64423096876878017D;
            // 
            // colAvgTime
            // 
            this.colAvgTime.Name = "colAvgTime";
            this.colAvgTime.Text = "colAvgTime";
            this.colAvgTime.Weight = 0.65384605994591349D;
            // 
            // colYJBJCLS
            // 
            this.colYJBJCLS.Name = "colYJBJCLS";
            this.colYJBJCLS.Text = "colYJBJCLS";
            this.colYJBJCLS.Weight = 1.1490383911132813D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 50F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(250.0882F, 10F);
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
            this.xrLTime.LocationFloat = new DevExpress.Utils.PointFloat(350.0882F, 10F);
            this.xrLTime.Name = "xrLTime";
            this.xrLTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLTime.SizeF = new System.Drawing.SizeF(297.91F, 25F);
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
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 103.3333F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(650F, 30F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tDate,
            this.tYHDLCS,
            this.tAvgTime,
            this.tYJBJCLCS});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tDate
            // 
            this.tDate.Name = "tDate";
            this.tDate.Text = "时间";
            this.tDate.Weight = 0.55288458017202524D;
            // 
            // tYHDLCS
            // 
            this.tYHDLCS.Name = "tYHDLCS";
            this.tYHDLCS.Text = "用户登录次数";
            this.tYHDLCS.Weight = 0.64423082791841968D;
            // 
            // tAvgTime
            // 
            this.tAvgTime.Name = "tAvgTime";
            this.tAvgTime.Text = "平均在线时长";
            this.tAvgTime.Weight = 0.653846200796274D;
            // 
            // tYJBJCLCS
            // 
            this.tYJBJCLCS.Name = "tYJBJCLCS";
            this.tYJBJCLCS.Text = "一键报警";
            this.tYJBJCLCS.Weight = 1.1490383911132813D;
            // 
            // rpTitle
            // 
            this.rpTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.rpTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.rpTitle.Name = "rpTitle";
            this.rpTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.rpTitle.SizeF = new System.Drawing.SizeF(650F, 31.25F);
            this.rpTitle.StylePriority.UseFont = false;
            this.rpTitle.StylePriority.UseTextAlignment = false;
            this.rpTitle.Text = "内部用户访问情况";
            this.rpTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.xrTable4,
            this.rpTitle,
            this.xrTable1});
            this.ReportHeader.HeightF = 133.3333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 43.6042F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable4.SizeF = new System.Drawing.SizeF(650F, 49.20832F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.Rpt_StartTime,
            this.cell_Rpt_StartTime,
            this.Rpt_EndTime,
            this.cell_Rpt_EndTime});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
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
            // Internal_AccessRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 50, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.whereInfo});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrEventStyle});
            this.Version = "13.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.Internal_AccessRpt_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tDate;
        private DevExpress.XtraReports.UI.XRTableCell tYHDLCS;
        private DevExpress.XtraReports.UI.XRTableCell tAvgTime;
        private DevExpress.XtraReports.UI.XRTableCell tYJBJCLCS;
        private DevExpress.XtraReports.UI.XRLabel rpTitle;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell colDate;
        private DevExpress.XtraReports.UI.XRTableCell colDLCS;
        private DevExpress.XtraReports.UI.XRTableCell colAvgTime;
        private DevExpress.XtraReports.UI.XRTableCell colYJBJCLS;
        private DevExpress.XtraReports.Parameters.Parameter whereInfo;
        private DevExpress.XtraReports.UI.XRControlStyle xrEventStyle;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_EndTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_EndTime;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLTime;
    }
}

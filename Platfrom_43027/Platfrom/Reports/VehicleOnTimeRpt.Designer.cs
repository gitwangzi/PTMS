namespace Gsafety.PTMS.Reports
{
    partial class VehicleOnTimeRpt
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel1 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SideBySideBarSeriesLabel sideBySideBarSeriesLabel2 = new DevExpress.XtraCharts.SideBySideBarSeriesLabel();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.tabMain = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.colOnline = new DevExpress.XtraReports.UI.XRTableCell();
            this.colAvgTimespan = new DevExpress.XtraReports.UI.XRTableCell();
            this.celDISTANCE = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtOnLine = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtAvgTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.tDISTANCE = new DevExpress.XtraReports.UI.XRTableCell();
            this.rpTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrEventStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tabMain});
            this.Detail.HeightF = 30F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // tabMain
            // 
            this.tabMain.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tabMain.EvenStyleName = "xrEventStyle";
            this.tabMain.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tabMain.LocationFloat = new DevExpress.Utils.PointFloat(2.000122F, 0F);
            this.tabMain.Name = "tabMain";
            this.tabMain.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.tabMain.SizeF = new System.Drawing.SizeF(648F, 30F);
            this.tabMain.StylePriority.UseBorders = false;
            this.tabMain.StylePriority.UseFont = false;
            this.tabMain.StylePriority.UseTextAlignment = false;
            this.tabMain.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colTime,
            this.colOnline,
            this.colAvgTimespan,
            this.celDISTANCE});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // colTime
            // 
            this.colTime.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colTime.Name = "colTime";
            this.colTime.StylePriority.UseBorders = false;
            this.colTime.Text = "colTime";
            this.colTime.Weight = 0.84797403978917651D;
            this.colTime.EvaluateBinding += new DevExpress.XtraReports.UI.BindingEventHandler(this.colTime_EvaluateBinding);
            // 
            // colOnline
            // 
            this.colOnline.Name = "colOnline";
            this.colOnline.Text = "colOnline";
            this.colOnline.Weight = 2.0185230010536812D;
            // 
            // colAvgTimespan
            // 
            this.colAvgTimespan.Name = "colAvgTimespan";
            this.colAvgTimespan.Text = "colAvgTimespan";
            this.colAvgTimespan.Weight = 1.7824834545068911D;
            // 
            // celDISTANCE
            // 
            this.celDISTANCE.Name = "celDISTANCE";
            this.celDISTANCE.Text = "celDISTANCE";
            this.celDISTANCE.Weight = 1.7824834545068911D;
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
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(288.813F, 9.999974F);
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
            this.xrLTime.LocationFloat = new DevExpress.Utils.PointFloat(388.813F, 9.999974F);
            this.xrLTime.Name = "xrLTime";
            this.xrLTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLTime.SizeF = new System.Drawing.SizeF(261.1871F, 25F);
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
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(2.000125F, 111.4583F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(648.0001F, 30F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtTime,
            this.xrtOnLine,
            this.xrtAvgTime,
            this.tDISTANCE});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrtTime
            // 
            this.xrtTime.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtTime.Name = "xrtTime";
            this.xrtTime.StylePriority.UseBorders = false;
            this.xrtTime.Text = "时间";
            this.xrtTime.Weight = 0.84797403978917651D;
            // 
            // xrtOnLine
            // 
            this.xrtOnLine.Name = "xrtOnLine";
            this.xrtOnLine.Text = "上线数";
            this.xrtOnLine.Weight = 2.0185230010536812D;
            // 
            // xrtAvgTime
            // 
            this.xrtAvgTime.Name = "xrtAvgTime";
            this.xrtAvgTime.Text = "平均运行时长(H)";
            this.xrtAvgTime.Weight = 1.7824834545068911D;
            // 
            // tDISTANCE
            // 
            this.tDISTANCE.Name = "tDISTANCE";
            this.tDISTANCE.Text = "行驶距离(M)";
            this.tDISTANCE.Weight = 1.7824834545068911D;
            // 
            // rpTitle
            // 
            this.rpTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.rpTitle.LocationFloat = new DevExpress.Utils.PointFloat(2.000122F, 0F);
            this.rpTitle.Name = "rpTitle";
            this.rpTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.rpTitle.SizeF = new System.Drawing.SizeF(648.0001F, 37.99999F);
            this.rpTitle.StylePriority.UseFont = false;
            this.rpTitle.StylePriority.UseTextAlignment = false;
            this.rpTitle.Text = "车辆在线时长统计";
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
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChart1});
            this.GroupFooter1.HeightF = 347.9167F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrChart1
            // 
            this.xrChart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xrChart1.BorderColor = System.Drawing.Color.Black;
            this.xrChart1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            xyDiagram1.AxisX.Label.Font = new System.Drawing.Font("Segoe UI", 8F);
            xyDiagram1.AxisX.Label.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.Label.Font = new System.Drawing.Font("Segoe UI", 8F);
            xyDiagram1.AxisY.Label.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.DefaultPane.EnableAxisXScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisXZooming = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYScrolling = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.DefaultPane.EnableAxisYZooming = DevExpress.Utils.DefaultBoolean.False;
            this.xrChart1.Diagram = xyDiagram1;
            this.xrChart1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.xrChart1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.xrChart1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.xrChart1.Legend.EquallySpacedItems = false;
            this.xrChart1.Legend.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.xrChart1.Legend.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrChart1.Name = "xrChart1";
            series1.ArgumentDataMember = "COLTIME";
            sideBySideBarSeriesLabel1.Font = new System.Drawing.Font("Segoe UI", 8F);
            sideBySideBarSeriesLabel1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            series1.Label = sideBySideBarSeriesLabel1;
            series1.Name = "Series 1";
            series1.ValueDataMembersSerializable = "COLONLINECOUNT";
            series2.ArgumentDataMember = "COLTIME";
            sideBySideBarSeriesLabel2.Font = new System.Drawing.Font("Segoe UI", 8F);
            sideBySideBarSeriesLabel2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            series2.Label = sideBySideBarSeriesLabel2;
            series2.Name = "Series 2";
            series2.ValueDataMembersSerializable = "COLAVGTIMESPAN";
            series3.ArgumentDataMember = "COLTIME";
            series3.Name = "Series 3";
            series3.ValueDataMembersSerializable = "DISTANCE";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
            this.xrChart1.SizeF = new System.Drawing.SizeF(650.0001F, 347.9167F);
            chartTitle1.Antialiasing = false;
            chartTitle1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            chartTitle1.Text = "车辆在线时长统计";
            chartTitle1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(110)))), ((int)(((byte)(110)))));
            this.xrChart1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.rpTitle,
            this.xrTable1});
            this.ReportHeader.HeightF = 141.4583F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(2.000122F, 51.04167F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable4.SizeF = new System.Drawing.SizeF(648.0001F, 49.20832F);
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
            // VehicleOnTimeRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupFooter1,
            this.ReportHeader});
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 50, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.whereInfo});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrEventStyle});
            this.Version = "13.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.VehicleOnTimeRpt_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(sideBySideBarSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrtTime;
        private DevExpress.XtraReports.UI.XRTableCell xrtAvgTime;
        private DevExpress.XtraReports.UI.XRLabel rpTitle;
        private DevExpress.XtraReports.UI.XRTable tabMain;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell colTime;
        private DevExpress.XtraReports.UI.XRTableCell colAvgTimespan;
        private DevExpress.XtraReports.Parameters.Parameter whereInfo;
        private DevExpress.XtraReports.UI.XRControlStyle xrEventStyle;
        private DevExpress.XtraReports.UI.XRTableCell colOnline;
        private DevExpress.XtraReports.UI.XRTableCell xrtOnLine;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRChart xrChart1;
        private DevExpress.XtraReports.UI.XRTableCell celDISTANCE;
        private DevExpress.XtraReports.UI.XRTableCell tDISTANCE;
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

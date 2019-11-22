namespace Gsafety.PTMS.Reports
{
    partial class AlarmStatisticsReport
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
            xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colGroup = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTrue = new DevExpress.XtraReports.UI.XRTableCell();
            this.colFalse = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTransfer = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLTime = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_StartTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.cell_Rpt_EndTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtGroup = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtHandled = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrtTrue = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtFalse = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTransfer = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrChart1 = new DevExpress.XtraReports.UI.XRChart();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 30F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.EvenStyleName = "xrControlStyle1";
            this.xrTable1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(2.000046F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(724.9999F, 30F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colGroup,
            this.colTotal,
            this.colTrue,
            this.colFalse,
            this.colTransfer});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // colGroup
            // 
            this.colGroup.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colGroup.Name = "colGroup";
            this.colGroup.StylePriority.UseBorders = false;
            this.colGroup.Text = "colGroup";
            this.colGroup.Weight = 1.550819827599065D;
            // 
            // colTotal
            // 
            this.colTotal.Name = "colTotal";
            this.colTotal.Text = "colTotal";
            this.colTotal.Weight = 0.69496461105819551D;
            // 
            // colTrue
            // 
            this.colTrue.Name = "colTrue";
            this.colTrue.Text = "colTrue";
            this.colTrue.Weight = 1.5429245604592903D;
            // 
            // colFalse
            // 
            this.colFalse.Name = "colFalse";
            this.colFalse.Text = "colFalse";
            this.colFalse.Weight = 1.1830579980246763D;
            // 
            // colTransfer
            // 
            this.colTransfer.Name = "colTransfer";
            this.colTransfer.Text = "colTransfer";
            this.colTransfer.Weight = 1.459696681994419D;
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
            this.BottomMargin.HeightF = 42.70833F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(329.09F, 9.999974F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(99.99998F, 25F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLTime
            // 
            this.xrLTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLTime.LocationFloat = new DevExpress.Utils.PointFloat(429.0899F, 9.999974F);
            this.xrLTime.Name = "xrLTime";
            this.xrLTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLTime.SizeF = new System.Drawing.SizeF(297.91F, 25F);
            this.xrLTime.StylePriority.UseFont = false;
            this.xrLTime.StylePriority.UseTextAlignment = false;
            this.xrLTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrtTitle,
            this.xrTable2});
            this.ReportHeader.HeightF = 175.625F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(2.000046F, 52.87501F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable4.SizeF = new System.Drawing.SizeF(724.9999F, 49.20832F);
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
            // xrtTitle
            // 
            this.xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.xrtTitle.LocationFloat = new DevExpress.Utils.PointFloat(2.000046F, 0F);
            this.xrtTitle.Name = "xrtTitle";
            this.xrtTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrtTitle.SizeF = new System.Drawing.SizeF(725.0001F, 40.625F);
            this.xrtTitle.StylePriority.UseFont = false;
            this.xrtTitle.StylePriority.UseTextAlignment = false;
            this.xrtTitle.Text = "报警统计报表";
            this.xrtTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable2
            // 
            this.xrTable2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
            | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(2.000292F, 115.625F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(724.9999F, 60F);
            this.xrTable2.StylePriority.UseBackColor = false;
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtGroup,
            this.xrtTotal,
            this.xrTableCell3,
            this.xrtTransfer});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrtGroup
            // 
            this.xrtGroup.Name = "xrtGroup";
            this.xrtGroup.StylePriority.UseBorders = false;
            this.xrtGroup.Text = "分类";
            this.xrtGroup.Weight = 0.733506816159857D;
            // 
            // xrtTotal
            // 
            this.xrtTotal.Name = "xrtTotal";
            this.xrtTotal.Text = "报警总数";
            this.xrtTotal.Weight = 0.32870499073181014D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "转警";
            this.xrTableCell3.Weight = 1.2893365622357946D;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(9.131432E-05F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrTable3.SizeF = new System.Drawing.SizeF(307.2918F, 60F);
            this.xrTable3.StylePriority.UseBorders = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtHandled});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrtHandled
            // 
            this.xrtHandled.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtHandled.Name = "xrtHandled";
            this.xrtHandled.StylePriority.UseBorders = false;
            this.xrtHandled.Text = "已处理";
            this.xrtHandled.Weight = 1.5029532018261069D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtTrue,
            this.xrtFalse});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrtTrue
            // 
            this.xrtTrue.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtTrue.Name = "xrtTrue";
            this.xrtTrue.StylePriority.UseBorders = false;
            this.xrtTrue.Text = "真警";
            this.xrtTrue.Weight = 1.0904163080980105D;
            // 
            // xrtFalse
            // 
            this.xrtFalse.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrtFalse.Name = "xrtFalse";
            this.xrtFalse.StylePriority.UseBorders = false;
            this.xrtFalse.Text = "假警";
            this.xrtFalse.Weight = 0.836091527214999D;
            // 
            // xrtTransfer
            // 
            this.xrtTransfer.Name = "xrtTransfer";
            this.xrtTransfer.Text = "转警";
            this.xrtTransfer.Weight = 0.69040967641228046D;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // whereInfo
            // 
            this.whereInfo.Name = "whereInfo";
            this.whereInfo.Visible = false;
            // 
            // xrControlStyle2
            // 
            this.xrControlStyle2.Name = "xrControlStyle2";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrChart1});
            this.GroupFooter1.HeightF = 350F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrChart1
            // 
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
            xyDiagram1.Rotated = true;
            this.xrChart1.Diagram = xyDiagram1;
            this.xrChart1.FillStyle.FillMode = DevExpress.XtraCharts.FillMode.Hatch;
            this.xrChart1.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.Center;
            this.xrChart1.Legend.AlignmentVertical = DevExpress.XtraCharts.LegendAlignmentVertical.BottomOutside;
            this.xrChart1.Legend.Direction = DevExpress.XtraCharts.LegendDirection.LeftToRight;
            this.xrChart1.Legend.EquallySpacedItems = false;
            this.xrChart1.Legend.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.xrChart1.LocationFloat = new DevExpress.Utils.PointFloat(2.000046F, 0F);
            this.xrChart1.Name = "xrChart1";
            series1.ArgumentDataMember = "PROJECT";
            series1.Name = "TotalAlarm";
            series1.ValueDataMembersSerializable = "ALARMNUM";
            series2.ArgumentDataMember = "PROJECT";
            series2.Name = "TrueAlarm";
            series2.ValueDataMembersSerializable = "TRUEALARM";
            series3.ArgumentDataMember = "PROJECT";
            series3.Name = "FalseAlarm";
            series3.ValueDataMembersSerializable = "FalseAlarm";
            series4.ArgumentDataMember = "PROJECT";
            series4.Name = "TransferAlarm";
            series4.ValueDataMembersSerializable = "IS_TRANSFER";
            this.xrChart1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3,
        series4};
            this.xrChart1.SizeF = new System.Drawing.SizeF(725.0001F, 350F);
            chartTitle1.Antialiasing = false;
            chartTitle1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            chartTitle1.Text = "报警统计报表";
            this.xrChart1.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // AlarmStatisticsReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupFooter1});
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(50, 50, 50, 43);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.whereInfo});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
            this.Version = "13.2";
            this.DataSourceDemanded += new System.EventHandler<System.EventArgs>(this.AlarmReport_DataSourceDemanded);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrChart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell colGroup;
        private DevExpress.XtraReports.UI.XRTableCell colTotal;
        private DevExpress.XtraReports.UI.XRTableCell colTrue;
        private DevExpress.XtraReports.UI.XRTableCell colFalse;
        private DevExpress.XtraReports.UI.XRTableCell colTransfer;
        private DevExpress.XtraReports.UI.XRLabel xrtTitle;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtGroup;
        private DevExpress.XtraReports.UI.XRTableCell xrtTotal;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle1;
        private DevExpress.XtraReports.Parameters.Parameter whereInfo;
        private DevExpress.XtraReports.UI.XRControlStyle xrControlStyle2;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_StartTime;
        private DevExpress.XtraReports.UI.XRTableCell Rpt_EndTime;
        private DevExpress.XtraReports.UI.XRTableCell cell_Rpt_EndTime;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo1;
        private DevExpress.XtraReports.UI.XRLabel xrLTime;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrtHandled;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrtTrue;
        private DevExpress.XtraReports.UI.XRTableCell xrtFalse;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTableCell xrtTransfer;
        private DevExpress.XtraReports.UI.XRChart xrChart1;
        private DevExpress.XtraCharts.XYDiagram xyDiagram1;
        DevExpress.XtraCharts.ChartTitle chartTitle1;
    }
}

namespace Gsafety.PTMS.Reports
{
    partial class VehicleHistoryReport
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.colVehicle = new DevExpress.XtraReports.UI.XRTableCell();
            this.colOrganization = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDistrict = new DevExpress.XtraReports.UI.XRTableCell();
            this.colVehicleType = new DevExpress.XtraReports.UI.XRTableCell();
            this.colLon = new DevExpress.XtraReports.UI.XRTableCell();
            this.colLat = new DevExpress.XtraReports.UI.XRTableCell();
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
            this.xrtVehicle = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtOrganization = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDistrict = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtVehicleType = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtLon = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtLat = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.whereInfo = new DevExpress.XtraReports.Parameters.Parameter();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrtSpeed = new DevExpress.XtraReports.UI.XRTableCell();
            this.colSpeed = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtDir = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.colDir = new DevExpress.XtraReports.UI.XRTableCell();
            this.colTime = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
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
            this.xrTable1.SizeF = new System.Drawing.SizeF(897.0001F, 30F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.colVehicle,
            this.colOrganization,
            this.colDistrict,
            this.colVehicleType,
            this.colLon,
            this.colLat,
            this.colSpeed,
            this.colDir,
            this.colTime});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // colVehicle
            // 
            this.colVehicle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.colVehicle.Name = "colVehicle";
            this.colVehicle.StylePriority.UseBorders = false;
            this.colVehicle.Text = "colVehicle";
            this.colVehicle.Weight = 0.83005222495893027D;
            this.colVehicle.EvaluateBinding += new DevExpress.XtraReports.UI.BindingEventHandler(this.colCreateTime_EvaluateBinding);
            // 
            // colOrganization
            // 
            this.colOrganization.Name = "colOrganization";
            this.colOrganization.Text = "colOrganization";
            this.colOrganization.Weight = 0.96294260275676979D;
            // 
            // colDistrict
            // 
            this.colDistrict.Name = "colDistrict";
            this.colDistrict.Text = "colDistrict";
            this.colDistrict.Weight = 0.86325098503176778D;
            // 
            // colVehicleType
            // 
            this.colVehicleType.Name = "colVehicleType";
            this.colVehicleType.Text = "colVehicleType";
            this.colVehicleType.Weight = 0.87046552316970516D;
            // 
            // colLon
            // 
            this.colLon.Multiline = true;
            this.colLon.Name = "colLon";
            this.colLon.Text = "colLon\r\n";
            this.colLon.Weight = 0.99798592272799425D;
            // 
            // colLat
            // 
            this.colLat.Name = "colLat";
            this.colLat.Text = "colLat";
            this.colLat.Weight = 0.95338457444664848D;
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
            this.BottomMargin.HeightF = 43F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(501.09F, 9.999974F);
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
            this.xrLTime.LocationFloat = new DevExpress.Utils.PointFloat(601.09F, 9.999974F);
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
            this.ReportHeader.HeightF = 145.625F;
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
            this.xrTable4.SizeF = new System.Drawing.SizeF(896.9999F, 49.20832F);
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
            this.cell_Rpt_StartTime.Weight = 1.2554036480766408D;
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
            this.Rpt_EndTime.Weight = 0.62358913731790433D;
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
            this.cell_Rpt_EndTime.Weight = 1.2327315441728235D;
            // 
            // xrtTitle
            // 
            this.xrtTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.xrtTitle.LocationFloat = new DevExpress.Utils.PointFloat(2.000046F, 0F);
            this.xrtTitle.Name = "xrtTitle";
            this.xrtTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrtTitle.SizeF = new System.Drawing.SizeF(897F, 40.625F);
            this.xrtTitle.StylePriority.UseFont = false;
            this.xrtTitle.StylePriority.UseTextAlignment = false;
            this.xrtTitle.Text = "车辆历史轨迹";
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
            this.xrTable2.SizeF = new System.Drawing.SizeF(896.9999F, 30F);
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
            this.xrtVehicle,
            this.xrtOrganization,
            this.xrtDistrict,
            this.xrtVehicleType,
            this.xrtLon,
            this.xrtLat,
            this.xrtSpeed,
            this.xrtDir,
            this.xrtTime});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 0.5D;
            // 
            // xrtVehicle
            // 
            this.xrtVehicle.Name = "xrtVehicle";
            this.xrtVehicle.StylePriority.UseBorders = false;
            this.xrtVehicle.Text = "车牌号";
            this.xrtVehicle.Weight = 0.3925976674495254D;
            // 
            // xrtOrganization
            // 
            this.xrtOrganization.Name = "xrtOrganization";
            this.xrtOrganization.Text = "组织机构";
            this.xrtOrganization.Weight = 0.45545321061784844D;
            // 
            // xrtDistrict
            // 
            this.xrtDistrict.Name = "xrtDistrict";
            this.xrtDistrict.Text = "地区";
            this.xrtDistrict.Weight = 0.4083014337360435D;
            // 
            // xrtVehicleType
            // 
            this.xrtVehicleType.Name = "xrtVehicleType";
            this.xrtVehicleType.Text = "车辆类别";
            this.xrtVehicleType.Weight = 0.41171309328351013D;
            // 
            // xrtLon
            // 
            this.xrtLon.Name = "xrtLon";
            this.xrtLon.Text = "经度";
            this.xrtLon.Weight = 0.4720280644940435D;
            // 
            // xrtLat
            // 
            this.xrtLat.Name = "xrtLat";
            this.xrtLat.Text = "纬度";
            this.xrtLat.Weight = 0.45093221296264985D;
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
            // xrtSpeed
            // 
            this.xrtSpeed.Name = "xrtSpeed";
            this.xrtSpeed.Text = "速度";
            this.xrtSpeed.Weight = 0.30413731656542708D;
            // 
            // colSpeed
            // 
            this.colSpeed.Name = "colSpeed";
            this.colSpeed.Text = "colSpeed";
            this.colSpeed.Weight = 0.64302245208133169D;
            // 
            // xrtDir
            // 
            this.xrtDir.Name = "xrtDir";
            this.xrtDir.Text = "方向";
            this.xrtDir.Weight = 0.28403220927065514D;
            // 
            // xrtTime
            // 
            this.xrtTime.Name = "xrtTime";
            this.xrtTime.Text = "时间";
            this.xrtTime.Weight = 0.5844410955715158D;
            // 
            // colDir
            // 
            this.colDir.Name = "colDir";
            this.colDir.Text = "colDir";
            this.colDir.Weight = 0.60051546745487849D;
            // 
            // colTime
            // 
            this.colTime.Name = "colTime";
            this.colTime.Text = "colTime";
            this.colTime.Weight = 1.2356552709396851D;
            // 
            // VechileHistoryReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(16, 14, 50, 43);
            this.PageHeight = 1268;
            this.PageWidth = 929;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4Extra;
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
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell colVehicle;
        private DevExpress.XtraReports.UI.XRTableCell colOrganization;
        private DevExpress.XtraReports.UI.XRTableCell colDistrict;
        private DevExpress.XtraReports.UI.XRTableCell colVehicleType;
        private DevExpress.XtraReports.UI.XRLabel xrtTitle;
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
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrtVehicle;
        private DevExpress.XtraReports.UI.XRTableCell xrtOrganization;
        private DevExpress.XtraReports.UI.XRTableCell xrtDistrict;
        private DevExpress.XtraReports.UI.XRTableCell xrtVehicleType;
        private DevExpress.XtraReports.UI.XRTableCell colLon;
        private DevExpress.XtraReports.UI.XRTableCell colLat;
        private DevExpress.XtraReports.UI.XRTableCell xrtLon;
        private DevExpress.XtraReports.UI.XRTableCell xrtLat;
        private DevExpress.XtraReports.UI.XRTableCell xrtSpeed;
        private DevExpress.XtraReports.UI.XRTableCell colSpeed;
        private DevExpress.XtraReports.UI.XRTableCell colDir;
        private DevExpress.XtraReports.UI.XRTableCell colTime;
        private DevExpress.XtraReports.UI.XRTableCell xrtDir;
        private DevExpress.XtraReports.UI.XRTableCell xrtTime;
    }
}

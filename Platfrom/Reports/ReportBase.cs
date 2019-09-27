﻿using Gsafety.Common.Localization;
using Gsafety.PTMS.Report.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Reports
{
    public class ReportBase : DevExpress.XtraReports.UI.XtraReport
    {
        protected StringResourceReader reader = null;
        protected ReportStatisticsRepository repository = null;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        public static string FontName = string.Empty;

        public ReportBase()
        {
            try
            {
                reader = new StringResourceReader();
                if (FontName == string.Empty)
                    FontName = System.Configuration.ConfigurationManager.AppSettings["FontName"].ToString();
            }
            catch (Exception)
            {
            }
        }

        protected virtual void ChangeFont()
        {

        }

        private void InitializeComponent()
        {
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Name = "detailBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // ReportBase
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.Version = "13.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}

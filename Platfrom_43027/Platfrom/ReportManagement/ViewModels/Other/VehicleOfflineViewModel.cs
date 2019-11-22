using BaseLib.Model;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Report.Repository;
using Gsafety.PTMS.ReportManager.Base;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.ReportManager.ViewModels
{
    [ExportAsViewModel(ReportName.VehicleOfflineVm)]
    public class VehicleOfflineViewModel : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Title"); } }

        private List<int> spans = new List<int>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<int> Spans
        {
            get { return spans; }
            set
            {
                spans = value;
                RaisePropertyChanged(() => Spans);
            }
        }

        private int span = 24;

        public int Span
        {
            get { return span; }
            set { span = value; }
        }

        public VehicleOfflineViewModel() :
            base("Gsafety.PTMS.Reports.VechileOffLineReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {
            Spans.Add(24);
            Spans.Add(48);
            Spans.Add(72);

            RaisePropertyChanged(() => Spans);
            Span = Spans[0];
            RaisePropertyChanged(() => Span);
        }

        protected override bool Validate()
        {
            return true;
        }

        protected override ReportWhereInfo GenerateParameter()
        {
            ReportWhereInfo whereinfo = base.GenerateParameter();
            whereinfo.BeginTime = BeginTime.Value.AddDays(1).ToUniversalTime();
            whereinfo.EndTime = whereinfo.BeginTime.AddHours(-span);
            return whereinfo;
        }
    }
}

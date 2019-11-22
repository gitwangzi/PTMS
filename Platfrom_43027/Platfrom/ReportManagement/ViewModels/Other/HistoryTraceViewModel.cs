using BaseLib.Model;
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
    [ExportAsViewModel(ReportName.HistoryTraceVm)]
    public class HistoryTraceViewModel : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("HistoryRote"); } }

        public HistoryTraceViewModel() :
            base("Gsafety.PTMS.Reports.VehicleHistoryReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {

        }
    }
}

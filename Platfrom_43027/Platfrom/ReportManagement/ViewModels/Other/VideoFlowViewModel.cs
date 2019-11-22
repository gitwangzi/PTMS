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
    [ExportAsViewModel(ReportName.VideoFlowVm)]
    public class VideoFlowViewModel : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Title"); } }

        public VideoFlowViewModel() :
            base("Gsafety.PTMS.Reports.VideoFlowReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {

        }
    }
}

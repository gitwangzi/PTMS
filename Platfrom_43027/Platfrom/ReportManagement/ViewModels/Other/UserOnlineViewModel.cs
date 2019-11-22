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
    [ExportAsViewModel(ReportName.UserOnlineVm)]
    public class UserOnlineViewModel : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Title"); } }

        public UserOnlineViewModel() :
            base("Gsafety.PTMS.Reports.UserOnlineReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {

        }

        protected override bool Validate()
        {
            if (BeginTime.Value > EndTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Report"), ApplicationContext.Instance.StringResourceReader.GetString("VDM_TimeSettingError"), MessageDialogButton.Ok);
                return false;
            }
            if (EndTime > DateTime.Now)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_Report"), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"), MessageDialogButton.Ok);
                return false;
            }

            return true;
        }
    }
}

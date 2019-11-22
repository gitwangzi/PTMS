using BaseLib.Model;
using Gsafety.PTMS.Report.Repository;
using Gsafety.PTMS.ReportManager.Base;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Reflection;

namespace Gsafety.PTMS.ReportManager.ViewModels.Alarm
{

    public class AlarmBaseViewModel : BaseReportViewModel
    {
        public string ReportTitle { get { return ApplicationContext.Instance.StringResourceReader.GetString("Rpt_Alarm_Title"); } }

        private List<NameValueModel<int>> alarmsources = new List<NameValueModel<int>>();
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<NameValueModel<int>> AlarmSources
        {
            get { return alarmsources; }
            set
            {
                alarmsources = value;
                RaisePropertyChanged(() => AlarmSources);
            }
        }

        private NameValueModel<int> alarmsource = null;

        public NameValueModel<int> AlarmSource
        {
            get { return alarmsource; }
            set { alarmsource = value; }
        }

        public AlarmBaseViewModel() :
            base("Gsafety.PTMS.Reports.AlarmStatisticsReport, Reports, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")
        {
            try
            {
                alarmsources.Add(new NameValueModel<int>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), Value = -1 });
                alarmsources.Add(new NameValueModel<int>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("SecuritySuite"), Value = 0 });
               // alarmsources.Add(new NameValueModel<int>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("CellPhone"), Value = 1 });
                alarmsources.Add(new NameValueModel<int>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("ManualAlert"), Value = 2 });

                RaisePropertyChanged(() => AlarmSources);
                AlarmSource = AlarmSources[0];
                RaisePropertyChanged(() => AlarmSource);
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override Report.Repository.ReportWhereInfo GenerateParameter()
        {
            ReportWhereInfo whereInfo = base.GenerateParameter();

            if(AlarmSource.Value == -1)
            {
                whereInfo.AlarmSource = null;
            }
            else
            {
                whereInfo.AlarmSource = AlarmSource.Value;
            }

            return whereInfo;
        }
    }
}

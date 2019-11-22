using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
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

namespace Gsafety.PTMS.Maintain.ViewModels
{

    [ExportAsViewModel(MaintainName.MaintenanceScrapVm)]
    public class MaintenanceScrapVm : BaseEntityViewModel
    {
        private MaintenanceRecordServiceClient client;
        public ICommand ReturnCommand { get; private set; }

        public PagedServerCollection<MaintainanceDetail> PSC_MaintainanceDetail { get; set; }

        public SuiteMaintenance CurrentMaintainingRecord { get; private set; }

        public string MaintainTime { get; set; }

        public string SuiteId { get; set; }

        public string Maintainer { get; set; }

        public string Note { get; set; }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {

            base.ActivateView(viewName, viewParameters);
            object value = "";
            if (viewParameters.TryGetValue("SuiteId", out value))
            {
                SuiteId = viewParameters["SuiteId"].ToString();
                //Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged("SuiteID"));
                client.GetMaintenanceNoteRecordsAsync(SuiteId, RepairType.Scrap, null, null, null);
            }

        }
        public MaintenanceScrapVm()
        {
            try
            {
                client = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();

                client.GetMaintenanceNoteRecordsCompleted += client_GetMaintenanceNoteRecordsCompleted;
            }
            catch
            {

            }
        }


        void client_GetMaintenanceNoteRecordsCompleted(object sender, GetMaintenanceNoteRecordsCompletedEventArgs e)
        {

            try
            {
                MaintainanceDetail item = e.Result.Result[0];
                MaintainTime = item.MaintainTime.ToString();
                Maintainer = item.Maintainer;
                //SuiteId = item.SuiteId;
                Note = item.Note;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MaintainTime));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Maintainer));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteId));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));

                if (e.Result.TotalRecord == 0)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_DataEmpty"));
                }
            }
            catch
            {
                //MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Operate_Failed"));
            }
        }

        protected override void OnCommitted()
        {
            EventAggregator.Publish(new ViewNavigationArgs("MaintainRecord", new Dictionary<string, object>() { { "action", "return" } }));
        }
    }
}

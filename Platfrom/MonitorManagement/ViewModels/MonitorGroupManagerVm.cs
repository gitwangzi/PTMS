using Gsafety.PTMS.Monitor.ViewModels;
using Jounce.Core.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Monitor;
using Gsafety.PTMS.BasicPage.Monitor.ViewModels;
namespace Gsafety.PTMS.Monitor.ViewModels
{
    [ExportAsViewModel(MonitorName.MonitorGroupManagerVm)]
    public class MonitorGroupManagerVm : BaseViewModel
    {
        private MoniterGroupManage _MoniterGroupManager;
        public ObservableCollection<MoniterGroup> MonitorGroups
        {
            get { return _MoniterGroupManager.ToGroupList(); }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            _MoniterGroupManager = new MoniterGroupManage();
        }
    }
}

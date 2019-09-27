using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.Application;
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

namespace BaseLib.ViewModels
{
    public class PTMSBaseViewModel : BaseViewModel
    {
        protected string _rolefucs;

        protected bool _initialrole = false;

        protected void Error(string methodname, Exception ex)
        {
            ApplicationContext.Instance.Logger.LogError(methodname, ex);
        }

        protected void Log(string source, string info)
        {
            ApplicationContext.Instance.Logger.Log(LogSeverity.Information, source, info);
        }

        public virtual void OnRoleEvent(ObservableCollection<FuncItem> FuncItems)
        {
            ObservableCollection<FuncItem> items = OnFilterRoleFunc(FuncItems);
            OnInitialUI(items);

            _initialrole = true;
        }

        protected virtual ObservableCollection<FuncItem> OnFilterRoleFunc(ObservableCollection<FuncItem> FuncItems)
        {
            ObservableCollection<FuncItem> items = new ObservableCollection<FuncItem>();
            foreach (var item in FuncItems)
            {
                if (item.ID.Contains(_rolefucs))
                {
                    items.Add(item);
                }
            }

            return items;
        }

        protected virtual void OnInitialUI(ObservableCollection<FuncItem> FuncItems)
        {

        }

        public virtual void OnHelp()
        {

        }

        protected override void InitializeVm()
        {
            base.InitializeVm();

            if (!_initialrole)
            {
                OnRoleEvent(ApplicationContext.Instance.AuthenticationInfo.Role.FuncItems);
            }
        }
    }
}

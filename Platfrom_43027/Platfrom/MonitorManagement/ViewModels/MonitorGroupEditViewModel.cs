using BaseLib.ViewModels;
using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.ChauffeurService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Gsafety.Ant.Monitor.ViewModels
{
    public class MonitorGroupEditViewModel : DetailViewModel<string>
    {
        protected string action;



        public MonitorGroupEditViewModel()
        {
           
        }
       
        

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null ? null : value.Trim();
                if (string.IsNullOrEmpty(_name) || Duplicate(_name))
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = true;
                }
                ValidateName(ExtractPropertyName(() => Name), _name);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Name));
            }
        }


        private bool _isenable = false;
        public bool IsEnabled
        {
            get { return _isenable; }
            set
            {
                _isenable = value;

                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
            }
        }
        private bool Duplicate(string value)
        {
            foreach (var item in ApplicationContext.Instance.BufferManager.MonitorGroupManager.MoniterGroupManagerOC)
            {
                if ((item.GroupName ==Name)) return true;
            }
            return false;
        }
        private void ValidateName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(prop))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {
                if (Duplicate(Name))
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("Monitor_GroupNameDuplication"));
                
                }
            }
        }
    }
}

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
    public class MonitorNoteEditViewModel : DetailViewModel<string>
    {
        protected string action;

      

        public MonitorNoteEditViewModel()
        {
           
        }
        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                _note = value == null ? null : value.Trim();
                if (string.IsNullOrEmpty(_note))
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = true;
                }
                ValidateName(ExtractPropertyName(() => Note), _note);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
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
        private void ValidateName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(prop))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
    }
}

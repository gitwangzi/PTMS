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
    public class MonitorEmailEditViewModel : DetailViewModel<string>
    {
        protected string action;
        Regex regex = new Regex(@"^([\d\w_]+[\d\w_.]*)@([0-9a-zA-Z_])+(.[\d\w_])*", RegexOptions.IgnoreCase);


        public MonitorEmailEditViewModel()
        {
           
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value == null ? null : value.Trim();
                if (string.IsNullOrEmpty(_mail) || string.IsNullOrEmpty(_name) || !regex.Match(_mail).Success)
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

        private string _mail;
        public string Mail
        {
            get { return _mail; }
            set
            {
                _mail = value == null ? null : value.Trim();
                if (string.IsNullOrEmpty(_mail) || string.IsNullOrEmpty(_name) || !regex.Match(_mail).Success)
                {
                    IsEnabled = false;
                }
                else
                {
                    IsEnabled = true;
                }
                ValidateMail(ExtractPropertyName(() => Mail), _mail);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Mail));
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
            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
        }
        private void ValidateMail(string prop, string value)
        {
          
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(LProxy.NotNull));
            }
            else
            {
                if (!regex.Match(value.Trim()).Success)
                {

                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("EmailUnright"));
                }                              
            }
        }
    }
}

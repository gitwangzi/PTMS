using BaseLib.ViewModels;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    public class ResetPasswordViewModel : DetailViewModel<GUser>
    {
        private string _username;
        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value == null ? null : value.Trim();
                ValidateUserName(ExtractPropertyName(() => UserName), _username);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UserName));
            }
        }

        private void ValidateUserName(string prop, string value)
        {
            ValidateRequire(prop, value);
        }

        private string _firstPassword;
        public string FirstPassword
        {
            get { return _firstPassword; }
            set
            {
                _firstPassword = value == null ? null : value.Trim();
                ValidatePassword(ExtractPropertyName(() => FirstPassword), _firstPassword);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FirstPassword));
            }
        }

        private void ValidatePassword(string prop, string value)
        {
            ValidateRequire(prop, value);
            if (string.IsNullOrWhiteSpace(FirstPassword) == false && string.IsNullOrWhiteSpace(SecondPassword) == false)
            {
                if (FirstPassword != SecondPassword)
                {
                    base.SetError(ExtractPropertyName(() => SecondPassword), ApplicationContext.Instance.StringResourceReader.GetString("NotUnified"));
                }
                else
                {
                    base.ClearErrors(ExtractPropertyName(() => SecondPassword));
                }
            }
        }

        private string _secondPassword;
        public string SecondPassword
        {
            get { return _secondPassword; }
            set
            {
                _secondPassword = value == null ? null : value.Trim();
                ValidatePassword(ExtractPropertyName(() => SecondPassword), _secondPassword);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SecondPassword));
            }
        }
    }
}

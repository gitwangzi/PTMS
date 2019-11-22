using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.Net;
using System.Text.RegularExpressions;
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
    public class DetailViewModel<T> : PTMSBaseViewModel where T : class
    {
        protected string action;
        protected virtual void Reset() { }
        protected virtual void Return() { }

        #region Title
        private string _title = string.Empty;
        public string Title
        {
            get
            {
                return _title;
            }
            protected set
            {
                if (string.Equals(_title, value) == false)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }
        #endregion

        #region IsReadOnly

        private bool _isReadonly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadonly;
            }
            protected set
            {
                _isReadonly = value;
                RaisePropertyChanged(() => IsReadOnly);

                Enable = !IsReadOnly;
            }
        }

        private bool _isEnable;
        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            protected set
            {
                _isEnable = value;
                RaisePropertyChanged(() => IsEnable);
            }
        }

        #endregion

        #region Enable
        private bool _enable = false;
        public bool Enable
        {
            get
            {
                return _enable;
            }
            set
            {
                if (_enable != value)
                {
                    _enable = value;
                    RaisePropertyChanged(() => Enable);
                }
            }
        }
        #endregion

        #region ViewVisibility
        private Visibility _viewVisibility;
        public Visibility ViewVisibility
        {
            get { return _viewVisibility; }
            set
            {
                if (_viewVisibility != value)
                {
                    _viewVisibility = value;
                    RaisePropertyChanged(() => ViewVisibility);
                }
            }
        }
        #endregion

        protected T _initialModel;
        protected T InitialModel
        {
            get { return _initialModel; }
            set
            {
                _initialModel = value;
                RaisePropertyChanged(() => InitialModel);
            }
        }

        private T _currentModel;
        public T CurrentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                RaisePropertyChanged(() => CurrentModel);
            }
        }

        public IActionCommand ResetCommand { get; protected set; }
        public IActionCommand ReturnCommand { get; protected set; }
        public IActionCommand SaveCommand { get; protected set; }

        public DetailViewModel()
        {
            ResetCommand = new ActionCommand<object>(obj => Reset());
            ReturnCommand = new ActionCommand<object>(obj => Return());
            SaveCommand = new ActionCommand<object>(obj => Save());
        }

        protected virtual void Save()
        {
        }

        protected bool ValidateRequire(string prop, string value)
        {
            bool isSuccess = true;
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));
                isSuccess = false;
            }
            return isSuccess;
        }


        Regex regex = new Regex(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z]{2,6}$", RegexOptions.IgnoreCase);
        protected void ValidateEmailFormat(string prop, string value)
        {
            ClearErrors(prop);
            if (!string.IsNullOrEmpty(value) && !regex.Match(value).Success)
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"));//格式非法
            }
        }

        protected void ValidateIntFormat(string prop, string value)
        {
            ClearErrors(prop);
            int result = 0;

            if (!int.TryParse(value, out result))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }

        protected void ValidatePosIntFormat(string prop, string value)
        {
            ClearErrors(prop);
            int result = 0;

            if (!int.TryParse(value, out result) || result < 0)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }

        protected void ValidateLongFormat(string prop, string value)
        {
            ClearErrors(prop);
            long result = 0;

            if (!long.TryParse(value, out result))
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }

        public void ValidateBeginAndEndDate(string begin, DateTime? beginValue, string end, DateTime? endValue)
        {
            if (end == null)
            {
                ClearErrors(begin);
                if (beginValue == null)
                {
                    base.SetError(begin, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.notnull));

                }
            }
            else
            {
                if (beginValue != null && endValue != null)
                {
                    ClearErrors(begin);
                    ClearErrors(end);

                    if (beginValue > endValue)
                    {
                        base.SetError(begin, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                        base.SetError(end, ApplicationContext.Instance.StringResourceReader.GetString("TimeError"));
                    }
                }
            }

        }

        protected virtual void ValidatePhone(string prop, string value)
        {
            ClearErrors(prop);
            bool flag = true;
            if (!string.IsNullOrEmpty(value))
            {
                int num = 0;
                char[] cc = value.ToCharArray();
                foreach (var item in cc)
                {
                    if (item != ' ' && item != '-' && item != '(' && item != ')')
                        flag = flag && int.TryParse(item.ToString(), out num);
                }
            }

            if (!flag)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
            else
            {
                if (value != null && value.Length > 12)
                {
                    base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("PhoneOverLength"));
                }
            }
        }
    }
}

using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Controls
{
    public partial class ChildWindowWithCheck : ChildWindow, INotifyDataErrorInfo, INotifyPropertyChanged
    {
        public ChildWindowWithCheck()
        {
            InitializeComponent();
            this._errors = new Dictionary<string, IEnumerable<string>>();
        }
        protected virtual void ClearErrors(string propertyName)
        {
            this.SetErrors(propertyName, new List<string>());
        }

        protected virtual void SetError(string propertyName, string error)
        {
            List<string> propertyErrors = new List<string>();
            propertyErrors.Add(error);
            this.SetErrors(propertyName, propertyErrors);
        }



        protected virtual void SetErrors(string propertyName, IEnumerable<string> propertyErrors)
        {
            IEnumerable<string> enumerable;
            if (propertyErrors.Any<string>(error => error == null))
            {
                // throw new ArgumentException(Resources.BaseViewModel_SetErrors_NoNullErrors, "propertyErrors");
            }
            string str = propertyName ?? string.Empty;
            if (this._errors.TryGetValue(str, out  enumerable))
            {
                if (!_AreErrorCollectionsEqual(enumerable, propertyErrors))
                {
                    if (propertyErrors.Any<string>())
                    {
                        this._errors[str] = propertyErrors;
                    }
                    else
                    {
                        this._errors.Remove(str);
                    }
                    this.RaiseErrorsChanged(str);
                }
            }
            else if (propertyErrors.Any<string>())
            {
                this._errors[str] = propertyErrors;
                this.RaiseErrorsChanged(str);
            }
        }

        private static bool _AreErrorCollectionsEqual(IEnumerable<string> propertyErrors, IEnumerable<string> currentPropertyErrors)
        {
            IEnumerable<bool> source = currentPropertyErrors.Zip<string, string, bool>(propertyErrors, (current, newError) => current == newError);
            if (propertyErrors.Count<string>() != currentPropertyErrors.Count<string>())
            {
                return false;
            }
            return source.All<bool>(b => b);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            add
            {
                this.ErrorsChanged += value;
            }
            remove
            {
                this.ErrorsChanged -= value;
            }
        }

        private readonly Dictionary<string, IEnumerable<string>> _errors;
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            IEnumerable<string> enumerable;
            if (!this._errors.TryGetValue(propertyName ?? string.Empty, out enumerable))
            {
                return null;
            }
            return enumerable;

        }



        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            EventHandler<DataErrorsChangedEventArgs> errorsChanged = this.ErrorsChanged;
            if (errorsChanged != null)
            {
                errorsChanged.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        public bool HasErrors
        {
            get { return _errors.Any(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}


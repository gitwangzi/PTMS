using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.ServiceReference.AppConfigService
{
    public partial class ConfigItem : INotifyDataErrorInfo
    {
        private Dictionary<string, string> _errorMap = new Dictionary<string, string>();

        public Dictionary<string, string> ErrorMap
        {
            get { return _errorMap; }
            set { _errorMap = value; }
        }
        public ConfigItem()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SectionName = "Section1";

        }

        public void AddError(string propName, string error)
        {


            if ((_errorMap ?? new Dictionary<string, string>()).ContainsKey(propName))
            {
                _errorMap[propName] = error;
            }
            else
            {
                _errorMap.Add(propName, error);
            }
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propName));
            }

        }

        public void RemoveError(string propName)
        {
            if ((_errorMap ?? new Dictionary<string, string>()).Remove(propName))
            {
                if (ErrorsChanged != null)
                {
                    ErrorsChanged(this, new DataErrorsChangedEventArgs(propName));
                }
            }

        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            string result = "";
            (_errorMap ?? new Dictionary<string, string>()).TryGetValue(propertyName, out result);
            return new string[] { result };
        }

        public bool HasErrors
        {
            get { return _errorMap.Count > 0; }
        }
    }


}
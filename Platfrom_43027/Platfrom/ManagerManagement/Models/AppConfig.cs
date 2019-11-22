using Gsafety.Common.CommMessage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.Models
{
    public class AppConfigs : INotifyPropertyChanged, INotifyDataErrorInfo 
    {
        private Dictionary<string, List<string>> _errorMap = new Dictionary<string, List<string>>();
        public string SECTION_DESC { get; set; }

        public string SECTION_UNIT { get; set; }

        public string SECTION_NAME { get; set; }

        public int Index { get; set; }

        private string m_SECTION_VALUE;

        public string SECTION_VALUE
        {
            get
            {
                return m_SECTION_VALUE;
            }
            set
            {
                var  isValid=ValidConfigValue(value);
                RaiseErrorsChanged("SECTION_VALUE");
                if (!isValid)
                {
                    OperatorVisible = Visibility.Collapsed;
                }
                else
                {
                    m_SECTION_VALUE = value;
                    OnPropertyChanged("SECTION_VALUE");
                    OperatorVisible = Visibility.Visible;
                }
            }

        }
        private PredefinedColor _CurrentColor;

        public PredefinedColor CurrentColor
        {
            get { return _CurrentColor; }
            set { _CurrentColor = value; }
        }
        private ObservableCollection<PredefinedColor> _ColorList;

        public ObservableCollection<PredefinedColor> ColorList
        {
            get { return _ColorList; }
            set { _ColorList = value; }
        }
        
        private Visibility _operatorVisible;
        public Visibility OperatorVisible
        {
            get { return _operatorVisible; }
            set
            {
                if (_operatorVisible != value)
                {
                    _operatorVisible = value;
                    OnPropertyChanged("OperatorVisible");
                }
            }
        }

        private void OnPropertyChanged(string propName)
        {
            if (!string.IsNullOrWhiteSpace(propName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

        }

        private bool ValidConfigValue(string v)
        {
            bool result = true;
            var msg = string.Format("{0}", "The configuration must be greater then 0");
            int tem;
            if (!int.TryParse(v, out tem) || tem < 0)
            {
                result = false;
                if (!this._errorMap.ContainsKey("SECTION_VALUE"))
                {
                    this._errorMap.Add("SECTION_VALUE", new List<string>());
                }

                _errorMap["SECTION_VALUE"].Add(msg);
            }
            else
            {
                _errorMap.Remove("SECTION_VALUE");
            }

            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (_errorMap.ContainsKey(propertyName ?? ""))
            {
                return _errorMap[propertyName];
            }
            return null;
        }

        public bool HasErrors
        {
            get { return _errorMap.Count > 0; }
        }
        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}

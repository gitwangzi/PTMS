/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: a1f1f7a5-2bd6-4669-b451-24b832c21d4b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: Vm_Base
/////          Class Version: v1.0.0.0
/////            Create Time: 2014-01-15 10:04:29
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014-01-15 10:04:29
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gsafety.PTMS.BasicPage.VideoDisplay.ViewModels
{
    public abstract class Vm_Base : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> _errorMap = new Dictionary<string, List<string>>();
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            List<string> result = null;
            _errorMap.TryGetValue(propertyName, out result);
            return result;
        }
        public bool HasErrors
        {
            get { return _errorMap.Count > 0; }
        }

        protected virtual void OnErrorsChanged(string propName)
        {
            if (this.ErrorsChanged != null && !string.IsNullOrWhiteSpace(propName))
            {
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propName));
            }
        }
        protected virtual void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null && !string.IsNullOrWhiteSpace(propName))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        protected virtual void AddError(string propName, string msg)
        {
            if (!this._errorMap.ContainsKey(propName))
            {
                this._errorMap.Add(propName, new List<string>());
            }
            this._errorMap[propName].Add(msg);
            this.OnErrorsChanged(propName);
        }

        protected virtual void RemoveError(string propName)
        {
            this._errorMap.Remove(propName);
            this.OnErrorsChanged(propName);
        }
    }
}

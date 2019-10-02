using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels
{
    public class SystemLogBaseViewModel<T> : ListViewModel<T>
    {
        public IActionCommand ExportCommand { get; protected set; }
        public IActionCommand LogQueryCommand { get; protected set; }

        public SystemLogBaseViewModel()
        {
            ExportCommand = new ActionCommand<object>(obj => ExportAction(obj));
            LogQueryCommand = new ActionCommand<object>(obj => LogQueryAction(obj));
        }

        #region Select Condition
        private DateTime? beginTime = DateTime.Now.Date.AddMonths(-1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? BeginTime
        {
            get
            {
                return beginTime;
            }
            set
            {
                this.beginTime = value;
                if (BeginTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginTime), (DateTime)BeginTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.BeginTime);
            }
        }
        private DateTime? endTime = DateTime.Now.Date;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                this.endTime = value;
                if (BeginTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginTime), (DateTime)BeginTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.EndTime);
            }
        }

        #endregion

        public bool ExportBtnStatus { get; set; }
        protected void setExportBtnStatus(bool Flag)
        {
            ExportBtnStatus = Flag;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => ExportBtnStatus));
        }

        protected virtual void ExportAction(object obj)
        {
        }

        protected virtual void LogQueryAction(object obj)
        {
           
        }

        protected bool ExportDate()
        {
            if (BeginTime > EndTime)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"),
                        ApplicationContext.Instance.StringResourceReader.GetString("LogQueryDateCondition"), MessageDialogButton.Ok);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

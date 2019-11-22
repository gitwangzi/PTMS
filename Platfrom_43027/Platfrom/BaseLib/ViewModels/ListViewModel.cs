using BaseLib.Model;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
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

namespace BaseLib.ViewModels
{
    public class ListViewModel<T> : PTMSBaseViewModel
    {
        public ListViewModel()
        {
            InitPagination();
            pagesizelist = BaseCommon.PageSizeList;
            PageSizeValue = PageSizeList[1];

            BtnSearchCommand = new ActionCommand<object>(q => Query());
            BtnAddCommand = new ActionCommand<object>(method => Add("add"));
            BtnEditCommand = new ActionCommand<object>(method => Update("update"));
            BtnDeleteCommand = new ActionCommand<object>(method => Delete());
            BtnViewDetailCommand = new ActionCommand<object>(method => ViewDetail("view"));

        }

        protected virtual void ViewDetail(string actionName)
        {
        }

        protected virtual void Delete()
        {
        }

        protected virtual void Update(string actionName)
        {
        }

        protected virtual void Add(string actionName)
        {
        }

        protected virtual void Query()
        {
        }

        protected virtual void InitPagination()
        {
        }


        /// <summary>
        /// 列表数据源
        /// </summary>
        public PagedServerCollection<T> Data
        {
            get;
            set;
        }

        protected List<int> pagesizelist = null;

        public List<int> PageSizeList
        {
            get
            {
                return pagesizelist;
            }
            set
            {
                pagesizelist = value;
            }
        }

        /// <summary>
        /// View每页显示数据条数
        /// </summary>
        private int pageSizeValue;
        public int PageSizeValue
        {
            get { return pageSizeValue; }
            set
            {
                pageSizeValue = value;
                if (this.Data != null)
                {
                    this.Data.PageSize = pageSizeValue;
                }
            }
        }

        public int TotalCount
        {
            get
            {
                return this.Data.TotalItemCount;
            }
        }

        public ICommand BtnSearchCommand { get; set; }
        public ICommand BtnAddCommand { get; set; }
        public ICommand BtnEditCommand { get; set; }
        public ICommand BtnDeleteCommand { get; set; }
        public ICommand BtnViewDetailCommand { get; set; }

        protected int currentIndex = 1;

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            if (Data != null)
            {
                Data.RefreshPage();
            }
        }

        public void ValidateBeginAndEndDate(string begin, DateTime beginValue, string end, DateTime endValue)
        {
            //if (beginValue > endValue)
            //{
            //    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("LogQueryDateCondition"), MessageDialogButton.Ok);
            //}

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

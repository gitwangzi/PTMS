/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 01bb5d64-e9a0-4494-bd1a-7cefddb0eaa2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation
/////    Project Description:    
/////             Class Name: PagedServerCollection
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 10:45:18
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 10:45:18
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace Gsafety.PTMS.Maintain
{
    public class PagedServerCollection<T> : IPagedCollectionView, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable
    {
        private Action<int, int> _asyncLoadDataMethod;

        public PagedServerCollection(Action<int, int> asyncLoadDataMethod)
        {
            _asyncLoadDataMethod = asyncLoadDataMethod;
            _PageSize = 0;
            _PageIndex = 1;
            _ItemCount = 0;
        }


        #region IPagedCollectionView
        public event EventHandler<EventArgs> PageChanged;
        public event EventHandler<PageChangingEventArgs> PageChanging;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region INotifyCollectionChanged
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        #endregion

        #region Event Invoker

        void OnPageChanged()
        {
            if (PageChanged != null)
                PageChanged(this, EventArgs.Empty);
        }
        /// <summary>
        /// 出发PageChanging时间，用户可以设置e.Cancel取消
        /// </summary>
        /// <returns>翻页是否继续执行</returns>
        bool OnPageChanging()
        {
            if (PageChanging == null)
                return true;
            var e = new PageChangingEventArgs(_PageIndex);
            _IsPageChanging = true;
            PageChanging(this, e);
            _IsPageChanging = false;
            return !e.Cancel;
        }
        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        void OnCollectionChanged()
        {
            if (CollectionChanged != null)
            {
                //NotifyCollectionChangedAction.Reset：集合中的内容显著更改
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }



        #endregion


        int _PageIndex;
        int _PageSize;
        int _ItemCount;
        bool _IsPageChanging;
        IEnumerable<T> _items = new List<T>();

        /// <summary>
        /// 自定义参数，如:检索条件等
        /// </summary>

        public int PageIndex
        {
            get { return _PageIndex - 1; }
        }
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value > 0 ? value : _PageSize;
                OnPropertyChanged("PageSize");
                MoveToFirstPage();
            }
        }
        public int ItemCount
        {
            get { return _ItemCount; }
        }
        public int TotalItemCount
        {
            get { return _ItemCount; }
        }
        public int PageCount
        {
            get { return (int)Math.Ceiling(_ItemCount / (double)_PageSize); }
        }
        public bool CanChangePage
        {
            get { return PageCount > 0; }
        }
        public bool IsPageChanging
        {
            get { return _IsPageChanging; }
        }

        public bool MoveToFirstPage()
        {
            return ToPage(-1);
        }

        public bool MoveToLastPage()
        {
            return ToPage(PageCount);
        }

        public bool MoveToNextPage()
        {
            return ToPage(_PageIndex + 1);
        }

        public bool MoveToPage(int pageIndex)
        {
            return ToPage(pageIndex + 1);
        }

        public bool MoveToPreviousPage()
        {
            return ToPage(_PageIndex - 1);
        }

        /// <summary>
        /// 跳转到某页
        /// </summary>
        /// <param name="page">页码</param>
        /// <returns></returns>
        public bool ToPage(int page)
        {
            if (page > PageCount)
            {

                page = PageCount;
            }
            if (page <= 0)
            {
                page = 1;
            }
            _PageIndex = page;

            if (!OnPageChanging())
                return false;
            _asyncLoadDataMethod(_PageIndex, _PageSize);

            //监听Finished事件          
            return true;
        }

        public void loader_Finished(PagedResult<T> pageResult)
        {
            if (pageResult == null)
                return;
            //变更总页数
            if (_ItemCount != pageResult.Count)
            {
                _ItemCount = pageResult.Count;

                OnPropertyChanged("ItemCount");
                OnPropertyChanged("TotalItemCount");
                OnPropertyChanged("CanChangePage");
            }

            _items = pageResult.Items;
            _PageIndex = pageResult.PageIndex;
            //变更页码
            OnPropertyChanged("PageIndex");
            //内容变更，刷新数据
            OnCollectionChanged();

        }

        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        #endregion
    }
}

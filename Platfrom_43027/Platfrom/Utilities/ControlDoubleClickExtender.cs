/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3c3301ee-7362-4d04-beae-d82b832a7b46      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: SilverlightApplication11
/////    Project Description:    
/////             Class Name: ControlDoubleClickExtender
/////          Class Version: v1.0.0.0
/////            Create Time: 2014-01-02 11:04:55
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014-01-02 11:04:55
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using System.Windows.Threading;
using System.Linq;

namespace Gsafety.Common.Utilities
{
    /// <summary>
    /// ControlDoubleClickExtender
    /// </summary>
    public static class ControlDoubleClickExtender
    {
        internal static TimeSpan HookTime { get; private set; }
        private static System.Collections.Generic.List<HookControlState> _controlList = new System.Collections.Generic.List<HookControlState>();
        private static object _asyncObj = new object();
        static ControlDoubleClickExtender()
        {
            HookTime = TimeSpan.FromMilliseconds(300);
        }

        /// <summary>
        /// add Double click
        /// </summary>
        /// <param name="control">control</param>
        /// <param name="doubleClickCallBack">callback</param>
        public static void DoubleClickHook(this FrameworkElement control, Action<object> doubleClickCallBack)
        {
            lock (_asyncObj)
            {

                if (_controlList.Any(x => x.Control == control))
                {
                    return;
                }
                _controlList.Add(new HookControlState(control, doubleClickCallBack));
            }
        }

        /// <summary>
        /// UnDoubleClickHook
        /// </summary>
        /// <param name="control"></param>
        public static void UnDoubleClickHook(FrameworkElement control)
        {
            lock (_asyncObj)
            {
                var state = _controlList.FirstOrDefault(x => x.Control == control);
                if (state != null)
                {
                    _controlList.Remove(state);
                    state.Dispose();
                }
            }
        }

        #region
        class HookControlState : IDisposable
        {
            public FrameworkElement Control { get; set; }
            private Action<object> _callBack;
            private MouseButtonEventHandler _leftDownHandler;
            private DateTime _preClickTime;
            private bool _isDisposed;
            public HookControlState(FrameworkElement control, Action<object> _dbCallBack)
            {
                this.Control = control;
                this._callBack = _dbCallBack;
                InitializeComponent();
            }
            private void InitializeComponent()
            {
                _leftDownHandler = new MouseButtonEventHandler(Control_MouseLeftButtonDown);
                this.Control.AddHandler(UIElement.MouseLeftButtonDownEvent, _leftDownHandler, true);
            }


            void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                var tp = DateTime.Now - _preClickTime;
                if (tp <= ControlDoubleClickExtender.HookTime)
                {
                    if (_callBack != null)
                    {
                        _callBack(e.OriginalSource);
                    }
                }
                _preClickTime = DateTime.Now;
            }

            private void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    if (disposing)
                    {
                        this.Control.RemoveHandler(UIElement.MouseLeftButtonDownEvent, _leftDownHandler); 
                        this.Control = null;
                        _leftDownHandler = null; 
                    }
                }
                _isDisposed = true;

            }
            public void Dispose()
            {

                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}

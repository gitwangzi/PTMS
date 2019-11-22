/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 3435c827-b95e-4906-8095-435b354ec0d6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BasicPage
/////    Project Description:    
/////             Class Name: HistoryVideoFileItem
/////          Class Version: v1.0.0.0
/////            Create Time: 2014-01-15 09:56:56
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014-01-15 09:56:56
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
using Gsafety.PTMS.ServiceReference.VedioService;

namespace Gsafety.PTMS.BasicPage.VideoDisplay.ViewModels
{
    public class HistoryVideoFileItem : Vm_Base
    {
        public QueryServerFileListMessage Message;

        private string _timeStemp;
        /// <summary>
        /// TimeSpan
        /// </summary>
        public string TimeStemp
        {
            get { return _timeStemp; }
            set
            {
                if (_timeStemp != value)
                {
                    _timeStemp = value;
                    OnPropertyChanged("TimeStemp");
                }
            }
        }


        private string _playUrl;

        public string PlayUrl
        {
            get
            {
                return _playUrl;
            }
            set
            {
                if (_playUrl != value)
                {
                    _playUrl = value;
                    OnPropertyChanged("PlayUrl");
                }
            }
        }

        private string _channel;
        /// <summary>
        /// Channel
        /// </summary>
        public string Channel
        {
            get { return _channel; }
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    OnPropertyChanged("Channel");
                }
            }
        }


        private bool _isPlay;
        public bool IsPlay
        {
            get { return _isPlay; }
            set
            {
                if (_isPlay != value)
                {
                    _isPlay = value;
                    OnPropertyChanged("IsPlay");
                }
            }
        }


        private string _size;

        /// <summary>
        ///Size
        /// </summary>
        public string Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    OnPropertyChanged("Size");
                }
            }
        }
    }
}

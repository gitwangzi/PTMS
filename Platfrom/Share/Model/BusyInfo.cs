/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 384dff9d-b301-4a41-a3b1-638113764442      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Model
/////    Project Description:    
/////             Class Name: BusyInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 6:52:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 6:52:48 PM
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
using Jounce.Core.Model;

namespace Gsafety.PTMS.Share
{
    public class BusyInfo : BaseNotify
    {
        #region Fields

        private bool _IsBusy;
        private string _BusyContent;
        private int _InitLoadingNum = 0;
        object lockobject = new object();


        #endregion

        #region Attribute


        public bool IsInitComplete
        {
            get
            {
                return !(_InitLoadingNum > 0);
            }
        }


        public int InitLoadingNum
        {
            get { return _InitLoadingNum; }
            set
            {
                lock (lockobject)
                {
                    _InitLoadingNum = value;
                }
            }
        }

        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                try
                {
                    _IsBusy = value;
                    RaisePropertyChanged(() => IsBusy);
                }
                catch
                {
                }
            }
        }

        public string BusyContent
        {
            get
            {
                return _BusyContent;
            }
            set
            {
                _BusyContent = value;
                RaisePropertyChanged(() => BusyContent);

            }
        }

        public BusyInfo()
        {
            _BusyContent = ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_LoadingContent");
        }

        #endregion


    }
}

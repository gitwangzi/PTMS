/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4fb6e040-7c4c-4b36-8896-e9786363c5d1      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInfo
/////    Project Description:    
/////             Class Name: MessageHeaderUserInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 10/12/2013 2:02:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 10/12/2013 2:02:48 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.BaseInfo
{
    public class MessageHeaderUserInfo
    {
        #region Fields

        private string _UserName;
        private string _Password;
        private short? _UserType = 0;
        private string _Regions = "00|01";
        private string _group;
        private string _pri;
        private string _city;
        #endregion

        #region Attributes

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public short? UserType
        {
            get { return _UserType; }
            set { _UserType = value; }
        }

        public string Regions
        {
            get { return _Regions; }
            set { _Regions = value; }
        }
        #endregion
    }
}

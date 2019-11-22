/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 393a4bbb-c7f4-4086-a7e2-7a7baf6f276f      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: AuthenticationInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 7/26/2013 4:21:52 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/26/2013 4:21:52 PM
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
using MessageService = Gsafety.PTMS.ServiceReference.MessageService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.ADUserInfoService;
using System.Xml.Serialization;
using System.IO;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.RunMonitorGroupService;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.PTMS.Share
{
    public class AuthenticationInfo : BaseNotify
    {
        #region Fields

        private string _UserName;
        private string _UserShowName;
        private string _Password;
        private string _Description;
        private string _Message;
        private bool _IsBusy;
        private UserInfoMessageHeader _UserInfoMessagerHeader;
        private string _MessageHeader;
        private string _userid;
        #endregion Fields

        #region Attributes
        public string UserID
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
                RaisePropertyChanged("UserID");
            }
        }

        private string _department;
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
                RaisePropertyChanged("Department");
            }
        }

        private string _clientid;
        public string ClientID
        {
            get
            {
                return _clientid;
            }
            set
            {
                _clientid = value;
                RaisePropertyChanged("ClientID");
            }
        }



        private string _account;
        public string Account
        {
            get
            {
                return _account;
            }
            set
            {
                _account = value;
                RaisePropertyChanged("Account");
            }
        }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string UserShowName
        {
            get
            {
                return _UserShowName;
            }
            set
            {
                _UserShowName = value;
                RaisePropertyChanged("UserShowName");
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                RaisePropertyChanged("Password");
            }
        }

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                RaisePropertyChanged("Description");
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
                _IsBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        short _transfermode = 0;
        public short TransferMode
        {
            get
            {
                return _transfermode;
            }
            set
            {
                _transfermode = value;
                RaisePropertyChanged("TransferMode");
            }
        }

        private Role _role;
        public Role Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                RaisePropertyChanged("Role");
            }
        }

        List<InstallStation> stations = null;

        public List<InstallStation> Stations
        {
            get { return stations; }
            set { stations = value; }
        }

        public bool IsClientCreate
        { get; set; }

        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                RaisePropertyChanged("Message");
            }
        }

        public string Phone
        {
            get;
            set;
        }

        public string Mobile
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string FunctionNames
        {
            get;
            set;
        }

        public bool MonitorFunction
        {
            get
            {
                if (Role.FuncItems.Contains(""))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AlarmFunction
        {
            get
            {
                if (Role.FuncItems.Contains("02-01-01-02"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AlertFunction
        {
            get
            {
                if (Role.RoleCategory == (short)RoleCategory.MaintainAdmin || Role.RoleCategory == (short)RoleCategory.MaintainMonitor)
                {
                    return false;
                }
                else
                {
                    return true;
                }

                if (string.IsNullOrEmpty(FunctionNames))
                    return false;
                if (FunctionNames.Contains("MAINPAGE_Alert"))
                    return true;
                return false;
            }
        }

        public bool TrafficFunction
        {
            get
            {
                if (string.IsNullOrEmpty(FunctionNames))
                    return false;
                if (FunctionNames.Contains("MAINPAGE_Traffic"))
                    return true;
                return false;
            }
        }

        public ObservableCollection<Organization> Organizations
        {
            get;
            set;
        }
        #endregion Attributes

        public string MessageHeader
        {
            get
            {
                if (string.IsNullOrEmpty(_MessageHeader))
                {
                    if (_UserInfoMessagerHeader == null)
                    {
                        if (string.IsNullOrEmpty(Account))
                        {
                            return string.Empty;
                        }
                        _UserInfoMessagerHeader = new UserInfoMessageHeader();
                        _UserInfoMessagerHeader.UserName = Account;
                    }
                    XmlSerializer serializer = new XmlSerializer(typeof(UserInfoMessageHeader));
                    StringWriter textWriter = new StringWriter();
                    serializer.Serialize(textWriter, _UserInfoMessagerHeader);
                    textWriter.Close();
                    _MessageHeader = textWriter.ToString();
                }
                return _MessageHeader;
            }
        }
    }
}

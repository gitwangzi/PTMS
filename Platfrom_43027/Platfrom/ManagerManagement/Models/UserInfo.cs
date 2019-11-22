/////Copyright (C)2014Microsoft All Rights Reserved.
/////======================================================================
/////                   Guid: 5fd5f106-7182-4262-aea1-f3d1239ada27      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-JIANGJ
/////                 Author: TEST(JiangJ)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Models
/////    Project Description:    
/////             Class Name: UserInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/7/28 18:07:19
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/7/28 18:07:19
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

namespace Gsafety.PTMS.Manager.Models
{
    public class UserInfo
    {
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string Organization { get; set; }
        public string SuperUser { get; set; }
        public string Phone { get; set; }
        public string LoginDate { get; set; }
        public string Enable { get; set; }
        public string Valid { get; set; }
        public string Note { get; set; }
        public string UserLoginName { get; set; }
        public string UserGroup { set; get; }
        public string UserLevel { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public string ProviceName { get; set; }
        public string CarCompanyName { get; set; }
        public string Support_StationName { get; set; }
        public string SiteCode { get; set; }
        public string CityCode { get; set; }
        public string ProvinceCode { get; set; }
        public string CarCompanyCode { get; set; }
        public int Level { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

    }
}

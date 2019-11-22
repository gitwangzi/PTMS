/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 14e5d5fc-208c-4434-b216-1fda21dccee5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.Common.AD
/////    Project Description:    
/////             Class Name: ADAccountEntility
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/5 9:25:57
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/5 9:25:57
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.Common.AD
{
    /// <summary>
    /// 业务用户实体对象
    /// </summary>
  public  class ADAccountEntity
    {
      /// <summary>
      /// 登录名
      /// </summary>
      public string UserPrincipalName { get; set; }
      /// <summary>
      /// 用户名(Win2000前)
      /// </summary>
      public string UserName { get; set; }
      /// <summary>
      /// 显示名称
      /// </summary>
      public string DisplayName { get; set; }
      /// <summary>
      /// 用户密码
      /// </summary>
      public string UserPassword { get; set; }
      /// <summary>
      /// 电话号码
      /// </summary>
      public string Phone { set; get; }
      /// <summary>
      /// 用户信息描述
      /// </summary>
      public string Description { get; set; }
      /// <summary>
      /// 存在用户org信息
      /// </summary>
      public string Company { get; set; }
      /// <summary>
      /// 传真
      /// </summary>
      public string Fax { get; set; }
      /// <summary>
      /// 安全组
      /// </summary>
      public string SecurityGroup { get; set; }
      /// <summary>
      /// 邮件
      /// </summary>
      public string Email { get; set; }
      /// <summary>
      /// 家庭地址
      /// </summary>
      public string Address { get; set; }

      public override string ToString()
      {
          StringBuilder builder = new StringBuilder();
          if (!string.IsNullOrEmpty(Convert.ToString(UserPrincipalName)))
          {
              builder.AppendLine("UserPrincipalName:" + UserPrincipalName.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(UserName)))
          {
              builder.AppendLine("UserName:" + UserName.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(DisplayName)))
          {
              builder.AppendLine("DisplayName:" + DisplayName.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(UserPassword)))
          {
              builder.AppendLine("UserPassword:" + UserPassword.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Phone)))
          {
              builder.AppendLine("Phone:" + Phone.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Description)))
          {
              builder.AppendLine("Description:" + Description.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Company)))
          {
              builder.AppendLine("Company:" + Company.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Fax)))
          {
              builder.AppendLine("Fax:" + Fax.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(SecurityGroup)))
          {
              builder.AppendLine("SecurityGroup:" + SecurityGroup.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Email)))
          {
              builder.AppendLine("Email:" + Email.ToString());
          }
          if (!string.IsNullOrEmpty(Convert.ToString(Address)))
          {
              builder.AppendLine("Address:" + Address.ToString());
          }
          return builder.ToString();
      }

    }
}

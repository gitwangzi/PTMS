/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 33709c98-4a52-40c9-918c-16cf2e29a51d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Models
/////    Project Description:    
/////             Class Name: PasswordInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 8/28/2013 4:32:55 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/28/2013 4:32:55 PM
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
using Gsafety.PTMS.Share;
using Gsafety.Common.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gsafety.PTMS.MainPage.Models
{
    public class PasswordInfo : NotifyDataErrorInfo
    {
        #region Fields
        private string _UserName = string.Empty;
        private string _CurrentPassword = string.Empty;
        private string _NewPassword = string.Empty;
        private string _ConfirmPassword = string.Empty;

        #endregion

        #region Attributes

        public string UserName
        {
            get { return _UserName; }
        }

        public string PasswordForm
        {
            get
            {
                return ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordNotNull");
            }
        }

         [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
         [RegularExpression(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$",
             ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
             ErrorMessageResourceName = "MAINPAGE_PasswordFormError")]
        public string CurrentPassword
        {
            get { return _CurrentPassword; }
            set
            {

                ClearErrors("CurrentPassword");
                try
                {
                    //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "CurrentPassword" });
                    _CurrentPassword = value;
                }
                catch (ValidationException ex)
                {
                    SetError("CurrentPassword", ex.Message);
                }
            }
        }

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        [StringLength(20,MinimumLength=7,
             ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "PasswordAccordRule")]
        [RegularExpression(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$",
            ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_PasswordFormError")]
        public string NewPassword
        {
            get { return _NewPassword; }
            set 
            {
                ClearErrors("NewPassword");
                try
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "NewPassword" });
                    _NewPassword = value;
                }
                catch (ValidationException ex)
                {
                    SetError("NewPassword", ex.Message);
                }

               // ValidataPasswordContrast();
               
            }
        }

        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set 
            {
                ClearErrors("ConfirmPassword");
                try
                {
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "ConfirmPassword" });
                    _ConfirmPassword = value;
                    ValidataPasswordContrast();
                }
                catch (ValidationException ex)
                {
                    SetError("ConfirmPassword", ex.Message);
                }
            }
        }
        public void ClearError()
        {
            ClearErrors("ConfirmPassword");
        }
        public  void ValidataPasswordContrast()
        {
            
            if (!NewPassword.Equals(ConfirmPassword))
            {
                SetError("ConfirmPassword", ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordDifferError"));
            }
        }

        #endregion

        public PasswordInfo()
        {
            _UserName = ApplicationContext.Instance.AuthenticationInfo.Account;
            string errorContent = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_RequiredError");
            SetError("CurrentPassword", errorContent);
            SetError("NewPassword", errorContent);
            SetError("ConfirmPassword", errorContent);
            
        }
    }
}

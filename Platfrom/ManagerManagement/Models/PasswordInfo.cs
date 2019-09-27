/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 9d0a12ac-7255-46e6-b738-20c1ab966ef6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Models
/////    Project Description:    
/////             Class Name: PasswordInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/11/15 14:19:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/11/15 14:19:17
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;
using Gsafety.Common.Utilities;
namespace Gsafety.PTMS.Manager.Models
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
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "CurrentPassword" });
                    _CurrentPassword = value;
                }
                catch(ValidationException ex)
                {
                    SetError("CurrentPassword", ex.Message);
                }
            }
        }
        [Required(ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_RequiredError")]
        [StringLength(20, MinimumLength = 7,
             ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "PasswordAccordRule")]
        [RegularExpression(@"^(?![0-9a-z]+$)(?![0-9A-Z]+$)(?![0-9\W]+$)(?![a-z\W]+$)(?![a-zA-Z]+$)(?![A-Z\W]+$)[a-zA-Z0-9\W_]+$",
            ErrorMessageResourceType = typeof(Gsafety.Common.Localization.Resource.StringResource),
            ErrorMessageResourceName = "MAINPAGE_PasswordFormError")]
        [CustomValidation(typeof(PasswordValidation), "ValidataPassword")]
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
                catch(ValidationException ex)
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
                catch(ValidationException ex)
                {
                    SetError("ConfirmPassword", ex.Message);
                }
            }
        }
        public void ClearError()
        {
            ClearErrors("ConfirmPassword");
        }
        public void ValidataPasswordContrast()
        {

            if(!NewPassword.Equals(ConfirmPassword))
            {
                SetError("ConfirmPassword", ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_PasswordDifferError"));
            }
        }

        #endregion

        public PasswordInfo(string Name)
        {
            _UserName = Name;
            string errorContent = ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_RequiredError");
            SetError("CurrentPassword", errorContent);
            SetError("NewPassword", errorContent);
            SetError("ConfirmPassword", errorContent);
        }

    }

    public class PasswordValidation
    {
        public static ValidationResult ValidataPassword(string strpwd, ValidationContext context)
        {
            PasswordInfo pwd = context.ObjectInstance as PasswordInfo;
            if(strpwd.ToUpper().Contains(pwd.UserName.ToUpper()))
            {
                return new ValidationResult(ApplicationContext.Instance.StringResourceReader.GetString("PasswordLikeUserName"));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}

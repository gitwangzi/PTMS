/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a3905a32-3292-4fcd-af55-79a2523c7676      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Utilities
/////    Project Description:    
/////             Class Name: NotifyDataErrorInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 8/31/2013 11:00:38 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/31/2013 11:00:38 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Shapes;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;
using System.Collections;
using System.Reflection;

namespace Gsafety.Common.Utilities
{
    public class NotifyDataErrorInfo : INotifyDataErrorInfo
    {
        /// <summary>
        ///     Internal list of errors
        /// </summary>
        private readonly Dictionary<string, IEnumerable<string>> _errors = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        ///     Collction of errors changed
        /// </summary>
        private event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Occurs when the validation errors have changed for a property or for the entire object.
        /// </summary>
        event EventHandler<DataErrorsChangedEventArgs> INotifyDataErrorInfo.ErrorsChanged
        {
            [method:
                SuppressMessage("Microsoft.Design", "CA1033", Justification = "Doesn't need to access event methods.")]
            add { ErrorsChanged += value; }
            [method:
                SuppressMessage("Microsoft.Design", "CA1033", Justification = "Doesn't need to access event methods.")]
            remove { ErrorsChanged -= value; }
        }

        /// <summary>
        ///     True if errors exist
        /// </summary>
        bool INotifyDataErrorInfo.HasErrors
        {
            get { return HasErrors; }
        }

        /// <summary>
        ///     True if errors exist
        /// </summary>
        protected bool HasErrors
        {
            get { return _errors.Any(); }
        }

        /// <summary>
        ///     Get errors for a property
        /// </summary>
        /// <param name="propertyName">The property</param>
        /// <returns>The list of errors</returns>
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName)
        {
            return GetErrors(propertyName);
        }

        /// <summary>
        ///     Get errors for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        protected virtual IEnumerable GetErrors(string propertyName)
        {
            IEnumerable<string> error;
            return _errors.TryGetValue(propertyName ?? string.Empty, out error) ? error : null;
        }

        /// <summary>
        ///     Set an error for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="error">The error</param>
        protected virtual void SetError(string propertyName, string error)
        {
            SetErrors(propertyName, new List<string> { error });
        }

        /// <summary>
        /// Overload for expression
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">An expression that points to the property</param>
        /// <param name="error">The error</param>
        protected virtual void SetError<T>(Expression<Func<T>> propertyExpresssion, string error)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            SetError(propertyName, error);
        }

        /// <summary>
        ///     Clears the errors
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void ClearErrors(string propertyName)
        {
            SetErrors(propertyName, new List<string>());
        }

        /// <summary>
        ///     Clear all errors for a property
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">The expression that points to the property</param>
        protected virtual void ClearErrors<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            ClearErrors(propertyName);
        }

        /// <summary>
        ///     Set errors for a property
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpresssion">The expression for the property</param>
        /// <param name="propertyErrors">The collection of errors</param>
        protected virtual void SetErrors<T>(Expression<Func<T>> propertyExpresssion, IEnumerable<string> propertyErrors)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            SetErrors(propertyName, propertyErrors);
        }

        /// <summary>
        ///     Set errors for a property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="propertyErrors">The collection of errors</param>
        protected virtual void SetErrors(string propertyName, IEnumerable<string> propertyErrors)
        {
            if (propertyErrors.Any(error => error == null))
            {
                //throw new ArgumentException(Core.Resources.BaseViewModel_SetErrors_NoNullErrors, "propertyErrors");
                return;
            }

            var propertyNameKey = propertyName ?? string.Empty;

            IEnumerable<string> currentPropertyErrors;
            if (_errors.TryGetValue(propertyNameKey, out currentPropertyErrors))
            {
                if (!_AreErrorCollectionsEqual(currentPropertyErrors, propertyErrors))
                {
                    if (propertyErrors.Any())
                    {
                        _errors[propertyNameKey] = propertyErrors;
                    }
                    else
                    {
                        _errors.Remove(propertyNameKey);
                    }

                    RaiseErrorsChanged(propertyNameKey);
                }
            }
            else
            {
                if (propertyErrors.Any())
                {
                    _errors[propertyNameKey] = propertyErrors;
                    RaiseErrorsChanged(propertyNameKey);
                }
            }
        }

        /// <summary>
        ///     Raises this object's ErrorsChangedChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has new errors.</param>
        protected virtual void RaiseErrorsChanged(string propertyName)
        {
            var handler = ErrorsChanged;
            if (handler != null)
            {
                handler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        ///     Raises this object's ErrorsChangedChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the property that has errors</typeparam>
        /// <param name="propertyExpresssion">A Lambda expression representing the property that has new errors.</param>
        protected virtual void RaiseErrorsChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = ExtractPropertyName(propertyExpresssion);
            RaiseErrorsChanged(propertyName);
        }

        /// <summary>
        /// Extracts the property name from the property expression
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="propertyExpression">An expression that evaluates to the property</param>
        /// <returns>The property name</returns>
        /// <remarks>
        /// Use this to take an expression like <code>() => MyProperty</code> and evaluate to the
        /// string "MyProperty"
        /// </remarks>
        protected string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
                //throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NotAMember, "propertyExpression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentNullException("propertyExpression");
                //throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NotAProperty, "propertyExpression");
            }

            var getMethod = property.GetGetMethod(true);

            if (getMethod == null)
            {
                throw new ArgumentNullException("propertyExpression");
                // this shouldn't happen - the expression would reject the property before reaching this far
                //throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_NoGetter, "propertyExpression");
            }

            if (getMethod.IsStatic)
            {
                throw new ArgumentNullException("propertyExpression");
                //throw new ArgumentException(Resources.BaseNotify_ExtractPropertyName_Static, "propertyExpression");
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        ///     Compares error collections
        /// </summary>
        /// <param name="propertyErrors">The property errors</param>
        /// <param name="currentPropertyErrors">The current</param>
        /// <returns>True if there are/aren't equal</returns>
        private static bool _AreErrorCollectionsEqual(IEnumerable<string> propertyErrors,
                                                     IEnumerable<string> currentPropertyErrors)
        {
            var equals = currentPropertyErrors.Zip(propertyErrors, (current, newError) => current == newError);
            return propertyErrors.Count() == currentPropertyErrors.Count() && equals.All(b => b);
        }
    }
}
